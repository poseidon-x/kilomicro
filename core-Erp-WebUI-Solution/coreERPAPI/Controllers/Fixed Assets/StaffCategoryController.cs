using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.Fixed_Assets
{
    [AuthorizationFilter()]
    public class StaffCategoryController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public StaffCategoryController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/staffCategory
        public IEnumerable<staffCategory> Get()
        {
            return le.staffCategories
                .OrderBy(p => p.staffCategoryID)
                .ToList();
        }

        // GET: api/staffCategory/5
        [HttpGet]
        public staffCategory Get(int id)
        {
            return le.staffCategories
                .FirstOrDefault(p => p.staffCategoryID == id);
        }

        [HttpPost]
        // POST: api/staffCategory
        public staffCategory Post([FromBody]staffCategory value)
        {
            le.staffCategories.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/staffCategory/5
        public staffCategory Put([FromBody]staffCategory value)
        {
            var toBeUpdated = new staffCategory
            {
                staffCategoryID = value.staffCategoryID,
                staffCategoryName = value.staffCategoryName,
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/staffCategory/5
        public void Delete([FromBody]staffCategory value)
        {
            var forDelete = le.staffCategories.FirstOrDefault(p => p.staffCategoryID == value.staffCategoryID);
            if (forDelete != null)
            {
                le.staffCategories.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
