using System;
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
    public class LocationTypeController : ApiController
    {
        ICommerceEntities le;

        public LocationTypeController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LocationTypeController(ICommerceEntities lent)
        {
            le = lent;
        }

        public IEnumerable<locationType> Get()
        {
            return le.locationTypes
                .OrderBy(p => p.locationTypeId)
                .ToList();
        }

        [HttpPost]
        public IEnumerable<locationType> PostNew(List<locationType> inputs)
        {
            if (inputs.Any())
            {
                foreach (var inp in inputs)
                {
                    le.locationTypes.Add(inp);
                }
            }

            le.SaveChanges();

            return inputs;
        }
     
        // GET: api/location/5
        [HttpGet]
        public locationType Get(int id)
        {
            return le.locationTypes
                .FirstOrDefault(p => p.locationTypeId == id);
        }
        [HttpPost]
        public KendoResponse Post(locationType input)
        {
            le.locationTypes
                .Add(input);
            le.SaveChanges();

            return new KendoResponse{Data = new locationType[] {input}};
        }
        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "locationTypeName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<locationType>(req, parameters);

            var query = le.locationTypes.AsQueryable();
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
        // PUT: api/locationTypes/5
        public KendoResponse Put([FromBody]locationType value)
        {
            var toBeUpdated = le.locationTypes.First(p => p.locationTypeId == value.locationTypeId);

            toBeUpdated.locationTypeId = value.locationTypeId;
            toBeUpdated.locationTypeName = value.locationTypeName;
            toBeUpdated.parentLocationTypeId = value.parentLocationTypeId;
            toBeUpdated.locationTypeCode = value.locationTypeCode;

            le.SaveChanges();

            return new KendoResponse { Data = new locationType[] { toBeUpdated } };
        }

        [HttpDelete]
        // DELETE: api/locationTypes/5
        public void Delete([FromBody]locationType value)
        {
            var forDelete = le.locationTypes.FirstOrDefault(p => p.locationTypeId == value.locationTypeId);
            if (forDelete != null)
            {
                le.locationTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }


    }//class
    
}//namespace
