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
    public class CollateralTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public CollateralTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api//CollateralType
        public IEnumerable<collateralType> Get()
        {
            return le.collateralTypes
                .OrderBy(p => p.collateralTypeName)
                .ToList();
        }

        [HttpPost]
        // POST: api/CollateralType
        public void Post([FromBody]collateralType value)
        {
            le.collateralTypes.Add(value);
            le.SaveChanges();
        }

        [HttpPut]
        // PUT: api/CollateralType
        public void Put([FromBody]collateralType value)
        {
            var toBeUpdated = new collateralType
            {
                collateralTypeID = value.collateralTypeID,
                collateralTypeName = value.collateralTypeName
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/CollateralType
        public void Delete([FromBody]collateralType value)
        {
            var forDelete = le.collateralTypes.FirstOrDefault(p => p.collateralTypeID == value.collateralTypeID);
            if (forDelete != null)
            {
                le.collateralTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
