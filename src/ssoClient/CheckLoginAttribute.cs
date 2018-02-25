using sso;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ssoClient
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        private string _LoginUrl { get; set; }
        private string _CallbackUrl { get; set; }
        private string _CurrentDomain { get; set; }

        public CheckLoginAttribute(string loginUrl, string currentDomain, string callbackUrl = null)
        {
            _LoginUrl = loginUrl;
            _CurrentDomain = currentDomain;
            _CallbackUrl = callbackUrl;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isLogin = WebAuth.IsLogin();
            if (!isLogin)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult() { Data = new { Code = -1, Message = "请先登录" } };
                }
                else
                {
                    GotoLogin(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }


        private void GotoLogin(ActionExecutingContext filterContext)
        {
            string callbackUrl = null;
            //回调地址
            if (string.IsNullOrWhiteSpace(_CallbackUrl))
            {
                callbackUrl = _CurrentDomain + filterContext.HttpContext.Request.Url.PathAndQuery.Substring(1);
            }
            else
            {
                callbackUrl = _CallbackUrl;
            }

            var addition = "";
            var index = _LoginUrl.IndexOf("?");
            if (index < 0)//无
            {
                addition = "?";
            }
            else if (index == _LoginUrl.Length - 1)//?结尾
            {
                addition = "";
            }
            else//?非结尾
            {
                addition = "&";
            }

            var redirectUrl = _LoginUrl + addition + "callbackUrl=" + HttpUtility.UrlEncode(callbackUrl);
            filterContext.Result = new RedirectResult(redirectUrl);
        }
    }
}
