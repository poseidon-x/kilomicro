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
    public class EmployeeContractTypeController : ApiController
    {
        IcoreLoansEntities le;

        public EmployeeContractTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        public EmployeeContractTypeController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/Category
        public IEnumerable<employeeContractType> Get()
        {
            return le.employeeContractTypes
                .OrderBy(p => p.employeeContractTypeName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public employeeContractType Get(int id)
        {
            return le.employeeContractTypes
                .FirstOrDefault(p => p.employeeContractTypeID == id);
        }

        

    }
}
