using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Authorization.Models;
using AuthorizationServer.Models;
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
        public void ConfigureAuth(IAppBuilder app)
        {
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

            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString(Paths.AuthorizePath),
                TokenEndpointPath = new PathString(Paths.TokenPath),
                ApplicationCanDisplayErrors = true,
#if DEBUG
                AllowInsecureHttp = true,
#endif
                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientRedirectUri = ValidateClientRedirectUri,
                    OnValidateClientAuthentication = ValidateClientAuthentication,
                    OnGrantResourceOwnerCredentials = GrantResourceOwnerCredentials,
                    OnGrantClientCredentials = GrantClientCredetails
                },

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
            });
        }

        private Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            using (var db = new AuthenticationModelContext())
            {
                int clientId;
                if (Int32.TryParse(context.ClientId, out clientId))
                {
                    ConsumerModel consumerModel = db.ConsumerModels.Find(clientId);
                    if (clientId == consumerModel.ConsumerId && consumerModel.RedirectUrl == context.RedirectUri)
                    {
                        context.Validated();
                    }
                }
            }
            return Task.FromResult(0);
        }

        private Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientIdStr;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientIdStr, out clientSecret) ||
                context.TryGetFormCredentials(out clientIdStr, out clientSecret))
            {
                using (var db = new AuthenticationModelContext())
                {
                    int clientId;
                    if (Int32.TryParse(context.ClientId, out clientId))
                    {
                        ConsumerModel consumerModel = db.ConsumerModels.Find(clientId);
                        if (clientId == consumerModel.ConsumerId &&
                            clientSecret == consumerModel.ConsumerSecret)
                        {
                            context.Validated();
                        }
                    }
                }
            }
            return Task.FromResult(0);
        }

        private Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var claims = new List<Claim>();
            try
            {
                for (int i = 0; i < context.Scope.Count; i++)
                {
                    string type = context.Scope[i];
                    string value = context.Scope[++i];
                    var claim = new Claim(type, value);
                    claims.Add(claim);
                }
            }
            catch (Exception ex)
            {
                //:TODO
                //Log.Error(ErrorMessage, ex);
                throw;
            }

            var identity = new ClaimsIdentity(new GenericIdentity(
                context.ClientId, OAuthDefaults.AuthenticationType), claims);

            context.Validated(identity);

            return Task.FromResult(0);
        }

        private Task GrantClientCredetails(OAuthGrantClientCredentialsContext context)
        {
            var claims = new List<Claim>();
            try
            {
                for (int i = 0; i < context.Scope.Count; i++)
                {
                    string type = context.Scope[i];
                    string value = context.Scope[++i];
                    var claim = new Claim(type, value);
                    claims.Add(claim);
                }
            }
            catch (Exception ex)
            {
                //:TODO
                //Log.Error(ErrorMessage, ex);
                throw;
            }

            var identity = new ClaimsIdentity(new GenericIdentity(
                context.ClientId, OAuthDefaults.AuthenticationType), claims);

            context.Validated(identity);

            return Task.FromResult(0);
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