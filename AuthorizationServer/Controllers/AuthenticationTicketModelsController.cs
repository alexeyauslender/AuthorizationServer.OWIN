using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Authorization.Models;
using AuthorizationServer.Models;

namespace AuthorizationServer.Controllers
{
    public class AuthenticationTicketModelsController : ApiController
    {
        private readonly AuthenticationModelContext db = new AuthenticationModelContext();

        // GET: api/AuthenticationTicketModels
        public IQueryable<AuthenticationTicketModel> GetAuthenticationTicketModels()
        {
            return db.AuthenticationTicketModels;
        }

        // GET: api/AuthenticationTicketModels/5
        [ResponseType(typeof (AuthenticationTicketModel))]
        public async Task<IHttpActionResult> GetAuthenticationTicketModel(Guid id)
        {
            AuthenticationTicketModel authenticationTicketModel = await db.AuthenticationTicketModels.FindAsync(id);
            if (authenticationTicketModel == null)
            {
                return NotFound();
            }

            return Ok(authenticationTicketModel);
        }

        // PUT: api/AuthenticationTicketModels/5
        [ResponseType(typeof (void))]
        public async Task<IHttpActionResult> PutAuthenticationTicketModel(Guid id,
            AuthenticationTicketModel authenticationTicketModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != authenticationTicketModel.ContextToken)
            {
                return BadRequest();
            }

            db.Entry(authenticationTicketModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthenticationTicketModelExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AuthenticationTicketModels
        [ResponseType(typeof (AuthenticationTicketModel))]
        public async Task<IHttpActionResult> PostAuthenticationTicketModel(
            AuthenticationTicketModel authenticationTicketModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AuthenticationTicketModels.Add(authenticationTicketModel);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AuthenticationTicketModelExists(authenticationTicketModel.ContextToken))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtRoute("DefaultApi", new {id = authenticationTicketModel.ContextToken},
                authenticationTicketModel);
        }

        // DELETE: api/AuthenticationTicketModels/5
        [ResponseType(typeof (AuthenticationTicketModel))]
        public async Task<IHttpActionResult> DeleteAuthenticationTicketModel(Guid id)
        {
            AuthenticationTicketModel authenticationTicketModel = await db.AuthenticationTicketModels.FindAsync(id);
            if (authenticationTicketModel == null)
            {
                return NotFound();
            }

            db.AuthenticationTicketModels.Remove(authenticationTicketModel);
            await db.SaveChangesAsync();

            return Ok(authenticationTicketModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthenticationTicketModelExists(Guid id)
        {
            return db.AuthenticationTicketModels.Count(e => e.ContextToken == id) > 0;
        }
    }
}