using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.hc
{
    [AuthorizationFilter()]
    public class LeaveTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public LeaveTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/leaveType
        public IEnumerable<leaveType> Get()
        {
            return le.leaveTypes
                .OrderBy(p => p.leaveTypeID)
                .ToList();
        }

        // GET: api/leaveType/5
        [HttpGet]
        public leaveType Get(int id)
        {
            return le.leaveTypes
                .FirstOrDefault(p => p.leaveTypeID == id);
        }

        [HttpPost]
        // POST: api/leaveType
        public leaveType Post([FromBody]leaveType value)
        {
            le.leaveTypes.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/leaveType/5
        public leaveType Put([FromBody]leaveType value)
        {
            var toBeUpdated = new leaveType
            {
                leaveTypeID = value.leaveTypeID,
                leaveTypeName = value.leaveTypeName,
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/leaveType/5
        public void Delete([FromBody]leaveType value)
        {
            var forDelete = le.leaveTypes.FirstOrDefault(p => p.leaveTypeID == value.leaveTypeID);
            if (forDelete != null)
            {
                le.leaveTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
