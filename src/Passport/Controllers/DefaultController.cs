using sso;
using ssoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Passport.Controllers
{
    public class DefaultController : Controller
    {
        private static string AuthDomain = ".sso.com";
        private static string DefaultIndex = "http://webapp1.com/Default/Index2";
        private static List<User> _UserList = new List<Controllers.User>();
        public DefaultController()
        {
            _UserList.Add(new User() { ID = 1, UserName = "tom", Password = "123456" });
            _UserList.Add(new User() { ID = 2, UserName = "jerry", Password = "9527" });
        }
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string callbackUrl)
        {
            if (string.IsNullOrEmpty(callbackUrl))
            {
                callbackUrl = DefaultIndex;
            }

            ViewBag.callbackUrl = callbackUrl;
            return View();
        }

        public JsonResult PostLogin(string userName, string password)
        {
            var success = false;

            success = _UserList.Exists(t => t.UserName == userName && t.Password == password);

            if (success)
            {
                var sesssion = new UserSession();
                var sessionString = sesssion.CreateSessionString(sesssion.CreateToken());

                WriteCookie(sessionString, false, 0);
            }

            return Json(new { success = success, err = "1" });
        }

        public ActionResult LoginOut(string callbackUrl)
        {
            string token = System.Web.HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(token))
            {
                token = WebAuth.GetToken();
            }
            if (!string.IsNullOrEmpty(token))
            {
                //clear token
            }
            RemoveSessionInCookie();
            FormsAuthentication.SignOut();

            if (string.IsNullOrEmpty(callbackUrl))
            {
                return Redirect(nameof(Login));
            }
            else
            {
                return RedirectToAction(nameof(Login), new { callbackUrl = callbackUrl });
            }
        }

        private void WriteCookie(string signedSessionString, bool isPersistent, uint persistentDays)
        {
            var now = DateTime.Now;
            DateTime expire;
            if (isPersistent)
            {
                expire = now.AddDays(persistentDays);
            }
            else
                expire = DateTime.MinValue;

            var cookie = Response.Cookies[WebAuth.SessionCookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(WebAuth.SessionCookieName);
            }

            cookie.Value = signedSessionString;
            cookie.Expires = expire;
            cookie.Domain = AuthDomain;
            cookie.HttpOnly = true;
            Response.Cookies.Set(cookie);
        }

        private void RemoveSessionInCookie()
        {
            var cookie = Response.Cookies[WebAuth.SessionCookieName];
            if (cookie == null)
            {
                return;
            }

            cookie.Expires = DateTime.Now.AddDays(-100);
            cookie.Domain = AuthDomain;
            Response.Cookies.Set(cookie);
        }
    }

    public class User
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}