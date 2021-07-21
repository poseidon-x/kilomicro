using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Inventory.Setup
{
    [AuthorizationFilter()]
    public class UnitOfMeasurementController : ApiController
    {
        CommerceEntities le;

        public UnitOfMeasurementController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public UnitOfMeasurementController(CommerceEntities lent)
        {
            le = lent;
        }

        public IEnumerable<unitOfMeasurement> Get()
        {
            return le.unitOfMeasurements
                .OrderBy(p => p.unitOfMeasurementId)
                .ToList();
        }

        // GET: api/location/5
        [HttpGet]
        public unitOfMeasurement Get(int id)
        {
            return le.unitOfMeasurements
                .FirstOrDefault(p => p.unitOfMeasurementId == id);
        }
        [HttpPost]
        public KendoResponse Post(unitOfMeasurement input)
        {
            le.unitOfMeasurements
                .Add(input);
            le.SaveChanges();

            return new KendoResponse { Data = new unitOfMeasurement[] { input } };
        }
        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "unitOfMeasurementId";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<unitOfMeasurement>(req, parameters);

            var query = le.unitOfMeasurements.AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPut]
        // PUT: api/unitOfMeasurements/5
        public KendoResponse Put([FromBody]unitOfMeasurement value)
        {
            var toBeUpdated = le.unitOfMeasurements.First(p => p.unitOfMeasurementId == value.unitOfMeasurementId);

            toBeUpdated.unitOfMeasurementId = value.unitOfMeasurementId;
            toBeUpdated.unitOfMeasurementName = value.unitOfMeasurementName;
            toBeUpdated.complexDetailUnitOfMeasurementId = value.complexDetailUnitOfMeasurementId;
            toBeUpdated.numberOfUnits = value.numberOfUnits;
            toBeUpdated.createdBy = value.createdBy;
            toBeUpdated.modifiedBy = value.modifiedBy;
            toBeUpdated.modifiedDate = value.modifiedDate;

            le.SaveChanges();

            return new KendoResponse { Data = new unitOfMeasurement[] { toBeUpdated } };
        }

        [HttpDelete]
        // DELETE: api/unitOfMeasurements/5
        public void Delete([FromBody]unitOfMeasurement value)
        {
            var forDelete = le.unitOfMeasurements.FirstOrDefault(p => p.unitOfMeasurementId == value.unitOfMeasurementId);
            if (forDelete != null)
            {
                le.unitOfMeasurements.Remove(forDelete);
                le.SaveChanges();
            }
        }


    }//class

}//namespace
