using Microsoft.Owin;
using Owin;
using ResourceServer;

[assembly: OwinStartup(typeof (Startup))]

namespace ResourceServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}