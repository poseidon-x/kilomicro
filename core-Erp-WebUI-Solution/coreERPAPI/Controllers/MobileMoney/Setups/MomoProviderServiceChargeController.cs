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
    public class MomoProviderServiceChargeController : ApiController
    {
        momoModelsConnectionString le = new momoModelsConnectionString();

        public MomoProviderServiceChargeController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<momoServiceCharge> Get()
        {
            return le.momoServiceCharges
                .OrderBy(p=> p.minTranAmount)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public momoServiceCharge Get(int id)
        {
            return le.momoServiceCharges 
                .FirstOrDefault(p=> p.serviceChargeID == id);
        }

        [HttpPost]
        // POST: api/Category
        public momoServiceCharge Post([FromBody]momoServiceCharge value)
        {
            le.momoServiceCharges.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]momoServiceCharge value)
        {
            var toBeUpdated = new momoServiceCharge
            { 
                serviceID = value.serviceID,
                maxTranAmount = value.maxTranAmount,
                minTranAmount = value.minTranAmount,
                chargesValue = value.chargesValue,
                isPercent = value.isPercent,
                serviceChargeID = value.serviceChargeID
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]momoServiceCharge value)
        {
            var forDelete = le.momoServiceCharges.FirstOrDefault(p => p.serviceChargeID == value.serviceChargeID);
            if (forDelete != null)
            {
                le.momoServiceCharges.Remove(forDelete);
                le.SaveChanges();
            }
        }

    }
}
