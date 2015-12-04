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


namespace Devices.Service.Controllers
{
    [EnableCors(origins: "http://localhost:49734", headers: "*", methods: "*")]
    public class DeviceController : ODataController
    {
        private MembersEntities db = new MembersEntities();


        private bool DeviceExists(int key)
        {
            return db.Device.Any(p => p.DeviceId == key);
        }

        private bool DeviceExists(string key)
        {
            return db.Device.Any(p => p.DeviceUUID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        [EnableQuery]
        //[Authorize]
        public IQueryable<Device> Get()
        {
            return db.Device;
        }

        [EnableQuery]
        //[Authorize]
        public SingleResult<Device> Get([FromODataUri] int key)
        {
            IQueryable<Device> result = db.Device.Where(p => p.DeviceId == key);
            return SingleResult.Create(result);
        }
        public async Task<IHttpActionResult> Post(Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceExists = await db.Device.FirstOrDefaultAsync(x => x.DeviceUUID == device.DeviceUUID);

            if (deviceExists != null)
            {

                deviceExists.PushToken = device.PushToken;
                deviceExists.DateModifed = DateTime.Now;
                
                //device.DeviceId = deviceExists.DeviceId;
                //db.Device.Attach(device);
                //db.Entry(device).State = EntityState.Modified;
                

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw;
                }

                return Updated(device);

            }
            else
            {

                device.DateCreated = DateTime.Now;

                db.Device.Add(device);

                await db.SaveChangesAsync();

                return Created(device);
            }

        }
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Device> device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Device.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            device.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(key))
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

        
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Device update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.DeviceId)
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
                if (!DeviceExists(key))
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

        //[Authorize]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await db.Device.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            db.Device.Remove(product);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}