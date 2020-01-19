using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptAndDecrypt
{
    public class Utf8Util
    {
        #region utf8加密（给评论用的）
        /// <summary>
        /// utf8加密（给评论用的）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Utf8Encode(string content)
        {
            return System.Web.HttpUtility.UrlEncode(content, System.Text.Encoding.UTF8);
        }
        #endregion

        #region utf8解密（给评论用的）
        /// <summary>
        /// utf8解密（给评论用的）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Utf8Decode(string content)
        {
            return System.Web.HttpUtility.UrlDecode(content, System.Text.Encoding.UTF8);
        }
        #endregion
    }
}
