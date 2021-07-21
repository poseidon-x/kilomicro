using coreData.Constants;
using coreErp.Models.Loan;
using coreERP.Models;
using coreERP.Providers;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setup
{
    public class BatchGroupLoanDisbursementController : ApiController
    {

        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";

        public BatchGroupLoanDisbursementController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public BatchGroupLoanDisbursementController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        [AuthorizationFilter]
        [HttpPost]
        public IEnumerable<CombinedLoanBatchDisburseCheckListModel> GetBatchGroupLoanDisbursement(CombinedBatchDisburseChecklistVM input)
        {
            //TODO: Check for logging user
            if (input == null) return null;
            var logUser = User?.Identity?.Name?.Trim()?.ToLower();           

            var group = le.loanGroups
            .Include(p => p.loanGroupClients)
            .FirstOrDefault(p => p.loanGroupId == input.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var grps = le.loans.Include(c => c.client)
                .Where(p => (p.loanStatusID == 1 || p.loanStatusID==2 || p.loanStatusID==3) && p.loanTypeID == 10
                    && groupClients.Contains(p.clientID))
                .Select(p => new CombinedLoanBatchDisburseCheckListModel
                {
                    amountApproved = p.amountApproved,
                    amountDisbursed = p.amountRequested,
                    approvalDate = p.applicationDate,
                    amountRequested = p.amountRequested,
                    disbursementDate = input.approvalDate,
                    approved = false,
                    clientId = p.clientID,
                    clientFullName = p.client.surName + ", " + p.client.otherNames,
                    loanNumber = p.loanNo,
                    loanId = p.loanID

                })
                .ToList()
                .GroupBy(p => new
                {
                    p.loanNumber,
                    p.loanId,
                    p.clientFullName,
                    p.amountRequested,
                    p.approved
                })
                .Select(p => new CombinedLoanBatchDisburseCheckListModel
                {
                    amountApproved = p.Sum(r => r.amountApproved),
                    amountDisbursed = p.Sum(c => c.amountDisbursed),
                    approvalDate = p.Min(r => r.approvalDate),
                    amountRequested = p.Min(r => r.amountRequested),
                    disbursementDate = p.Min(r => r.disbursementDate),
                    loanId = p.Key.loanId,
                    loanNumber = p.Key.loanNumber,
                    clientFullName = p.Key.clientFullName,
                    approved = p.Key.approved,
                    clientId = p.Max(r => r.clientId)

                })
                .OrderByDescending(p => p.approvalDate)
                .ThenBy(p => p.clientFullName)
                .ToList();

            foreach (var item in grps)
            {
                item.checkListItems = new List<LoansCheckListModel>();
                var lg = le.loanGroupClients
                    .Include(c => c.client)
                    .Include(c => c.client.category)
                    .Include(c => c.client.category.categoryCheckLists)
                    .FirstOrDefault(p => p.clientId == item.clientId);
                if (lg != null)
                {
                    item.groupAddedDate = lg.created;
                    var cg = le.loanGroups.FirstOrDefault(p => p.loanGroupId == lg.loanGroupId);
                    if (cg != null)
                    {
                        item.groupName = cg.loanGroupName;
                    }
                    var categoryCheckListItems = lg.client.category.categoryCheckLists.ToList();
                    foreach (var categoryItem in categoryCheckListItems)
                    {
                        item.checkListItems.Add(new LoansCheckListModel
                        {
                            categoryId = categoryItem.categoryID,
                            description = categoryItem.description,
                            isMandatory = categoryItem.isMandatory
                        });
                    }
                    var genericCheckListItems = le.genericCheckLists.ToList();
                    foreach (var categoryItem in genericCheckListItems)
                    {
                        item.checkListItems.Add(new LoansCheckListModel
                        {
                            categoryId = categoryItem.genericCheckListID,
                            description = categoryItem.description,
                            isMandatory = true
                        });
                    }
                }

                //Get the branch for client here
                item.BranchName = GetBranchNameForClient(item.clientId);
            }
            return grps.OrderBy(p => p.BranchName).ThenBy(r => r.clientFullName).ThenBy(r => r.groupName);
        }

        [AuthorizationFilter]
        [HttpPost]
        public bool PostBatchGroupLoanDisbursement(CombinedBatchDisburseChecklistVM input)
        {
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var user =
                (new coreSecurityEntities()).users
                .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());

            var ctl =
                le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == user.user_name.ToLower().Trim());
            if (ctl == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                               user.full_name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ctl.cashiersTillID && p.tillDay == input.approvalDate.Date
                && p.open);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" +
                                               user.full_name + ")");
            }
            if (input.approvalItemsList.Any())
            {
                var batch = new CombinedBatchDisburseChecklistVM
                {
                    groupId = input.groupId,
                    approvalDate = input.approvalDate,
                    disbursementDate = input.approvalDate,
                    approvalItemsList = new List<CombinedLoanBatchDisburseCheckListModel>()
                };
                foreach (var rep in input.approvalItemsList.Where(p => p.approved))
                {
                    batch.approvalItemsList.Add(new CombinedLoanBatchDisburseCheckListModel
                    {
                        loanId = rep.loanId,
                        loanNumber = rep.loanNumber,
                        amountApproved = rep.amountRequested,
                        amountDisbursed = rep.amountDisbursed,
                        approvalDate = rep.approvalDate
                    });

                }
                PostBatchDisburseChecklistInner(batch,user,ctl);
                return true;
            }
            else
            {
                throw new ApplicationException("No data to post");
            }
        }

        private string PostBatchDisburseChecklistInner(CombinedBatchDisburseChecklistVM batchApprovals,users user,cashiersTill ctl)
        {
            if (batchApprovals == null) return null;
            double totalBatchAmount = 0;
            StringBuilder lns = new StringBuilder();
            foreach (var ln in batchApprovals.approvalItemsList)
            {
                //amount disbursed is greater than amount approved
                if (ln.amountDisbursed > ln.amountRequested)
                    lns.Append(ln.loanNumberWithName + "has amount disbursed greater than approved <br/>");

                totalBatchAmount = batchApprovals.approvalItemsList.Select(p => p.amountDisbursed).Sum();
            }
            


            if (user.accessLevel.disbursementLimit < totalBatchAmount)
            {
                throw new ApplicationException("The amount to be disbursed is beyond your access level");
            }
            

            foreach (var checklist in batchApprovals.approvalItemsList)
            {
                var currentLoan = le.loans
                    .FirstOrDefault(p => p.loanID == checklist.loanId);
                if (currentLoan != null)
                {
                    currentLoan.checkedBy = user.user_name;
                    currentLoan.modification_date = DateTime.Now;
                    currentLoan.last_modifier = user.user_name;
                    currentLoan.loanStatusID = 3;
                    currentLoan.disbursedBy = user.user_name;
                    currentLoan.amountApproved = checklist.amountApproved;
                    currentLoan.finalApprovalDate = checklist.approvalDate;
                    currentLoan.disbursementDate = checklist.approvalDate.Date;
                    currentLoan.approvedBy = user.user_name;
                    currentLoan.approvalComments = $"Batch Group Loan Checklist and Disbursement by {user.full_name} on {DateTime.Now:dd-MMM-yyyy}";

                    var cd = new cashierDisbursement();
                    //TODO: Check if the same loan is in cashier Disbursements
                    var existLoan = le.cashierDisbursements.Where(p => p.clientID == currentLoan.clientID && p.loanID == currentLoan.loanID).ToList();
                    if(existLoan !=null && existLoan.Count > 0)
                    {
                        throw new ApplicationException($"The same Loan {currentLoan.loanNo} for {checklist.clientFullName} is in the cashier till, awaiting posting on {DateTime.Now.Date}");
                    }
                    cd.cashierTillID = ctl.cashiersTillID;
                    cd.txDate = checklist.approvalDate.Date;
                    cd.amount = checklist.amountDisbursed;
                    cd.clientID = currentLoan.clientID;
                    cd.loanID = checklist.loanId;
                    cd.posted = false;
                    cd.bankID = checklist.bankId;
                    cd.checkNo = checklist.chequeNumber;
                    cd.postToSavingsAccount = false;
                    cd.paymentModeID = 1;
                    le.cashierDisbursements.Add(cd);

                }
                if (checklist.amountDisbursed <= 0)
                {
                    throw new ApplicationException("Amount to be disbursed cannot be 0 or less");
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

            return "Batch Group Loan Disbursement successfully received.";
        }


        #region OTHER HELPERS
        public string GetBranchNameForClient(int clientId)
        {
            try
            {
                var client = le.clients.SingleOrDefault(e => e.clientID == clientId);
                string branchName = le?.branches?.FirstOrDefault(e => e.branchID == client.branchID)?.branchName;
                TextInfo thisTxtInfo = new CultureInfo("en-us", false).TextInfo;
                if (!string.IsNullOrWhiteSpace(branchName))
                    branchName = thisTxtInfo.ToTitleCase(branchName);
                return branchName;

            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private bool IsOwnerOrAdmin(string userName)
        {
            try
            {
                var secEnt = new coreSecurityEntities();
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => (p.roles.role_name.Trim().ToLower() == "owner" || p.roles.role_name.Trim().ToLower() == "admin") 
                    && p.users.user_name.Trim().ToLower() == userName.ToLower()).ToList();
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

        #endregion OHTER HELPERS



    }
}