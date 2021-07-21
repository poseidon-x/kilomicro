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
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class LoanPendingApprovalController : ApiController
    {
        IcoreLoansEntities le;
        IcoreSecurityEntities ctx;


        public LoanPendingApprovalController()
        {
            le = new coreLoansEntities();
            ctx = new coreSecurityEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public LoanPendingApprovalController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public  IEnumerable<loan> GetLoansPendingMyApproval()
        {
            var currentUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            string userAccessName = "";

            //Get user role & access Level
            IEnumerable<user_roles> userRoles = ctx.user_roles
                .Include(p=>p.roles)
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
                if(userAccess!=null) userAccessName = userAccess.accessLevelName;
            }
            
            var loansToApprove = le.loans
                .Include(p => p.loanApprovals)
                .Where(p => p.loanStatusID == 2
                 && p.loanType.loanApprovalStages.Any()
                 && (p.loanType.loanApprovalStages.Any(i => i.loanApprovalStageOfficers
                                                        .Any(m => m.profileType == "U" && m.profileValue == currentUser
                                                        || m.profileType == "R" && currentUserRoles.Contains(m.profileValue)
                                                        || m.profileType == "A" && m.profileValue == userAccessName))))
                .Include(p => p.loanApprovals)
                .ToList();

            return loansToApprove;
        }

        [HttpGet]
        // GET: api/  returns all loans pending my approval
        public loan GetLoanPendingMyApproval(int id)
        {
            var currentUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            string userRoleName = "";
            string userAccessName = "";

            //Get user role & access Level
            var userRole = ctx.user_roles.FirstOrDefault(p => p.users.user_name == currentUser);
            if (userRole != null) { userRoleName = userRole.roles.role_name; }
            var currentUserAccessLevel = ctx.users.FirstOrDefault(p => p.user_name == currentUser);
            if (currentUserAccessLevel != null)
            {
                var currentUserAccessLevelId = currentUserAccessLevel.accessLevelID;
                var userAccess = ctx.accessLevels.FirstOrDefault(p => p.accessLevelID == currentUserAccessLevelId);
                if (userAccess != null) userAccessName = userAccess.accessLevelName;
            }

            var loanToApprove = le.loans
                .FirstOrDefault(p => p.loanStatusID == 2 && p.loanID == id
                 && p.loanType.loanApprovalStages.Any()
                 && (p.loanType.loanApprovalStages.Any(i => i.loanApprovalStageOfficers
                                                        .Any(m => m.profileType == "U" && m.profileValue == currentUser
                                                        || m.profileType == "R" && m.profileValue == userRoleName
                                                        || m.profileType == "A" && m.profileValue == userAccessName))));
            return loanToApprove;
        }

        [HttpPost]
        // GET: api/
        public IEnumerable<loanApprovalStage> GetApproval(int id)
        {
            var approvals = le.loanApprovalStages
                .Include(p => p.loanApprovalStageOfficers)
                .Where(p => p.loanTypeId == id)
                .OrderBy(p => p.name)
                .ToList();
            if (!approvals.Any())
            {
                approvals = new List<loanApprovalStage>();
            }

            return approvals;
        }

        [HttpPost]
        public string Post(IEnumerable<loanApprovalStage> approvals)
        {
            foreach (var value in approvals)
            {
                if (value == null) return null;
                //Validate the input value

                if (value.loanApprovalStageId > 0)
                {
                    var toBeSaved = le.loanApprovalStages
                        .Include(p => p.loanApprovalStageOfficers)
                        .FirstOrDefault(p => p.loanApprovalStageId == value.loanApprovalStageId);
                    populateFields(toBeSaved, value);
                }
                else
                {
                    loanApprovalStage toBeSaved = new loanApprovalStage();
                    populateFields(toBeSaved, value);
                    le.loanApprovalStages.Add(toBeSaved);
                }
                
            }
            

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw x;
            }
            return "Approval saved succesfully";
        }

        private void populateFields(loanApprovalStage target, loanApprovalStage source)
        {
            target.loanTypeId = source.loanTypeId;
            target.name = source.name;
            target.isMandatory = source.isMandatory;
            target.ordinal = source.ordinal;
            
            if (source.loanApprovalStageOfficers.Count < 1)
                throw new ApplicationException("Please add Approval Officer");

            foreach (var stageOfficer in source.loanApprovalStageOfficers)
            {
                //Remove deleted loanApprovalStageOfficers from loanApprovalStage
                List<int> stageOfficerIds = source.loanApprovalStageOfficers.Select(p => p.loanApprovalStageOfficerId).ToList();
                foreach (loanApprovalStageOfficer staOff in target.loanApprovalStageOfficers)
                {
                    if (!stageOfficerIds.Contains(staOff.loanApprovalStageOfficerId))
                        le.loanApprovalStageOfficers.Remove(staOff);
                }

                if (stageOfficer.loanApprovalStageOfficerId < 1)
                {
                    var tobeSaved = new loanApprovalStageOfficer();
                    populateStageOfficerFields(tobeSaved, stageOfficer);
                    target.loanApprovalStageOfficers.Add(tobeSaved);
                }
            }

        }

        private void populateStageOfficerFields(loanApprovalStageOfficer target, loanApprovalStageOfficer source)
        {
            target.profileType = source.profileType;
            target.profileValue = source.profileValue;
        }



    }
}









