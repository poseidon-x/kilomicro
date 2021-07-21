using coreERP.Providers;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace coreERP.Controllers.Loans.Setups
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class IndustryController : ApiController
    {
      coreLoansEntities le = new coreLoansEntities();

       public IndustryController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

       // GET: api//Industry
       public IEnumerable<industry> Get()
       {
           return le.industries
               .OrderBy(p => p.industryName)
               .ToList();
       }

       // POST: api/Industry
       public void Post([FromBody]industry value)
       {
           le.industries.Add(value);
           le.SaveChanges();
       }

       [HttpPut]
       // PUT: api/Industry
       public void Put([FromBody]industry value)
       {
           var toBeUpdated = new industry
           {
               industryID = value.industryID,
               industryName = value.industryName
           };
           le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
           le.SaveChanges();
       }

       [HttpDelete]
       // DELETE: api/Industry
       public void Delete([FromBody]industry value)
       {
           var forDelete = le.industries.FirstOrDefault(p => p.industryID == value.industryID);
           if (forDelete != null)
           {
               le.industries.Remove(forDelete);
               le.SaveChanges();
           }
       }
    }
}
