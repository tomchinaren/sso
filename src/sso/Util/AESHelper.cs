using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sso
{
    public static class AESHelper
    {
        public static string Encrypt(string data, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, iv);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(data);
                        }
                        return ToHexString(msEncrypt.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string encryptString, byte[] key, byte[] iv)
        {
            string data = null;
            using (Aes aesAlg = Aes.Create())
            {
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(FromHexString(encryptString)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            data = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return data;
        }

        private static string ToHexString(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                if (b > 0xF)
                {
                    sb.Append(b.ToString("X"));
                }
                else
                {
                    sb.AppendFormat("0{0:X}", b);
                }
            }
            return sb.ToString();
        }

        private static byte[] FromHexString(string hexStr)
        {
            byte[] bs = new byte[hexStr.Length / 2];
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                bs[i / 2] = byte.Parse(hexStr.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bs;
        }
    }
}
