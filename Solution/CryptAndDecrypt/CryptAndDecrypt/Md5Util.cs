using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EncryptAndDecrypt
{
    public class Md5Util
    {
        /// <summary>
        /// MD5字符串加密升级版
        /// </summary>
        /// <param name="txt">要加密的字符串</param>
        /// <param name="IV">IV向量</param>
        /// <returns>加密后字符串</returns>
        public static string MD5Encode(string txt, string iv)
        {
            string temp = MD5Base(txt); //一次加密
            temp = MD5Base(temp);  //二次加密
            temp = GetIVString(temp, iv);
            return MD5Base(txt);
        }

        #region 将加密串和IV进行穿插组合
        /// <summary>
        /// 将加密串和IV进行穿插组合
        /// </summary>
        /// <param name="encryptedStr">加密后的串</param>
        /// <param name="iv">iv</param>
        /// <returns>最终的混合加密数据</returns>
        private static string GetIVString(String encryptedStr, String iv)
        {
            StringBuilder buffer = new StringBuilder();
            String result = "";
            int length1 = encryptedStr.Length;
            int length2 = iv.Length;
            for (int i = 0; i < length1 && i < length2; i++)
            {
                buffer.Append(encryptedStr[i]);
                buffer.Append(iv[i]);
            }

            result = buffer.ToString();
            if (length1 < length2)
            {
                result += iv.Substring(length1, length2 - length1);
            }
            else if (length1 > length2)
            {
                result += encryptedStr.Substring(length2, length1 - length2);
            }

            return result;
        }
        #endregion

        /// <summary>
        /// MD5字符串加密基础版
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        private static string MD5Base(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
