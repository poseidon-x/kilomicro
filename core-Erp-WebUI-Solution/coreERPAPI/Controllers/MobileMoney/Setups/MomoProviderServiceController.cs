using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class MomoProviderServiceController : ApiController
    {
        momoModelsConnectionString le = new momoModelsConnectionString();

        public MomoProviderServiceController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<momoService> Get()
        {
            return le.momoServices 
                .OrderBy(p=> p.serviceName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public momoService Get(int id)
        {
            return le.momoServices 
                .FirstOrDefault(p=> p.serviceID == id);
        }

        [HttpPost]
        // POST: api/Category
        public momoService Post([FromBody]momoService value)
        {
            le.momoServices.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]momoService value)
        {
            var toBeUpdated = new momoService
            {
                providerID = value.providerID,
                serviceName = value.serviceName,
                serviceID = value.serviceID
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]momoService value)
        {
            var forDelete = le.momoServices.FirstOrDefault(p => p.serviceID == value.serviceID);
            if (forDelete != null)
            {
                le.momoServices.Remove(forDelete);
                le.SaveChanges();
            }
        }
         
    }
}
