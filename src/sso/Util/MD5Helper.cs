using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sso
{
    public static class MD5Helper
    {
        /// <summary>
        /// md5加密（8, 16）
        /// </summary>
        /// <param name="s">需要加密的字符串</param>
        /// <returns></returns>
        public static string PasswordEncryption(string s, int passwordLength)
        {
            using (System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] data = System.Text.Encoding.Default.GetBytes(s);//将字符编码为一个字节序列 
                byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
                md5.Clear();
                string str = "";
                for (int i = 0; i < md5data.Length; i++)
                {
                    str += md5data[i].ToString("x").PadLeft(2, '0');
                }

                return str.Substring(8, passwordLength).ToUpper(); ;
            }
        }
    }
}
