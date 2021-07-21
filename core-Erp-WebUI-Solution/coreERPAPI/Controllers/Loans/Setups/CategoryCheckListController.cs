using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using coreERP;
using coreLogic;

namespace coreERP.Controllers
{
    public class CategoryCheckListController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public CategoryCheckListController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<categoryCheckList> Get()
        {
            return le.categoryCheckLists 
                .OrderBy(p=> p.description)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public categoryCheckList Get(int id)
        {
            return le.categoryCheckLists 
                .FirstOrDefault(p=> p.categoryCheckListID == id);
        }

        [HttpPost]
        // POST: api/Category
        public void Post([FromBody]categoryCheckList value)
        {
            var toBeInserted = new categoryCheckList
            {
                categoryCheckListID = value.categoryCheckListID,
                categoryID = value.categoryID,
                description = value.description,
                isMandatory = value.isMandatory
            };
            le.categoryCheckLists.Add(toBeInserted);
            le.SaveChanges(); 
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]categoryCheckList value)
        {
            var toBeUpdated = new categoryCheckList
            {
                categoryCheckListID = value.categoryCheckListID,
                categoryID = value.categoryID,
                description = value.description,
                isMandatory = value.isMandatory
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]categoryCheckList value)
        {
            var forDelete = le.categoryCheckLists.FirstOrDefault(p => p.categoryCheckListID == value.categoryCheckListID);
            if (forDelete != null)
            {
                le.categoryCheckLists.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
