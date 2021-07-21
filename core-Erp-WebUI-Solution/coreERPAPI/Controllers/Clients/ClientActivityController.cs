using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using coreERP.Models;
using coreERP.Models.DashAppModel;
using System.Linq.Dynamic;
using System.Data.Entity;
using coreReports;

namespace coreERP.Controllers.Clients
{
    public class ClientActivityController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public ClientActivityController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/relationType
        [HttpGet]
        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req, long? id)
        {
            string order = "activityDate";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<clientActivityLog>(req, parameters);

            var query = le.clientActivityLogs.Where(p=> (id == null || id == -1 || p.clientID == id))
                        .ToList();
            
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            var data = query 
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take) 
                .ToArray();

            return new KendoResponse(data.ToArray(), query.Count()); 
        }
 
        [HttpGet]
        public IEnumerable<clientActivityType> GetTypes()
        {
            return le.clientActivityTypes.ToList();
        }

        [HttpPost]
        public clientActivityLog Post(clientActivityLog value)
        {
            if (value.clientActivityLogID < 1)
            {
                le.clientActivityLogs.Add(value);
            }
            else
            {
                return null;
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw x;
            }

            

            return value;
        }

        [HttpPut]
        public clientActivityLog Put(clientActivityLog value)
        {
            le.Entry(value).State = EntityState.Modified;
            le.SaveChanges();

            return value;
        }

        [HttpDelete]
        public void Delete(clientActivityLog value)
        {
            le.clientActivityLogs.Remove(value);
            le.SaveChanges();

            return;
        }
    }
}
