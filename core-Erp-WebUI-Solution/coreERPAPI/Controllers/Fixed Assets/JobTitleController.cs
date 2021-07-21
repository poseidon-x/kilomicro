using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.Fixed_Assets
{
    [AuthorizationFilter()]
    public class JobTitleController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public JobTitleController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/jobTitles
        public IEnumerable<jobTitle> Get()
        {
            return le.jobTitles
                .OrderBy(p => p.jobTitleID)
                .ToList();
        }

        // GET: api/jobTitles/5
        [HttpGet]
        public jobTitle Get(int id)
        {
            return le.jobTitles
                .FirstOrDefault(p => p.jobTitleID == id);
        }

        [HttpPost]
        // POST: api/jobTitles
        public jobTitle Post([FromBody]jobTitle value)
        {
            le.jobTitles.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/jobTitles/5
        public jobTitle Put([FromBody]jobTitle value)
        {
            var toBeUpdated = new jobTitle
            {
                jobTitleID = value.jobTitleID,
                jobTitleName = value.jobTitleName,
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/jobTitles/5
        public void Delete([FromBody]jobTitle value)
        {
            var forDelete = le.jobTitles.FirstOrDefault(p => p.jobTitleID == value.jobTitleID);
            if (forDelete != null)
            {
                le.jobTitles.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
