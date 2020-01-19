using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EncryptAndDecrypt
{
    public class CryptLib
    {
        UTF8Encoding _enc;
        RijndaelManaged _rcipher;
        byte[] _key, _pwd, _ivBytes, _iv;

        #region 加密模式枚举
        /***
		 * Encryption mode enumeration
		 */
        private enum EncryptMode
        {
            /// <summary>
            /// 加密
            /// </summary>
            ENCRYPT,
            /// <summary>
            /// 解密
            /// </summary>
            DECRYPT
        };
        #endregion

        #region IV向量的随机获取字符组成 【CharacterMatrixForRandomIVStringGeneration】
        /// <summary>
        /// IV向量的随机获取字符组成
        /// </summary>
        static readonly char[] CharacterMatrixForRandomIVStringGeneration = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };
        #endregion

        #region 根据输入的length长度，获取随机的IV字符串【GenerateRandomIV】
        /**
		 * This function generates random string of the given input length.
		 * 根据输入的length长度，获取随机的IV字符串
		 * @param _plainText
		 *            Plain text to be encrypted
		 * @param _key
		 *            Encryption Key. You'll have to use the same key for decryption
		 * @return returns encrypted (cipher) text
		 */

        /// <summary>
        ///根据输入的length长度，获取随机的IV字符串
        /// </summary>
        /// <param name="length">想要获取字符串的长度</param>
        /// <returns>获取到随机的特定长度字符串</returns>

        public static string GenerateRandomIV(int length)
        {
            char[] _iv = new char[length];
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes); //Fills an array of bytes with a cryptographically strong sequence of random values. 
            }

            for (int i = 0; i < _iv.Length; i++)
            {
                int ptr = randomBytes[i] % CharacterMatrixForRandomIVStringGeneration.Length;
                _iv[i] = CharacterMatrixForRandomIVStringGeneration[ptr];
            }

            return new string(_iv);
        }
        #endregion

        #region 构造函数
        public CryptLib()
        {
            _enc = new UTF8Encoding();
            _rcipher = new RijndaelManaged();
            _rcipher.Mode = CipherMode.CBC;
            _rcipher.Padding = PaddingMode.PKCS7;
            _rcipher.KeySize = 256;
            _rcipher.BlockSize = 128;
            _key = new byte[32];
            _iv = new byte[_rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
            _ivBytes = new byte[16];
        }
        #endregion

        #region 根据秘钥，加密解密模式，初始化向量 获取加密后或解密后的字符串 【encryptDecrypt-私有方法】
        /**
		 * 
		 * @param _inputText
		 *            Text to be encrypted or decrypted
		 * @param _encryptionKey
		 *            Encryption key to used for encryption / decryption
		 * @param _mode
		 *            specify the mode encryption / decryption
		 * @param _initVector
		 * 			  initialization vector
		 * @return encrypted or decrypted string based on the mode
	 	*/
        /// <summary>
        ///根据秘钥，加密解密模式，初始化向量 获取加密后或解密后的字符串
        /// </summary>
        /// <param name="_encryptionKey">用于加密/解密的加密密钥</param>
        /// <param name="_initVector">初始化向量</param>
        /// <param name="_inputText">原串或加密串</param>
        /// <param name="_mode">ENCRYPT 或DECRYPT </param>
        /// <returns>获取到随机的特定长度字符串</returns>
        private String encryptDecrypt(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
        {

            string _out = "";// output string
            //_encryptionKey = MD5Hash (_encryptionKey);
            _pwd = Encoding.UTF8.GetBytes(_encryptionKey);
            _ivBytes = Encoding.UTF8.GetBytes(_initVector);

            int len = _pwd.Length;
            if (len > _key.Length)
            {
                len = _key.Length;
            }
            int ivLenth = _ivBytes.Length;
            if (ivLenth > _iv.Length)
            {
                ivLenth = _iv.Length;
            }

            Array.Copy(_pwd, _key, len);
            Array.Copy(_ivBytes, _iv, ivLenth);
            _rcipher.Key = _key;
            _rcipher.IV = _iv;

            if (_mode.Equals(EncryptMode.ENCRYPT))
            {
                //encrypt
                byte[] tempByte = _enc.GetBytes(_inputText);
                byte[] plainText = _rcipher.CreateEncryptor().TransformFinalBlock(tempByte, 0, tempByte.Length);
                _out = Convert.ToBase64String(plainText);
            }
            if (_mode.Equals(EncryptMode.DECRYPT))
            {
                //decrypt
                byte[] plainText = _rcipher.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(_inputText), 0, Convert.FromBase64String(_inputText).Length);
                _out = _enc.GetString(plainText);
            }
            _rcipher.Dispose();
            return _out;// return encrypted/decrypted string
        }
        #endregion

        #region  根据秘钥，初始化向量，原字符串 获取加密后的字符串【encrypt】
        /**
		 * This function encrypts the plain text to cipher text using the key
		 * provided. You'll have to use the same key for decryption
		 * 
		 * @param _plainText
		 *            Plain text to be encrypted
		 * @param _key
		 *            Encryption Key. You'll have to use the same key for decryption
		 * @return returns encrypted (cipher) text
		 */
        /// <summary>
        ///根据秘钥，初始化向量，原字符串 获取加密后的字符串
        /// </summary>
        /// <param name="_encryptionKey">用于加密密钥</param>
        /// <param name="_initVector">初始化向量</param>
        /// <param name="_inputText">原串</param>
        /// <returns>获取到随机的特定长度字符串</returns>
        public string encrypt(string _plainText, string _key, string _initVector)
        {
            return encryptDecrypt(_plainText, _key, EncryptMode.ENCRYPT, _initVector);
        }
        #endregion

        #region 解密字符串
        /***
		 * This funtion decrypts the encrypted text to plain text using the key
		 * provided. You'll have to use the same key which you used during
		 * encryprtion
		 * 
		 * @param _encryptedText
		 *            Encrypted/Cipher text to be decrypted
		 * @param _key
		 *            Encryption key which you used during encryption
		 * @return encrypted value
		 */
        /// <summary>
        ///根据秘钥，初始化向量，原字符串 获取加密后的字符串    【decrypt】
        /// </summary>
        /// <param name="_encryptedText">加密后的秘钥</param>
        /// <param name="_initVector">初始化向量</param>
        /// <param name="_key">加密用的key</param>
        /// <returns>获取到随机的特定长度字符串</returns>
        public string decrypt(string _encryptedText, string _key, string _initVector)
        {
            return encryptDecrypt(_encryptedText, _key, EncryptMode.DECRYPT, _initVector);
        }

        /// <summary>
        ///根据秘钥，初始化向量，原字符串 获取加密后的字符串 【decrypt】
        /// </summary>
        /// <param name="_encryptedText">加密后的秘钥</param>
        /// <param name="_key">加密用的key</param>
        /// <param name="length">生成随机IV的长度值</param>
        /// <returns>获取到随机的特定长度字符串</returns>
        public string decrypt(string _encryptedText, string _key, int length)
        {
            string _initVector = string.Empty;
            //根据加密后的串及IV向量的随机生成长度，分离出加密向量
            _encryptedText = GetAndRemoveIV(_encryptedText, 16, out _initVector);
            return encryptDecrypt(_encryptedText, _key, EncryptMode.DECRYPT, _initVector);
        }
        #endregion 

        #region 根据length，获取指定长度的hash串 【getHashSha256】
        /***
		 * This function decrypts the encrypted text to plain text using the key
		 * provided. You'll have to use the same key which you used during
		 * encryption
		 * 
		 * @param _encryptedText
		 *            Encrypted/Cipher text to be decrypted
		 * @param _key
		 *            Encryption key which you used during encryption
		 */
        /// <summary>
        ///根据length，获取指定长度的hash串
        /// </summary>
        /// <param name="text">要hash的字符串</param>
        /// <param name="length">长度</param>
        /// <returns>得到hash后字符串</returns>
        public static string getHashSha256(string text, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x); //covert to hex string
            }
            if (length > hashString.Length)
                return hashString;
            else
                return hashString.Substring(0, length);
        }
        #endregion

        #region MD5 hash  已不常用
        /// <summary>
        /// MD5 hash  已不常用 【MD5Hash-私有方法】
        /// </summary>
        /// <param name="text">要hash的字符串</param>
        /// <returns>返回hash后的串</returns>
        private static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            Console.WriteLine("md5 hash of they key=" + strBuilder.ToString());
            return strBuilder.ToString();
        }
        #endregion 

        #region 将AppID和IV拆分开来
        /// <summary>
        /// 将加密后的串和IV拆分开来
        /// </summary>
        /// <param name="encryptedStr">最终加密的串</param>
        /// <param name="length">长度</param>
        /// <param name="iv">拆出来的iv</param>
        /// <returns>拆出来的AppID</returns>
        private string GetAndRemoveIV(string encryptedStr, int length, out string iv)
        {
            StringBuilder ivBuffer = new StringBuilder();
            StringBuilder encryptedBuffer = new StringBuilder();
            String result = "";

            for (int i = 1; i < length * 2; i = i + 2)
            {
                ivBuffer.Append(encryptedStr[i]);
                encryptedBuffer.Append(encryptedStr[i - 1]);
            }

            iv = ivBuffer.ToString();
            result = encryptedBuffer.ToString() + encryptedStr.Substring(length * 2);
            return result;
        }
        #endregion

        #region 将AppID和IV进行穿插组合
        /// <summary>
        /// 将AppID和IV进行穿插组合
        /// </summary>
        /// <param name="encryptedStr">加密后的串</param>
        /// <param name="iv">iv</param>
        /// <returns>最终的混合加密数据</returns>
        public string appendIVString(String encryptedStr, String iv)
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
    }
}
