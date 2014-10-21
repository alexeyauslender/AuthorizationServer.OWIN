using System;
using System.Net.Http;
using System.Security.Claims;
using Constants;
using DotNetOpenAuth.OAuth2;

namespace ResourceOwnerPasswordCredentialGrant
{
    internal class Program
    {
        private static WebServerClient _webServerClient;
        private static string _accessToken;

        private static void Main(string[] args)
        {
            InitializeWebServerClient();

            Console.WriteLine("Requesting Token...");
            RequestToken();

            Console.WriteLine("Access Token: {0}", _accessToken);

            Console.WriteLine("Access Protected Resource");
            AccessProtectedResource();
        }

        private static void InitializeWebServerClient()
        {
            var authorizationServerUri = new Uri(Paths.AuthorizationServerBaseAddress);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, Paths.AuthorizePath),
                TokenEndpoint = new Uri(authorizationServerUri, Paths.TokenPath)
            };
            _webServerClient = new WebServerClient(authorizationServer);
        }

        private static void RequestToken()
        {
            IAuthorizationState state = _webServerClient.ExchangeUserCredentialForToken("aus@mail.ru", "123456",
                new[] { "SHOW_NOTES","SHOW_SOME_INFO" });
            _accessToken = state.AccessToken;
        }

        private static void AccessProtectedResource()
        {
            var resourceServerUri = new Uri(Paths.ResourceServerBaseAddress);
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            string body = client.GetStringAsync(new Uri(resourceServerUri, Paths.IdentityPath)).Result;
            Console.WriteLine(body);
        }
    }
}