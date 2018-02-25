using ssoClient;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [CheckLogin("http://passport.sso.com/Default/Login", "http://WebApp1.sso.com/")]
    public class BaseController: Controller
    {
    }
}