using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace NetCore.Framework
{
    public class EncryptDecrypt
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        private static string[] DEFAULT_COOKIE_KEYS = new string[] { "1qaz2wsx", "tgbnjity", "we5fw8jw,gegs", "fwj5963dd", "dfjeijg23,cd", "jgwoj901,yin", "e,wjjfe17", "18ddfe64s", "fwof7wefw", "dfervll,5" };

        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        #region Encrypt3DES

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="encryptKey">密钥</param>
        /// <param name="encryptString">加密字符串</param>
        /// <returns>string</returns>
        public static string Encrypt3DES(string encryptString, string encryptKey)
        {
            if (string.IsNullOrEmpty(encryptString))
            {
                throw new Exception("加密字符串不能为空");
            }
            if (string.IsNullOrEmpty(encryptKey) || encryptKey.Length<8)
            {
                throw new Exception("密钥必须大于等于8位");
            }
            try
            {
                byte[] byKey = null;
                byKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                DESCryptoServiceProvider dcsprovider = new DESCryptoServiceProvider()
                {
                    Mode = CipherMode.ECB
                };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, dcsprovider.CreateEncryptor(byKey, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Encrypt3DES

        #region Decrypt3DES

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="decryptKey">密钥</param>
        /// <param name="decryptString">解密字符串</param>
        /// <returns>string</returns>
        public static string Decrypt3DES(string decryptString, string decryptKey)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(decryptKey) || decryptKey.Length < 8)
            {
                throw new Exception("密钥必须大于等于8位");
            }
            if (string.IsNullOrWhiteSpace(decryptString))
            {
                throw new Exception("解密字符串不能为空或者空字符");
            }
            try
            {
                byte[] byKey = null;
                byte[] inputByteArray = new Byte[decryptString.Length];
                byKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider()
                {
                    Mode = CipherMode.ECB
                };
                inputByteArray = Convert.FromBase64String(decryptString);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Decrypt3DES

        #region EncryptMD5

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="md5String">加密字符串</param>
        /// <returns>string</returns>
        public static string EncryptMD5(string md5String)
        {
            if (string.IsNullOrWhiteSpace(md5String))
            {
                throw new Exception("空值与NULL不能为空");
            }
            return string.Join(null, MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(md5String)).Select(x => x.ToString("x2")));
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] fromData = Encoding.Unicode.GetBytes(md5String);
            //byte[] bufferData = md5.ComputeHash(fromData);
            //StringBuilder strbud = new StringBuilder();
            //for (int i = 0; i < bufferData.Length; i++)
            //{
            //    strbud.Append(bufferData[i].ToString("x"));
            //}
            //return strbud.ToString();
        }

        #endregion EncryptMD5

        #region EncryptMD5+Salt

        /// <summary>
        /// EncryptMD5+Salt
        /// </summary>
        /// <param name="md5string"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptMD5Salt(string md5string, object salt)
        {
            return EncryptMD5(EncryptSaltFormat(md5string, salt));
        }

        #endregion EncryptMD5+Salt

        #region Encrypt

        /// <summary>
        /// 字符混淆
        /// </summary>
        /// <param name="cookieKey">cookie密钥</param>
        /// <param name="cookieVal">cookie加密字符串</param>
        /// <returns>byte[]</returns>
        private static byte[] Encrypt(string cookieKey, string cookieVal)
        {
            if (string.IsNullOrWhiteSpace(cookieKey))
            {
                throw new ArgumentNullException("cookieKey");
            }
            if (string.IsNullOrWhiteSpace(cookieVal))
            {
                throw new ArgumentNullException("cookieVal");
            }
            List<byte> source = new List<byte>();
            try
            {
                byte[] byteVals = Encoding.Unicode.GetBytes(cookieVal);
                byte[] byteKeys = Encoding.UTF8.GetBytes(cookieKey);
                int index = 0;
                for (int i = 0; i < byteVals.Length; i++)
                {
                    byte item = (byte)(byteVals[i] ^ byteKeys[index]);
                    source.Add(item);
                    index++;
                    if (index >= byteKeys.Length)
                    {
                        index = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return source.ToArray<byte>();
        }

        #endregion Encrypt

        #region CookieEncode

        /// <summary>
        /// 格式化特殊字符
        /// </summary>
        /// <param name="cookieValue">cookieValue</param>
        /// <returns>string </returns>
        public static string CookieEncode(string cookieValue)
        {
            string encode = string.Empty;
            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                encode = cookieValue.Trim();
                encode.Replace("%", "%25");
                encode.Replace("=", "%3d");
                encode.Replace(",", "%2c");
                encode.Replace(";", "%3b");
            }
            return encode;
        }

        #endregion CookieEncode

        #region CookieDecode

        /// <summary>
        /// 还原特殊字符
        /// </summary>
        /// <param name="cookieValue">cookieValue</param>
        /// <returns>string</returns>
        public static string CookieDecode(string cookieValue)
        {
            string str = cookieValue.Trim();
            if (!string.IsNullOrEmpty(str))
            {
                str.Replace("%25", "%");
                str.Replace("%3d", "=");
                str.Replace("%2c", ",");
                str.Replace("%3b", ";");
            }
            return str;
        }

        #endregion CookieDecode

        #region EncryptCookie

        /// <summary>
        /// Cookie加密
        /// </summary>
        /// <param name="cookieString">cookie加密字符</param>
        /// <returns>string</returns>
        public static string EncryptCookie(string cookieString)
        {
            if (string.IsNullOrWhiteSpace(cookieString))
            {
                throw new ArgumentNullException("cookieString");
            }
            StringBuilder buffer = new StringBuilder();
            try
            {
                int index = new Random().Next(DEFAULT_COOKIE_KEYS.Length);
                byte[] encryptByte = Encrypt(DEFAULT_COOKIE_KEYS[index], cookieString);

                for (int i = 0; i < encryptByte.Length; i++)
                {
                    buffer.Append(Convert.ToByte(encryptByte[i]).ToString("X2"));
                }
                buffer.Append(index);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer.ToString();
        }

        #endregion EncryptCookie

        #region DecryptCookie

        /// <summary>
        /// cookie解密
        /// </summary>
        /// <param name="cookieString">解密字符串</param>
        /// <returns>string</returns>
        public static string DecryptCookie(string cookieValue)
        {
            if (string.IsNullOrWhiteSpace(cookieValue))
            {
                throw new ArgumentNullException("cookieString");
            }
            byte[] buffer2 = null;
            try
            {
                int index = Convert.ToInt32(cookieValue.Substring(cookieValue.Length - 1));
                byte[] buffer = new byte[(cookieValue.Length - 1) / 2];
                int indexNum = 0;
                for (int i = 0; i < (cookieValue.Length - 1); i += 2)
                {
                    buffer[indexNum++] = Convert.ToByte(Convert.ToInt64(cookieValue.Substring(i, 2), 0x10));
                }
                string result = Encoding.Unicode.GetString(buffer);
                buffer2 = Encrypt(DEFAULT_COOKIE_KEYS[index], result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Encoding.Unicode.GetString(buffer2);
        }

        #endregion DecryptCookie

        #region Base64Encode

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        #endregion Base64Encode

        #region Base64Decode

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        #endregion Base64Decode

        #region SHA256+Salt

        /// <summary>
        /// SHA256+Salt 加盐加密
        /// </summary>
        /// <param name="shaEnString"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Sha256EncryptSalt(string shaEnString, object salt)
        {
            // random salt
            // you can also use RNGCryptoServiceProvider class
            //System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            //byte[] saltBytes = new byte[36];
            //rng.GetBytes(saltBytes);
            //string salt = Convert.ToBase64String(saltBytes);
            //string salt = ToHexString(saltBytes);

            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(EncryptSaltFormat(shaEnString, salt));
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        public static string ToHexString(byte[] bytes)
        {
            var hex = new StringBuilder();
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        #endregion SHA256+Salt

        #region EncryptSaltFormat

        /// <summary>
        /// 字符串加盐规则
        /// </summary>
        /// <param name="str"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string EncryptSaltFormat(string str, object salt)
        {
            if (string.IsNullOrWhiteSpace(salt?.ToString()))
            {
                throw new Exception("salt is Invalid");
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                throw new Exception("strparam is Invalid");
            }
            return $"^{str}|{salt?.ToString().Trim()}^";
        }

        #endregion EncryptSaltFormat
    }
}