using sso;
using System.Web;

namespace ssoClient
{
    public static class WebAuth
    {
        public const string SessionCookieName = "session";
        private const string _TokenKey = "_Token";

        public static bool IsLogin()
        {
            return !string.IsNullOrWhiteSpace(GetToken());
        }

        public static string GetToken()
        {
            var token = Token;
            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            //get session cookie
            var cookie = HttpContext.Current.Request.Cookies.Get(SessionCookieName);
            if (cookie == null)
            {
                return null;
            }
            string signedSessionString = cookie.Value;
            var userSession = new UserSession(signedSessionString);
            token = userSession?.Token;
            if (string.IsNullOrWhiteSpace(token))
            {
                //兼容form验证方式
                token = HttpContext.Current.User.Identity.Name;
            }
            Token = token;
            return token;
        }

        private static string Token
        {
            get
            {
                if (HttpContext.Current.Items[_TokenKey] == null)
                {
                    return null;
                }
                else
                {
                    return (string)HttpContext.Current.Items[_TokenKey];
                }
            }
            set
            {
                HttpContext.Current.Items[_TokenKey] = value;
            }

        }
    }
}
