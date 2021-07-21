using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Web.Http.Cors;
using coreErpApi.Controllers.Models;


namespace coreErpApi.Controllers.Controllers.Staff
{
    //[AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class StaffController : ApiController
    {
        IcoreLoansEntities le;

        public StaffController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public StaffController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        [AuthorizationFilter()]
        public IEnumerable<StaffViewModel> Get()
        {
            var staffViewList = new List<StaffViewModel>();
            var staffList = le.staffs.ToList();
            try
            {
                var user = User?.Identity?.Name.Trim()?.ToLower();
                if (!IsOwner(user))
                {
                    var userBranchId = GetUserStaffBranchId(user);
                    staffList = staffList
                        .Where(p=>p.userName.Trim().ToLower()==user.Trim().ToLower() && p.branchID==userBranchId).ToList();
                }
                staffViewList = staffList
                    .Select(p => new StaffViewModel
                    {
                        staffId = p.staffID,
                        staffName = p.surName + ", " + p.otherNames,
                        staffNo = p.staffNo,
                        staffNameWithStaffNo = p.surName + ", " + p.otherNames + " - " + p.staffNo
                    })
                    .OrderBy(i => i.staffName)
                    .ToList();
                
            }
            catch (Exception e)
            {
            }            
            return staffViewList;
        }

        // GET: api/
        [HttpGet]
        public StaffViewModel Get(int id)
        {
            return le.staffs
                .Select(p => new StaffViewModel
                {
                    staffId = p.staffID,
                    staffName = p.surName + ", " + p.otherNames,
                    staffNo = p.staffNo
                })
                .FirstOrDefault(i => i.staffId == id);
        }

        #region OTHER HELPERS

        private bool IsOwner(string userName)
        {
            try
            {
                var secEnt = new coreSecurityEntities();
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => p.roles.role_name.Trim().ToLower() == "owner" && p.users.user_name.Trim().ToLower() == userName.ToLower()).ToList();
                if (userRoles != null && userRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private int GetUserStaffBranchId(string logginUser)
        {
            var staffBranchId = le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == logginUser.ToLower())?.branchID;
            return staffBranchId.Value;
        }

        #endregion OTHER HELPERS


    }
}
