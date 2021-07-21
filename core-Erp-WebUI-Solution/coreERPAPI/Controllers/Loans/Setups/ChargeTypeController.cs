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
    public class ChargeTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public ChargeTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<chargeType> Get()
        {
            return le.chargeTypes
                .AsNoTracking()
                .OrderBy(p => p.chargeTypeName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public chargeType Get(int id)
        {
            return le.chargeTypes
                .AsNoTracking()
                .FirstOrDefault(p => p.chargeTypeID == id);
        }

        [HttpPost]
        // POST: api/chargeType
        public chargeType Post([FromBody]chargeType value)
        {
            le.chargeTypes.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]chargeType value)
        {
            var toBeUpdated = new chargeType
            {
                chargeTypeID = value.chargeTypeID,
                automatic = value.automatic,
                chargeTypeCode = value.chargeTypeCode,
                chargeTypeName = value.chargeTypeName, 
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]chargeType value)
        {
            var forDelete = le.chargeTypes.FirstOrDefault(p => p.chargeTypeID == value.chargeTypeID);
            if (forDelete != null)
            {
                le.chargeTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }

    }
}
