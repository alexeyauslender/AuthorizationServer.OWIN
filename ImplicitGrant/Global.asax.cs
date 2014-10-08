using System.Web;
using System.Web.Routing;

namespace ImplicitGrant
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}