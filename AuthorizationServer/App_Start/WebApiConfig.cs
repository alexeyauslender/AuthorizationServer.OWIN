using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace AuthorizationServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );
        }
    }
}