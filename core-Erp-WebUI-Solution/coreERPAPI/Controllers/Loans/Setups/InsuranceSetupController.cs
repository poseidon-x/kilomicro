using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.Loans.Setups
{
    [AuthorizationFilter()]
    public class InsuranceSetupController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public InsuranceSetupController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/insuranceSetup
        public IEnumerable<insuranceSetup> Get()
        {
            return le.insuranceSetups
                .OrderBy(p => p.insuranceSetupID)
                .ToList();
        }

        // GET: api/insuranceSetup/5
        [HttpGet]
        public insuranceSetup Get(int id)
        {
            return le.insuranceSetups
                .FirstOrDefault(p => p.insuranceSetupID == id);
        }

        [HttpPost]
        // POST: api/insuranceSetup
        public insuranceSetup Post([FromBody]insuranceSetup value)
        {
            le.insuranceSetups.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/insuranceSetup/5
        public insuranceSetup Put([FromBody]insuranceSetup value)
        {
            var toBeUpdated = new insuranceSetup
            {
                insuranceSetupID = value.insuranceSetupID,
                loanTypeID = value.loanTypeID,
                insurancePercent = value.insurancePercent,
                insuranceAccountID = value.insuranceAccountID,
                isEnabled = value.isEnabled
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/insuranceSetup/5
        public void Delete([FromBody]insuranceSetup value)
        {
            var forDelete = le.insuranceSetups.FirstOrDefault(p => p.insuranceSetupID == value.insuranceSetupID);
            if (forDelete != null)
            {
                le.insuranceSetups.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
