using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Authorization.Models;
using AuthorizationServer.Models;

namespace AuthorizationServer.Controllers
{
    public class ConsumerModelsController : ApiController
    {
        private AuthenticationModelContext db = new AuthenticationModelContext();

        // GET: api/ConsumerModels
        public IQueryable<ConsumerModel> GetConsumerModels()
        {
            return db.ConsumerModels;
        }

        // GET: api/ConsumerModels/5
        [ResponseType(typeof(ConsumerModel))]
        public async Task<IHttpActionResult> GetConsumerModel(int id)
        {
            ConsumerModel consumerModel = await db.ConsumerModels.FindAsync(id);
            if (consumerModel == null)
            {
                return NotFound();
            }

            return Ok(consumerModel);
        }

        // PUT: api/ConsumerModels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutConsumerModel(int id, ConsumerModel consumerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != consumerModel.ConsumerId)
            {
                return BadRequest();
            }

            db.Entry(consumerModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsumerModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ConsumerModels
        [ResponseType(typeof(ConsumerModel))]
        public async Task<IHttpActionResult> PostConsumerModel(ConsumerModel consumerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ConsumerModels.Add(consumerModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = consumerModel.ConsumerId }, consumerModel);
        }

        // DELETE: api/ConsumerModels/5
        [ResponseType(typeof(ConsumerModel))]
        public async Task<IHttpActionResult> DeleteConsumerModel(int id)
        {
            ConsumerModel consumerModel = await db.ConsumerModels.FindAsync(id);
            if (consumerModel == null)
            {
                return NotFound();
            }

            db.ConsumerModels.Remove(consumerModel);
            await db.SaveChangesAsync();

            return Ok(consumerModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConsumerModelExists(int id)
        {
            return db.ConsumerModels.Count(e => e.ConsumerId == id) > 0;
        }
    }
}