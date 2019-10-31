using System;
using System.Drawing.Imaging;

namespace PdfOperate
{
    class Program
    {
        static void Main(string[] args)
        {
            O2SPdfToImg();
        }

        private static void O2SPdfToImg()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            int num = basePath.IndexOf("bin");
            basePath = basePath.Substring(0, num);
            O2SHelper.ConvertPDF2Image(basePath + @"file\demo.pdf", basePath + @"file\", "img_", 1, 2, ImageFormat.Jpeg, Definition.Five);
        }
    }
}
