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
using coreERP;
using System.Web.Http.Cors;

namespace coreErpApi.Controllers.Controllers.Loans.LoanApproval
{
    //[AuthorizationFilter()]
    //[EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class ApprovalStageController : ApiController
    {
        IcoreLoansEntities le;
        IcoreSecurityEntities ctx;


        public ApprovalStageController()
        {
            le = new coreLoansEntities();
            ctx = new coreSecurityEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public ApprovalStageController(IcoreLoansEntities cent)
        {
            le = cent;
        }


        [HttpPost]
        // GET: api/
        public IEnumerable<loanApprovalStage> GetApprovalStages()
        {
            var approvals = le.loanApprovalStages
                .OrderBy(p => p.name)
                .ToList();
            return approvals;
        }

        public IEnumerable<loanApprovalStage> GetMyApprovalStages(int id)
        {
            var ln = le.loans.FirstOrDefault(p => p.loanID == id);
            int lnTypeId = ln.loanTypeID;
            var currentUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            string userAccessName = "";

            //Get user role & access Level
            IEnumerable<user_roles> userRoles = ctx.user_roles
                .Include(p => p.roles)
                .Where(p => p.users.user_name == currentUser)
                .ToList();
            List<string> currentUserRoles = new List<string>();
            foreach (var role in userRoles)
            {
                currentUserRoles.Add(role.roles.role_name);
            }
            //if (userRole != null) { userRoleName = userRole.roles.role_name; }
            var currentUserAccessLevel = ctx.users.FirstOrDefault(p => p.user_name == currentUser);
            if (currentUserAccessLevel != null)
            {
                var currentUserAccessLevelId = currentUserAccessLevel.accessLevelID;
                var userAccess = ctx.accessLevels.FirstOrDefault(p => p.accessLevelID == currentUserAccessLevelId);
                if (userAccess != null) userAccessName = userAccess.accessLevelName;
            }

            var myApprovalStages = le.loanApprovalStages
                .Where(p => p.loanTypeId == lnTypeId
                 && (p.loanApprovalStageOfficers.Any(m => m.profileType == "U" && m.profileValue == currentUser
                        || m.profileType == "R" && currentUserRoles.Contains(m.profileValue)
                        || m.profileType == "A" && m.profileValue == userAccessName)))
                .OrderBy(p => p.name)
                .ToList();
            return myApprovalStages;
        }


    }
}









