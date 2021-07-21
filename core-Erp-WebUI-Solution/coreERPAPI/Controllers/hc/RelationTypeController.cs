using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.hc
{
    [AuthorizationFilter()]
    public class RelationTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public RelationTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/relationType
        public IEnumerable<coreLogic.relationType> Get()
        {
            return le.relationTypes
                .OrderBy(p => p.relationTypeID)
                .ToList();
        }

        // GET: api/relationType/5
        [HttpGet]
        public coreLogic.relationType Get(int id)
        {
            return le.relationTypes
                .FirstOrDefault(p => p.relationTypeID == id);
        }

        [HttpPost]
        // POST: api/relationType
        public coreLogic.relationType Post([FromBody]coreLogic.relationType value)
        {
            le.relationTypes.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/relationType/5
        public coreLogic.relationType Put([FromBody]coreLogic.relationType value)
        {
            var toBeUpdated = new coreLogic.relationType
            {
                relationTypeID = value.relationTypeID,
                relationTypeName = value.relationTypeName,
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/relationType/5
        public void Delete([FromBody]coreLogic.relationType value)
        {
            var forDelete = le.relationTypes.FirstOrDefault(p => p.relationTypeID == value.relationTypeID);
            if (forDelete != null)
            {
                le.relationTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
