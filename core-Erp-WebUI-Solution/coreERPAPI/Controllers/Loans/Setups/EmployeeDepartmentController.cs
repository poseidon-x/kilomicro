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
    public class EmployeeDepartmentController : ApiController
    {
        IcoreLoansEntities le;

        public EmployeeDepartmentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        public EmployeeDepartmentController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/Category
        public IEnumerable<employeeDepartment> Get()
        {
            return le.employeeDepartments
                .OrderBy(p => p.dapartmentName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public employeeDepartment Get(int id)
        {
            return le.employeeDepartments
                .FirstOrDefault(p => p.employeeDepartmentId == id);
        }

        
    }
}
