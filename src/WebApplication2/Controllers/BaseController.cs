using ssoClient;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    [CheckLogin("http://passport.sso.com/Default/Login", "http://WebApp2.sso.com/")]
    public class BaseController: Controller
    {
    }
}