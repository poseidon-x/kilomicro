using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")] 
    public class ChargeTypeTierController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public ChargeTypeTierController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<chargeTypeTier> Get()
        {
            return le.chargeTypeTiers
                .AsNoTracking()
                .OrderBy(p => p.minChargeAmount)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public chargeTypeTier Get(int id)
        {
            return le.chargeTypeTiers
                .AsNoTracking()
                .FirstOrDefault(p => p.chargeTypeTierId == id);
        }

        [HttpPost]
        // POST: api/chargeTypeTier
        public chargeTypeTier Post([FromBody]chargeTypeTier value)
        {
            le.chargeTypeTiers.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]chargeTypeTier value)
        {
            var toBeUpdated = new chargeTypeTier
            {
                chargeTypeTierId = value.chargeTypeTierId,
                percentCharge = value.percentCharge,
                chargeTypeId = value.chargeTypeId,
                maximumTransactionAmount = value.maximumTransactionAmount,
                minChargeAmount = value.minChargeAmount,
                minimumTransactionAmount = value.minimumTransactionAmount,
                maturityPercentCharge = value.maturityPercentCharge,
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]chargeTypeTier value)
        {
            var forDelete = le.chargeTypeTiers.FirstOrDefault(p => p.chargeTypeTierId == value.chargeTypeTierId);
            if (forDelete != null)
            {
                le.chargeTypeTiers.Remove(forDelete);
                le.SaveChanges();
            }
        }

    }
}
