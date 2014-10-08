using Microsoft.Owin.Security.OAuth;
using Owin;

namespace ResourceServer
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}