using System;
using System.Drawing.Imaging;
using System.IO;

using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PdfOperate
{
    class Program
    {
        static void Main(string[] args)
        {
            O2SPdfToImg();
        }

        /*
         * 
         * 以下4个方法都是用iTextSharp来实现，网上也有很多例子
         * 可以搜索学习
         * 
         * 
         */


        #region 给pdf文件重新添加文字内容
        private static bool AddText()
        {
            PdfReader reader = new PdfReader(@"E:\pdf\156281489401000002.pdf");

            FileStream out1 = new FileStream(@"E:\pdf\test.pdf", FileMode.Create, FileAccess.Write);

            PdfStamper stamp = new PdfStamper(reader, out1);
            try
            {
                //获得pdf总页数
                int count = reader.NumberOfPages;

                //设置文本域  iTextSharp.text.Rectangle(105, 100, 240, 125) 用来设置文本域的位置，四个参数分别为：llx、lly、urx、ury：
                //llx 为Left ，
                //lly 为Bottom, 
                //urx 为Right，
                //ury 为Top       其中：Width = Right - Left     Heigth = Top - Bototom

                TextField fieldDate = new TextField(stamp.Writer, new iTextSharp.text.Rectangle(460, 430, 540, 475), "date");
                fieldDate.BackgroundColor = BaseColor.WHITE;
                fieldDate.BorderWidth = 1;
                fieldDate.BorderColor = BaseColor.BLACK;
                fieldDate.BorderStyle = 4;
                fieldDate.FontSize = 11f;
                stamp.AddAnnotation(fieldDate.GetTextField(), 9);

                //创建文本
                Chunk y_identity = new Chunk("912365165354699654", FontFactory.GetFont("Futura", 11f, new BaseColor(0, 0, 0)));
                Chunk y_phone = new Chunk("96545632233", FontFactory.GetFont("Futura", 11f, new BaseColor(0, 0, 0)));
                Phrase p_y_identity = new Phrase(y_identity);
                Phrase p_y_phone = new Phrase(y_phone);

                //PdfContentBye类，用来设置图像和文本的绝对位置
                PdfContentByte over = stamp.GetOverContent(2);
                ColumnText.ShowTextAligned(over, Element.ALIGN_CENTER, p_y_identity, 240, 700, 0);
                ColumnText.ShowTextAligned(over, Element.ALIGN_CENTER, p_y_phone, 210, 604, 0);

                stamp.FormFlattening = true;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                stamp.Close();
                out1.Close();
                reader.Close();
            }
        }
        #endregion

        #region 给pdf文件设置遮挡水印
        /// <summary>
        /// 设置pdf图片水印
        /// </summary>
        /// <param name="imgPath">水印图片路径</param>
        /// <param name="filePath">需要添加水印的pdf文件</param>
        /// <param name="outfilePath">添加完成的pdf文件</param>
        /// <returns></returns>
        public static bool SetImgWaterMark(string imgInfoPath, string signImgPath, string filePath, string outfilePath)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            FileStream fileStream = null;
            try
            {
                pdfReader = new PdfReader(filePath);
                fileStream = new FileStream(outfilePath, FileMode.Create);
                pdfStamper = new PdfStamper(pdfReader, fileStream);
                int total = pdfReader.NumberOfPages;
                Rectangle psize = pdfReader.GetPageSize(1);
                PdfContentByte content;  //float width = psize.Width;   //595      //float height = psize.Height;  //841

                #region 添加水印信息
                //遮挡甲方身份证
                Image jia_identity = Image.GetInstance(imgInfoPath);
                jia_identity.SetAbsolutePosition(180, 695);  //水印的位置
                content = pdfStamper.GetOverContent(2);  //要放置的页数
                content.AddImage(jia_identity);

                //遮挡乙方姓名
                Image yi_name = Image.GetInstance(imgInfoPath);
                yi_name.SetAbsolutePosition(210, 655);  //水印的位置
                //yi_name.ScalePercent(4f);  //图片比例
                content = pdfStamper.GetOverContent(2);  //要放置的页数
                content.AddImage(yi_name);

                //遮挡乙方身份证
                Image yi_identity = Image.GetInstance(imgInfoPath);
                yi_identity.SetAbsolutePosition(180, 628);  //水印的位置
                content = pdfStamper.GetOverContent(2);  //要放置的页数
                content.AddImage(yi_identity);

                //遮挡乙方电话
                Image yi_phone = Image.GetInstance(imgInfoPath);
                yi_phone.SetAbsolutePosition(180, 603);  //水印的位置
                content = pdfStamper.GetOverContent(2);  //要放置的页数
                content.AddImage(yi_phone);

                #endregion

                //遮挡乙方签名
                //设置文本域  iTextSharp.text.Rectangle(105, 100, 240, 125) 用来设置文本域的位置，四个参数分别为：llx、lly、urx、ury：
                //llx 为Left ，
                //lly 为Bottom, 
                //urx 为Right，
                //ury 为Top       其中：Width = Right - Left     Heigth = Top - Bototom
                TextField yi_sign = new TextField(pdfStamper.Writer, new iTextSharp.text.Rectangle(460, 430, 540, 475), "date");
                yi_sign.BackgroundColor = BaseColor.WHITE;
                yi_sign.BorderWidth = 1;
                yi_sign.BorderColor = BaseColor.BLACK;
                yi_sign.BorderStyle = 4;
                yi_sign.FontSize = 11f;
                pdfStamper.AddAnnotation(yi_sign.GetTextField(), 9);
                pdfStamper.FormFlattening = true;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                #region 释放资源
                if (pdfStamper != null)
                {
                    pdfStamper.Close();
                }
                if (pdfReader != null)
                {
                    pdfReader.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                #endregion
            }
        }
        #endregion

        #region 替换pdf文件中默认的logo图
        private static void ReplaceImg()
        {
            PdfReader pdf = new PdfReader(@"E:\pdf\156281489401000002.pdf");
            PdfStamper stp = new PdfStamper(pdf, new FileStream(@"E:\pdf\test.pdf", FileMode.Create));
            PdfWriter writer = stp.Writer;
            Image img = Image.GetInstance(@"E:\pdf\sign.png");
            PdfDictionary pg = pdf.GetPageN(1);
            PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
            if (xobj != null)
            {
                foreach (PdfName name in xobj.Keys)
                {
                    PdfObject obj = xobj.Get(name);
                    if (obj.IsIndirect())
                    {
                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                        PdfName type = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
                        if (PdfName.IMAGE.Equals(type))
                        {
                            PdfReader.KillIndirect(obj);
                            Image maskImage = img.ImageMask;
                            if (maskImage != null)
                            {
                                writer.AddDirectImageSimple(maskImage);
                                writer.AddDirectImageSimple(img, (PRIndirectReference)obj);
                                break;
                            }
                        }

                    }
                }
                stp.Close();
            }
        }
        #endregion

        #region 网友提供的更高级的logo图替换
        /// <summary>
        /// 替换PDF中的图片
        /// </summary>
        /// <param name="src">pdf文件流</param>
        /// <param name="distinguishMethod">识别需要被替换图片的方法</param>
        /// <param name="replaceToImg">替换成这个图片</param>
        /// <returns></returns>
        private static MemoryStream ReplaceImage(Stream src, DistinguishImage distinguishMethod, Image replaceToImg)
        {
            PdfReader reader2 = new PdfReader(src);
            PdfMemoryStream outMemoryStream = new PdfMemoryStream();
            outMemoryStream.AllowClose = false;

            using (PdfStamper stamper = new PdfStamper(reader2, outMemoryStream))
            {
                int pageCount2 = reader2.NumberOfPages;
                for (int i = 1; i <= pageCount2; i++)
                {
                    //Get the page
                    var page = reader2.GetPageN(i);
                    PdfObject obj = FindImageInPDFDictionary(page, distinguishMethod);
                    if (obj != null)
                    {
                        PdfReader.KillIndirect(obj);//移除老图片 
                        //Image img = Image.GetInstance(replaceToImg, BaseColor.WHITE, true);
                        Image img = Image.GetInstance(replaceToImg);
                        Image maskImage = img.ImageMask;
                        if (maskImage != null)
                        {
                            stamper.Writer.AddDirectImageSimple(maskImage);
                            stamper.Writer.AddDirectImageSimple(img, (PRIndirectReference)obj);
                        }
                    }
                }
            }

            outMemoryStream.Position = 0;
            return outMemoryStream;
        }

        //在pdf页面中 找到logo图片
        private static PdfObject FindImageInPDFDictionary(PdfDictionary pg, DistinguishImage distinguishMethod)
        {
            PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
            if (xobj != null)
            {
                foreach (PdfName name in xobj.Keys)
                {
                    Console.WriteLine(name.ToString());
                    PdfObject obj = xobj.Get(name);
                    if (obj.IsIndirect())
                    {
                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                        PdfName type = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
                        //image at the root of the pdf
                        if (PdfName.IMAGE.Equals(type))
                        {
                            if (distinguishMethod(tg) == true)
                            {
                                return obj;
                            }
                            else
                            {
                                continue;//继续找
                            }
                        }// image inside a form
                        else if (PdfName.FORM.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg, distinguishMethod);
                        } //image inside a group
                        else if (PdfName.GROUP.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg, distinguishMethod);
                        }
                    }
                }
            }

            return null;

        }

        /// <summary>
        /// 辨别图片的委托
        /// </summary>
        /// <param name="imgObject"></param>
        /// <returns></returns>
        delegate bool DistinguishImage(PdfDictionary imgObject);

        /// <summary>
        /// 辨别图片是不是LOGO
        /// </summary>
        /// <param name="imgObject"></param>
        /// <returns></returns>
        private static bool DistinguishImageIsLogo(PdfDictionary imgObject)
        {
            int width, height, length;
            int.TryParse(imgObject.Get(PdfName.WIDTH).ToString(), out width);
            int.TryParse(imgObject.Get(PdfName.HEIGHT).ToString(), out height);
            int.TryParse(imgObject.Get(PdfName.LENGTH).ToString(), out length);

            //从这3个参数就可以判断是不是logo, 也可以按照name来判断.还可以硬编码判断两个图片对象是否一样.
            if (width == 270 && height == 111 && length == 11878)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class PdfMemoryStream : System.IO.MemoryStream
        {

            public PdfMemoryStream(byte[] bytes) : base(bytes)
            {
                AllowClose = true;
            }

            public PdfMemoryStream()
            {
                AllowClose = true;
            }

            public bool AllowClose { get; set; }

            public override void Close()
            {
                if (AllowClose)
                    base.Close();
            }

        }
        #endregion


        /*
         * 
         * 此dll在.net core环境中报错，需在.net 4.5.2下运行
         * 此外该方法转换pdf到图片时，要求pdf文件中没有透明的图片，否则报错
         * 参考网址：https://www.cnblogs.com/xibei666/p/7012807.html
         * 
         */
        #region  使用O2S将PDF转为图片
        private static void O2SPdfToImg()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            int num = basePath.IndexOf("bin");
            basePath = basePath.Substring(0, num);
            O2SHelper.ConvertPDF2Image(basePath + "file\\123456.pdf", basePath + "file\\", "img_", 1, 1, ImageFormat.Jpeg, Definition.Five);
        }
        #endregion 
    }
}
