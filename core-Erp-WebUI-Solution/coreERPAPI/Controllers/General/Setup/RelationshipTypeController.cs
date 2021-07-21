using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreData.Constants;
using coreData.ErrorLog;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.hc
{
    [AuthorizationFilter()]
    public class RelationshipTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public RelationshipTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/relationType
        public IEnumerable<relationshipType> Get()
        {
            return le.relationshipTypes
                .OrderBy(p => p.relationshipTypeName)
                .ToList();
        }

        // GET: api/relationType/5
        [HttpGet]
        public relationshipType Get(int id)
        {
            return le.relationshipTypes
                .FirstOrDefault(p => p.relationshipTypeId == id);
        }

        [HttpPost]
        // POST: api/relationType
        public coreLogic.relationshipType Post([FromBody]coreLogic.relationshipType value)
        {
            le.relationshipTypes.Add(value);
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                Logger.logError("Error saving relationShip Type");
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
           

            return value;
        }

        [HttpPut]
        // PUT: api/relationType/5
        public relationshipType Put([FromBody]relationshipType value)
        {
            var toBeUpdated = le.relationshipTypes
                .FirstOrDefault(p => p.relationshipTypeId == value.relationshipTypeId);

            if (toBeUpdated != null)
            {
                toBeUpdated.relationshipTypeName = value.relationshipTypeName;
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                Logger.logError("Error Updating relationShip Type");
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return toBeUpdated;
        }

        
    }
}
