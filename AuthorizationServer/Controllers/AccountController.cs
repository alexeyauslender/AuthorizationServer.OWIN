using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace AuthorizationServer.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
            if (Request.HttpMethod == "POST")
            {
                bool isPersistent = !string.IsNullOrEmpty(Request.Form.Get("isPersistent"));

                if (!string.IsNullOrEmpty(Request.Form.Get("submit.Signin")))
                {
                    authentication.SignIn(
                        new AuthenticationProperties {IsPersistent = isPersistent},
                        new ClaimsIdentity(
                            new[] {new Claim(ClaimsIdentity.DefaultNameClaimType, Request.Form["username"])},
                            "Application"));
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult External()
        {
            IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
            if (Request.HttpMethod == "POST")
            {
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.StartsWith("submit.External.") && !string.IsNullOrEmpty(Request.Form.Get(key)))
                    {
                        string authType = key.Substring("submit.External.".Length);
                        authentication.Challenge(authType);
                        return new HttpUnauthorizedResult();
                    }
                }
            }
            ClaimsIdentity identity = authentication.AuthenticateAsync("External").Result.Identity;
            if (identity != null)
            {
                authentication.SignOut("External");
                authentication.SignIn(
                    new AuthenticationProperties {IsPersistent = true},
                    new ClaimsIdentity(identity.Claims, "Application", identity.NameClaimType, identity.RoleClaimType));
                return Redirect(Request.QueryString["ReturnUrl"]);
            }

            return View();
        }
    }
}