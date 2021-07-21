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
    public class LineOfBusinessController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public LineOfBusinessController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api//lineOfBusiness
        public IEnumerable<lineOfBusiness> Get()
        {
            return le.lineOfBusinesses
                .OrderBy(p => p.lineOfBusinessName)
                .ToList();
        }

        [HttpPost]
        // POST: api/lineOfBusiness
        public void Post([FromBody]lineOfBusiness value)
        {
            le.lineOfBusinesses.Add(value);
            le.SaveChanges();
        }

        [HttpPut]
        // PUT: api/lineOfBusiness
        public void Put([FromBody]lineOfBusiness value)
        {
            var toBeUpdated = new lineOfBusiness
            {
                lineOfBusinessID = value.lineOfBusinessID,
                lineOfBusinessName = value.lineOfBusinessName
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/lineOfBusiness
        public void Delete([FromBody]lineOfBusiness value)
        {
            var forDelete = le.lineOfBusinesses.FirstOrDefault(p => p.lineOfBusinessID == value.lineOfBusinessID);
            if (forDelete != null)
            {
                le.lineOfBusinesses.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
