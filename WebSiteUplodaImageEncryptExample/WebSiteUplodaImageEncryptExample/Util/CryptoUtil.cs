using System.Security.Cryptography;
using System.Text;

namespace WebSiteUplodaImageEncryptExample.Util
{
    /// <summary>
    /// 加解密工具 https://lab.sp88.com.tw/genpass/
    /// </summary>
    public static class CryptoUtil
    {
        /// <summary>
        /// 1. 自定義固定金鑰
        /// </summary>
        private const string _AesDefaultKey = "24guDYHrUmj6ll4cIZXmBA8DTY2b8fzN";                                              
       
        /// <summary>
        /// 2. AES 加密
        /// </summary>        
        public static byte[] AesEncrypt(byte[] data)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_AesDefaultKey);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        /// <summary>
        /// 3. AES 解密
        /// </summary>        
        public static byte[] AesDecrypt(byte[] encryptedData)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_AesDefaultKey);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream decryptedMs = new MemoryStream())
                        {
                            csDecrypt.CopyTo(decryptedMs);
                            return decryptedMs.ToArray();
                        }
                    }
                }
            }
        }
    }
}
