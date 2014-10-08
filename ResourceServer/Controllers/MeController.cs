using System.Web.Http;
using System.Web.Http.Cors;

namespace ResourceServer.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MeController : ApiController
    {
        public string Get()
        {
            return User.Identity.Name;
        }
    }
}