using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EncryptAndDecrypt
{
    /// <summary>
    /// AES加密解密
    /// </summary>
    public class AesUtil
    {

        #region 加密字符串（aesKey）
        /// <summary>
        /// 加密字符串（aesKey）
        /// </summary>
        /// <param name="encodeInfo">要加密的字符串</param>
        /// <returns>加密后的字符串（含IV向量）</returns>
        public static string GetEnCodeInfo(string encodeInfo)
        {
            CryptLib _crypt = new CryptLib();
            string result = "";
            //aesKey密钥
            string aesKey = System.Configuration.ConfigurationManager.AppSettings["aesKey"].ToString();
            //拿到hash后的aseKey
            aesKey = CryptLib.getHashSha256(aesKey, 31); //32 bytes = 256 bits
            //获取一个16位的随机向量
            string iv = CryptLib.GenerateRandomIV(16);
            //加密字符串（AppID,hashaesKey,iv）
            result = _crypt.encrypt(encodeInfo, aesKey, iv);

            //将加密后的字符串与加密前的IV向量穿插混合后得到最后的串
            return _crypt.appendIVString(result, iv);  //将加密字符串和iv混合处理后再发送
        }

        #endregion

        #region 解密字符串（aesKey）
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encodeInfo">最终加密后的数据（含加密串和IV串的混合串）</param>
        /// <returns>解密后的数据</returns>
        public static string GetDeCodeInfo(string decodeInfo)
        {
            CryptLib _crypt = new CryptLib();
            //aesKey密钥
            string aesKey = System.Configuration.ConfigurationManager.AppSettings["aesKey"].ToString();
            if (!string.IsNullOrWhiteSpace(aesKey))
            {
                //得到hash后的aseKey
                aesKey = CryptLib.getHashSha256(aesKey, 31); //32 bytes = 256 bits
            }
            //返回解密后的原串值
            return _crypt.decrypt(decodeInfo, aesKey, 16);
        }
        #endregion

        #region 签名生成算法
        const string ParameterSeparator = "&";
        const string NameValueSeparator = "=";

        #region 签名的算法
        /// <summary>
        /// 签名的算法
        /// </summary>
        /// <param name="secretKey">secretKey</param>
        /// <param name="parameters">AppID和UTC时间两个参数组成的参数组</param>
        /// <returns></returns>
        public static string GetHashString(string secretKey, NameValuePair[] parameters)
        {
            var data = BuildDataString(parameters);
            return ComputeHash(secretKey, data);
        }
        #endregion

        private static string ComputeHash(string secretKey, string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            return ComputeHash(keyBytes, dataBytes);
        }

        private static string ComputeHash(byte[] secretKey, byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (var hmac = new HMACSHA256(secretKey))
            {
                var hash = hmac.ComputeHash(data, 0, data.Length);
                return ConvertBytesToHexString(hash);
            }
        }


        private static string BuildDataString(NameValuePair[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var builder = new StringBuilder();

            var orderedParameters = parameters
                .Where(p => string.IsNullOrEmpty(p.Name) && !string.IsNullOrEmpty(p.Value))
                .Union(parameters
                    .Where(p => !string.IsNullOrEmpty(p.Name) && !string.IsNullOrEmpty(p.Value))
                    .OrderBy(i => i.Name)
                )
                .ToList();

            foreach (var parameter in orderedParameters)
            {
                if (builder.Length > 0)
                    builder.Append(ParameterSeparator);

                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    builder.Append(parameter.Name);
                    builder.Append(NameValueSeparator);
                }

                builder.Append(parameter.Value);
            }

            var data = builder.ToString();
            return data;
        }

        private static string ConvertBytesToHexString(byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var b in bytes)
                builder.Append(b.ToString("x2", CultureInfo.InvariantCulture));

            return builder.ToString();
        }

        private static byte[] ConvertHexStringToBytes(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                .Where(i => i % 2 == 0)
                .Select(i => Convert.ToByte(hexString.Substring(i, 2), 16))
                .ToArray();
        }

        public class NameValuePair
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public NameValuePair(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append("[");
                builder.Append(Name);
                builder.Append(", ");
                builder.Append(Value);
                builder.Append("]");
                return builder.ToString();
            }
        }
        #endregion
    }
}
