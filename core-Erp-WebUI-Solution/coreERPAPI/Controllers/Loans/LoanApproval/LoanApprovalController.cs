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
using coreData.Constants;

namespace coreErpApi.Controllers.Controllers.Loans.LoanApproval
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class LoanApprovalController : ApiController
    {
        IcoreLoansEntities le;
        IcoreSecurityEntities ctx;
        ErrorMessages error = new ErrorMessages();


        public LoanApprovalController()
        {
            le = new coreLoansEntities();
            ctx = new coreSecurityEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public LoanApprovalController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpPost]
        // GET: api/
        public  IEnumerable<loan> Post(loan loanToApprove)
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
                if(userAccess!=null) userAccessName = userAccess.accessLevelName;
            }
            
            List<int> ids = new List<int>{3, 4, 7};
            var loansToApprove = le.loans
                .Where(p => !ids.Contains(p.loanStatusID)
                 && p.loanType.loanApprovalStages.Any()
                 && (p.loanType.loanApprovalStages.Any(i => i.loanApprovalStageOfficers
                                                        .Any(m => m.profileType == "U" && m.profileValue == currentUser
                                                        || m.profileType == "R" && m.profileValue == userRoleName
                                                        || m.profileType == "A" && m.profileValue == userAccessName))))
                .Include(p => p.loanApprovals)
                .ToList();
            var loan = le.loans.FirstOrDefault(p => p.loanID == loanToApprove.loanID);
            if (loan != null)
            {
                loan.insuranceAmount = loanToApprove.insuranceAmount;
                foreach (var approval in loanToApprove.loanApprovals)
                {
                    if (approval.loanApprovalId < 1)
                    {
                        loan.loanApprovals.Add(approval);
                    }
                }
            }
            le.SaveChanges();
            return loansToApprove;
        }

        [HttpGet]
        // GET: api/  return an instance of loan for approval
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

            var loansToApprove = le.loans
                .Include(p => p.loanApprovals)
                .FirstOrDefault(p => p.loanStatusID == 2 && p.loanID == id
                 && p.loanType.loanApprovalStages.Any()
                 && (p.loanType.loanApprovalStages.Any(i => i.loanApprovalStageOfficers
                                                        .Any(m => m.profileType == "U" && m.profileValue == currentUser
                                                        || m.profileType == "R" && m.profileValue == userRoleName
                                                        || m.profileType == "A" && m.profileValue == userAccessName))));
            return loansToApprove;
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
        // GET: api/
        public loan GetLoan(int id)
        {
            var approvals = le.loans
                .Include(p => p.loanApprovals)
                .FirstOrDefault(p => p.loanID == id);

            return approvals;
        }

        [HttpPost]
        // GET: api/ returns all approval stages for a loan
        public List<loanApprovalStage> GetLoanApprovalStages(int id)
        {
            var ln = le.loans.FirstOrDefault(p => p.loanID == id);
            var stages = le.loanApprovalStages
                .Where(p => p.loanTypeId == ln.loanTypeID) 
                .ToList();

            if (stages.Count == 0)
            {
                stages = new List<loanApprovalStage>();
            }
            return stages;
        }

        [HttpPost]
        // GET: api/ returns my approval stages for a particular loan
        public List<loanApprovalStage> GetMyLoanApprovalStages(int id)
        {

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

            //Get user role & access Level
            var currentUserAccessLevel = ctx.users.FirstOrDefault(p => p.user_name == currentUser);
            if (currentUserAccessLevel != null)
            {
                var currentUserAccessLevelId = currentUserAccessLevel.accessLevelID;
                var userAccess = ctx.accessLevels.FirstOrDefault(p => p.accessLevelID == currentUserAccessLevelId);
                if (userAccess != null) userAccessName = userAccess.accessLevelName;
            }

            var ln = le.loans.FirstOrDefault(p => p.loanID == id);

            var stages = le.loanApprovalStages
                .Where(p => p.loanTypeId == ln.loanTypeID && (p.loanApprovalStageOfficers.Any(m => m.profileType == "U" && m.profileValue == currentUser
                            || m.profileType == "R" && currentUserRoles.Contains(m.profileValue)
                            || m.profileType == "A" && m.profileValue == userAccessName)))
                .ToList();

            //var stages = le.loanApprovalStages.Where(p => p.loanTypeId == loan.loanTypeID).ToList();
            return stages;
        }


        [HttpPost]
        public string PostApproval(loan ln)
        {
            if (ln == null) return null;
            var currentloan = le.loans
                .Include(p => p.loanApprovals)
                .FirstOrDefault(p => p.loanID == ln.loanID);
            if (currentloan == null) return null;

            //Get the final approval stage for the current loan Type
            var loanTypeFinalStage = le.loanApprovalStages
                .Where(p => p.loanTypeId == currentloan.loanTypeID)
                .OrderByDescending(p => p.ordinal)
                .FirstOrDefault();

            //If the final Approval stage has been added already, Throw exception Loan Already fully Approved
            if (currentloan.loanApprovals.Any(p => p.approvalStageId == loanTypeFinalStage.loanApprovalStageId))
            { throw new ApplicationException(error.LoanFullyApprovedAlreadyErrorMsg); }

            foreach (var value in ln.loanApprovals)
            {
                if (value == null) return null;
                //If the loanApprovalId is set, we update the record
                if (value.loanApprovalId > 0)
                {
                    /////-----------Approval cannot be edited---------\\\\\\\
                    //var toBeSaved = le.loanApprovals
                    //    .FirstOrDefault(p => p.loanApprovalId == value.loanApprovalId);
                    //populateFields(toBeSaved, value);
                }
                else //If its not an update, we add a new approval
                {
                    //If the this approval stage is the final one, Set the loan to approved
                    if (value.approvalStageId == loanTypeFinalStage.loanApprovalStageId)
                    {
                        currentloan.loanStatusID = 3;
                        currentloan.insuranceAmount = ln.insuranceAmount;
                        currentloan.amountApproved = value.amountApproved;
                        currentloan.finalApprovalDate = value.approvalDate;
                        currentloan.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
                        currentloan.loanTenure = value.approvedTenure;
                    }
                    loanApproval toBeSaved = new loanApproval{loanId = ln.loanID};
                    populateFields(toBeSaved, value, currentloan);
                    le.loanApprovals.Add(toBeSaved);
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
            return "Approval stage saved succesfully";
        }

        private void populateFields(loanApproval target, loanApproval source, loan ln)
        {
            target.approvalDate = source.approvalDate;
            target.approvalAction = source.approvalAction;
            target.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            target.approvalStageId = source.approvalStageId;
            target.amountApproved = source.amountApproved;
            target.approvalComment = source.approvalComment;
            target.approvedTenure = source.approvedTenure;
            ln.loanTenure = source.approvedTenure;
            if (source.loanApprovalId < 1) target.created = DateTime.Now;
            if (source.approvalAction == "R")
            {
                ln.loanStatusID = 7;
            }
        }
    }
}









