using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Http; 
using coreLogic; 
using coreERP.Providers;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class EmployerController : ApiController
    {
        IcoreLoansEntities le;

        public EmployerController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        public EmployerController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/Category
        public IEnumerable<employer> Get()
        {
            return le.employers
                .OrderBy(p => p.employerName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public employer Get(int id)
        {
            return le.employers
                .FirstOrDefault(p => p.employerID == id);
        }

        // GET: api/Category/5
        [HttpGet]
        public employer[] GetByClient(int clientId)
        {
            var client = le.clients
                .Include(p => p.employeeCategories)
                .Include(p => p.employeeCategories.Select(q => q.employer))
                .First(p => p.clientID == clientId);
            return new employer[] {client.employeeCategories.First().employer};
        }

        [HttpPost]
        public KendoResponse Post(employer input)
        {
            le.employers
                .Add(input);
            le.SaveChanges();

            return new KendoResponse { Data = new employer[] { input } };
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "employerName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<employer>(req, parameters);

            var query = le.employers.AsQueryable();
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
        // PUT: api/depositType/5
        public KendoResponse Put([FromBody]employer value)
        {
            var toBeUpdated = le.employers.First(p => p.employerID == value.employerID);

            toBeUpdated.employerName = value.employerName;
            toBeUpdated.officeNumber = value.officeNumber; 
            toBeUpdated.employerAddressID = value.employerAddressID;
            toBeUpdated.employmentTypeID = value.employmentTypeID; 

            le.SaveChanges();

            return new KendoResponse { Data = new employer[] { toBeUpdated } };
        }

        [HttpDelete]
        // DELETE: api/depositType/5
        public void Delete([FromBody]employer value)
        {
            var forDelete = le.employers.FirstOrDefault(p => p.employerID == value.employerID);
            if (forDelete != null)
            {
                le.employers.Remove(forDelete);
                le.SaveChanges();
            }
        }

    }
}
