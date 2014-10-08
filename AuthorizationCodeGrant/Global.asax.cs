using System.Web;
using System.Web.Routing;

namespace AuthorizationCodeGrant
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}