using coreERP.Providers;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setups
{
    [AuthorizationFilter()]
    public class SectorController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public SectorController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api//Sector
        public IEnumerable<sector> Get()
        {
            return le.sectors
                .OrderBy(p => p.sectorName)
                .ToList();
        }

        [HttpPost]
        // POST: api/Sector
        public void Post([FromBody]sector value)
        {
            le.sectors.Add(value);
            le.SaveChanges();
        }

        [HttpPut]
        // PUT: api/Sector
        public void Put([FromBody]sector value)
        {
            var toBeUpdated = new sector
            {
                sectorID = value.sectorID,
                sectorName = value.sectorName
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Sector
        public void Delete([FromBody]sector value)
        {
            var forDelete = le.sectors.FirstOrDefault(p => p.sectorID == value.sectorID);
            if (forDelete != null)
            {
                le.sectors.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
