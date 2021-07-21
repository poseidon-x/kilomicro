using coreERP.Providers;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setups
{
    [AuthorizationFilter()]
    public class GenericCheckListController : ApiController
    {
         coreLoansEntities le = new coreLoansEntities();

         public GenericCheckListController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

       // GET: api//genericCheckList
       public IEnumerable<genericCheckList> Get()
       {
           return le.genericCheckLists
               .OrderBy(p => p.description)
               .ToList();
       }

       // POST: api/genericCheckList
       public void Post([FromBody]genericCheckList value)
       {
           le.genericCheckLists.Add(value);
           le.SaveChanges();
       }

       [HttpPut]
       // PUT: api/genericCheckList
       public void Put([FromBody]genericCheckList value)
       {
           var toBeUpdated = new genericCheckList
           {
               genericCheckListID = value.genericCheckListID,
               description = value.description
           };
           le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
           le.SaveChanges();
       }

       [HttpDelete]
       // DELETE: api/genericCheckList
       public void Delete([FromBody]genericCheckList value)
       {
           var forDelete = le.genericCheckLists.FirstOrDefault(p => p.genericCheckListID == value.genericCheckListID);
           if (forDelete != null)
           {
               le.genericCheckLists.Remove(forDelete);
               le.SaveChanges();
           }
       }
    }
}
