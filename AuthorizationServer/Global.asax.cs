using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AuthorizationServer
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    (s, cert, chain, sslPolicyErrors) => true;
        }
    }
}