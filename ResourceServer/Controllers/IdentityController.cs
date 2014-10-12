using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ResourceServer.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IdentityController : ApiController
    {
        public IEnumerable<object> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return claims;
        }

        //public Dictionary<string, string> Get()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    Claim[] claims = identity.Claims.ToArray();
        //    var retDictionary = new Dictionary<string, string>();
        //    for (int i = 0; i < claims.Length - 1; i++)
        //    {
        //        if (i % 2 == 0) continue;
        //        if (claims[i].Type == "urn:oauth:scope")
        //        {
        //            retDictionary.Add(claims[i].Value, claims[i + 1].Value);
        //        }
        //    }

        //    return retDictionary;
        //}
    }
}