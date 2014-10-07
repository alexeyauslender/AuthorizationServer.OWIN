using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizationServer.Constants
{
    public class Clients
    {
        public static Client Client1 { get; set; }

        public static Client Client2 { get; set; }

    }

    public class Client
    {
        public string Id { get; set; }

        public string Secret { get; set; }

        public string RedirectUrl { get; set; }
    }
}