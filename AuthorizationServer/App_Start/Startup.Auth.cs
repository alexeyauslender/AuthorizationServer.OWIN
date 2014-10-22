using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Authorization.Models;
using AuthorizationServer.Models;
using AuthorizationServer.Providers;
using Constants;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace AuthorizationServer
{
    public partial class Startup
    {

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);


            // Enable Application Sign In Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Application",
                AuthenticationMode = AuthenticationMode.Passive,
                LoginPath = new PathString(Paths.LoginPath),
                LogoutPath = new PathString(Paths.LogoutPath),
            });

            // Enable External Sign In Cookie
            app.SetDefaultSignInAsAuthenticationType("External");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "External",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = CookieAuthenticationDefaults.CookiePrefix + "External",
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
            });

            // Enable google authentication
            app.UseGoogleAuthentication();


            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString(Paths.AuthorizePath),
                TokenEndpointPath = new PathString(Paths.TokenPath),
                ApplicationCanDisplayErrors = true,
#if DEBUG
                AllowInsecureHttp = true,
#endif
                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new ApplicationOAuthProvider(),

                // Authorization code provider which creates and receives authorization code
                AuthorizationCodeProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateAuthenticationCode,
                    OnReceive = ReceiveAuthenticationCode,
                },

                // Refresh token provider which creates and receives referesh token
                RefreshTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateRefreshToken,
                    OnReceive = ReceiveRefreshToken,
                }
            };

            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(OAuthOptions);


            app.UseOAuthBearerTokens(OAuthOptions);
        }

       

        private void CreateAuthenticationCode(AuthenticationTokenCreateContext context)
        {
            using (var db = new AuthenticationModelContext())
            {
                context.SetToken(Guid.NewGuid().ToString());

                db.AuthenticationTicketModels.Add(new AuthenticationTicketModel
                {
                    ContextToken = new Guid(context.Token),
                    AuthenticationTicket = context.SerializeTicket()
                });
                db.SaveChanges();
            }
        }

        private void ReceiveAuthenticationCode(AuthenticationTokenReceiveContext context)
        {
            using (var db = new AuthenticationModelContext())
            {
                AuthenticationTicketModel ticketModel =
                    db.AuthenticationTicketModels.Find(new Guid(context.Token));
                context.DeserializeTicket(ticketModel.AuthenticationTicket);
            }
        }

        private void CreateRefreshToken(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());
        }

        private void ReceiveRefreshToken(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}