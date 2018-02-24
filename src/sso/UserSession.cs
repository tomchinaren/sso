using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sso
{
    public class UserSession
    {
        private const int _SignLength = 16;
        private const string _SignKey = "(%@&6F300BE5B54F";
        private static byte[] _AesKey = { 153, 38, 7, 88, 199, 29, 15, 114, 88, 89, 64, 187, 119, 207, 245, 217, 143, 29, 77, 203, 251, 226, 205, 27, 107, 198, 156, 254, 138, 125, 14, 144 };
        private static byte[] _AesIV = { 177, 44, 77, 23, 114, 141, 50, 51, 171, 73, 53, 108, 133, 232, 11, 248 };
        public string Token { get; set; }

        public UserSession()
        {

        }
        public UserSession(string encryptString)
        {
            string sessionString;
            if (!CheckSign(encryptString,out sessionString))
            {
                return;
            }

            var json = AESHelper.Decrypt(sessionString, _AesKey, _AesIV); ;
            UserSession session = JsonConvert.DeserializeObject<UserSession>(json);
            this.Token = session.Token;
        }
        public string CreateSessionString()
        {
            //new token
            this.Token = CreateToken();

            //serialize
            var sessionString = JsonConvert.SerializeObject(this);

            //encrypt
            var encryptSessionString = AESHelper.Encrypt(sessionString, _AesKey, _AesIV);
            var xx = AESHelper.Decrypt(encryptSessionString, _AesKey, _AesIV);

            //sign
            var sign = MD5Helper.PasswordEncryption(encryptSessionString + _SignKey, _SignLength);

            return encryptSessionString + sign;
        }

        private string CreateToken()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var token1 = Convert.ToBase64String(guid1.ToByteArray()).TrimEnd('=').Replace("/", "_").Replace("+", "-");
            var token2 = Convert.ToBase64String(guid2.ToByteArray()).TrimEnd('=').Replace("/", "_").Replace("+", "-");
            return (token1 + token2).Substring(0, 32);
        }

        private bool CheckSign(string signedSessionString,out string sessionString)
        {
            sessionString = signedSessionString.Substring(0, signedSessionString.Length - _SignLength);
            string expectedSign = signedSessionString.Substring(signedSessionString.Length - _SignLength);
            var actualSign = MD5Helper.PasswordEncryption(sessionString + _SignKey, _SignLength);
            return expectedSign == actualSign;
        }




    }
}
