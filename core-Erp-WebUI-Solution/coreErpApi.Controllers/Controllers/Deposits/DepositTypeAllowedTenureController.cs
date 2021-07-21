using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [AuthorizationFilter()]
    public class DepositTypeAllowedTenureController : ApiController
    {
        IcoreLoansEntities le;

        public DepositTypeAllowedTenureController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositTypeAllowedTenureController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/depositType
        public IEnumerable<depositTypeAllowedTenure> Get()
        {
            return le.depositTypeAllowedTenures
                .OrderBy(p => p.depositTypeAllowedTenureId)
                .ToList();
        }

        // GET: api/depositType/5
        [HttpGet]
        public depositTypeAllowedTenure Get(int id)
        {
            depositTypeAllowedTenure value = le.depositTypeAllowedTenures
                .FirstOrDefault(p => p.depositTypeAllowedTenureId == id);

            if (value == null)
            {
                value = new depositTypeAllowedTenure();
            }
            return value;
        }

        [HttpPost]
        public depositTypeAllowedTenure Post(depositTypeAllowedTenure input)
        {
            le.depositTypeAllowedTenures
                .Add(input);
            le.SaveChanges();

            return input;
        }

        [HttpPut]
        public depositTypeAllowedTenure Put(depositTypeAllowedTenure input)
        {
            var toBeUpdated = le.depositTypeAllowedTenures.First(p => p.depositTypeAllowedTenureId == input.depositTypeAllowedTenureId);

            toBeUpdated.depositTypeId = input.depositTypeId;
            toBeUpdated.tenureTypeId = input.tenureTypeId;
            toBeUpdated.minTenure = input.minTenure;
            toBeUpdated.maxTenure = input.maxTenure;

            le.SaveChanges();

            return input;
        }

        [HttpDelete]
        public void Delete([FromBody]depositTypeAllowedTenure input)
        {
            var forDelete = le.depositTypeAllowedTenures.FirstOrDefault(p => p.depositTypeAllowedTenureId == input.depositTypeAllowedTenureId);
            if (forDelete != null)
            {
                le.depositTypeAllowedTenures.Remove(forDelete);
                le.SaveChanges();               
            }
        }

    }
}
