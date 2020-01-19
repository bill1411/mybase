using System;

namespace EncryptAndDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            //AesEncrypt();
            //Des();
            //Utf8();
            Md5();
        }

        #region Aes加密解密及签名串处理
        private static void AesEncrypt()
        {
            //相当于appID
            string str = "12345678";
            Console.WriteLine(string.Format("要加密的字符串是：{0}", str));
            //加密后的appID（含穿插的IV）
            string encryptStr = AesUtil.GetEnCodeInfo(str);
            Console.WriteLine(string.Format("加密后的字符串是：{0}", encryptStr));
            Console.WriteLine(string.Format("解密后的字符串是：{0}", AesUtil.GetDeCodeInfo(encryptStr)));

            //时间戳
            string dtStr = DateTimeUtil.GetUTCString();
            Console.WriteLine(string.Format("当前UTC时间是：{0}", dtStr));

            //签名串（appId明文和时间戳明文组成一个字符串数组，在用AppSecret）
            string appSecret = "87654321";
            Console.WriteLine(string.Format("规定的secretKey：{0}", appSecret));
            var parameters = new[]
            {
                new AesUtil.NameValuePair(null, str), //客户端ID
                new AesUtil.NameValuePair(null, dtStr)  //原始的UTC时间
            };
            //加密后的签名串
            string signStr = AesUtil.GetHashString(appSecret, parameters);
            Console.WriteLine(string.Format("加密后的sign：{0}", signStr));

            string endStr = encryptStr + dtStr + signStr;
            Console.WriteLine(string.Format("最终传送的加密串：{0}", endStr));

            Console.ReadKey();

        }
        #endregion

        #region Des加密解密
        private static void Des()
        {
            string str = "123456789";
            Console.WriteLine(string.Format("要加密的字符串是：{0}", str));
            //加密后的appID（含穿插的IV）
            string encryptStr = DesUtil.Encode(str);
            Console.WriteLine(string.Format("加密后的字符串是：{0}", encryptStr));
            Console.WriteLine(string.Format("解密后的字符串是：{0}", DesUtil.Decode(encryptStr)));

            Console.ReadKey();
        }
        #endregion

        #region MD5加密
        private static void Md5()
        {
            string str = "123456789";
            Console.WriteLine(string.Format("要加密的字符串是：{0}", str));
            //加密后的appID（含穿插的IV）
            string encryptStr = Md5Util.MD5Encode(str, "helloworld");
            Console.WriteLine(string.Format("加密后的字符串是：{0}", encryptStr));

            Console.ReadKey();
        }
        #endregion

        #region Utf8加密解密
        private static void Utf8()
        {
            string str = "你们真的好帅啊";
            Console.WriteLine(string.Format("要加密的字符串是：{0}", str));
            //加密后的appID（含穿插的IV）
            string encryptStr = Utf8Util.Utf8Encode(str);
            Console.WriteLine(string.Format("加密后的字符串是：{0}", encryptStr));
            Console.WriteLine(string.Format("解密后的字符串是：{0}", Utf8Util.Utf8Decode(encryptStr)));

            Console.ReadKey();
        }
        #endregion
    }
}
