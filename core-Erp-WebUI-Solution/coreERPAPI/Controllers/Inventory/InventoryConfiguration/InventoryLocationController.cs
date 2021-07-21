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
    public class InventoryLocationController : ApiController
    {
        CommerceEntities le;

        public InventoryLocationController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public InventoryLocationController(CommerceEntities lent)
        {
            le = lent;
        }

        public IEnumerable<location> Get()
        {
            return le.locations
                .OrderBy(p => p.locationId)
                .ToList();
        }
        // GET: api/location/5
        [HttpGet]
        public location Get(int id)
        {
            return le.locations
                .FirstOrDefault(p => p.locationId == id);
        }

        [HttpPost]
        public KendoResponse Post(location input)
        {
            le.locations
                .Add(input);
            le.SaveChanges();

            return new KendoResponse { Data = new location[] { input } };
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "locationName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<location>(req, parameters);

            var query = le.locations.AsQueryable();
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
        // PUT: api/location/5
        public KendoResponse Put([FromBody]location value)
        {
            var toBeUpdated = le.locations.First(p => p.locationId == value.locationId);

            toBeUpdated.locationId = value.locationId;
            toBeUpdated.locationCode = value.locationName;
            toBeUpdated.locationName = value.locationName;
            toBeUpdated.locationTypeId = value.locationTypeId;
            toBeUpdated.physicalAddress = value.physicalAddress;
            toBeUpdated.cityId = value.cityId;
            toBeUpdated.isActive = value.isActive;
            toBeUpdated.longitude = value.longitude;
            toBeUpdated.lattitude = value.lattitude;

            le.SaveChanges();

            return new KendoResponse { Data = new location[] { toBeUpdated } };
        }

        [HttpDelete]
        // DELETE: api/location/5
        public void Delete([FromBody]location value)
        {
            var forDelete = le.locations.FirstOrDefault(p => p.locationId == value.locationId);
            if (forDelete != null)
            {
                le.locations.Remove(forDelete);
                le.SaveChanges();
            }
        }

    }//class

}//namespace
