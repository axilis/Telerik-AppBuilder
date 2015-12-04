using Members.EF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;

namespace DataService1.Controllers
{
    
    public class MembersController : ODataController
    {
        private MembersEntities db = new MembersEntities();

        
        private bool MemberExists(int key)
        {
            return db.Member.Any(p => p.MemberId == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        
        [EnableQuery]
        public IQueryable<Member> Get()
        {
            return db.Member;
        }

        [EnableQuery]
        public SingleResult<Member> Get([FromODataUri] int key)
        {
            IQueryable<Member> result = db.Member.Where(p => p.MemberId == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Member product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Member.Add(product);
            await db.SaveChangesAsync();
            return Created(product);
        }

        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public async Task<IHttpActionResult> Merge([FromODataUri] int key, Delta<Member> member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Member.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            member.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }


        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Member> member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Member.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            member.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Member update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.MemberId)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var member = await db.Member.FindAsync(key);
            if (member == null)
            {
                return NotFound();
            }
            db.Member.Remove(member);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
