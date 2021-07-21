using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using coreERP;
using coreErpApi.Controllers.Models;


namespace coreErpApi.Controllers.Controllers.Loans.LoanType
{
    //[AuthorizationFilter()]
    //[EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class ProfTypeController : ApiController
    {
        IcoreSecurityEntities le;

        public ProfTypeController()
        {
            le = new coreSecurityEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ProfTypeController(IcoreSecurityEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public IEnumerable<ProfileModel> GetAccessLevels()
        {
            var accessLevels = le.accessLevels
                .Select(p => new ProfileModel
                {
                    value = p.accessLevelName
                })
                .ToList();

            return accessLevels;
        }

        [HttpGet]
        // GET: api/
        public IEnumerable<ProfileModel> GetRoles()
        {
            var userRoles = le.roles
                .Select(p => new ProfileModel
                {
                    value = p.role_name
                })
                .ToList();

            return userRoles;
        }

        [HttpGet]
        // GET: api/
        public IEnumerable<ProfileModel> GetUsers()
        {
            var users = le.users
                .Select(p => new ProfileModel
                {
                    value = p.user_name
                })
                .ToList();
            return users;
        }

    }
}









