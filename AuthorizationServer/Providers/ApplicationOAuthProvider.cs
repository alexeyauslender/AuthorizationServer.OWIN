using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Authorization.Models;
using AuthorizationServer.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace AuthorizationServer.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
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


        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            
            //Client credentials grant
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                using (var db = new AuthenticationModelContext())
                {
                    int iclientId;
                    if (Int32.TryParse(context.ClientId, out iclientId))
                    {
                        ConsumerModel consumerModel = db.ConsumerModels.Find(iclientId);
                        if (iclientId == consumerModel.ConsumerId &&
                            clientSecret == consumerModel.ConsumerSecret)
                        {
                            context.Validated();
                        }
                    }
                }
            }

            // Resource owner password credentials does not provide a client ID.
            else if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                OAuthDefaults.AuthenticationType);
            oAuthIdentity.AddClaims(context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(
        context.ClientId, OAuthDefaults.AuthenticationType),
        context.Scope.Select(x => new Claim("urn:oauth:scope", x))
        );

            context.Validated(identity);

            return Task.FromResult(0);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"userName", userName}
            };
            return new AuthenticationProperties(data);
        }
    }
}