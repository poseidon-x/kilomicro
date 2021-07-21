using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Text;
using coreData.Constants;
using coreErpApi.Controllers.Models;
using coreErp.Models.Loan;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Configuration;
using coreERP.Models;
using System.Globalization;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    public class LoanGroupBatchRepaymentController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        HelperMethod helper;

        //BatchRepayModel

        private string ErrorToReturn = "";

        public LoanGroupBatchRepaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            helper = new HelperMethod();
        }

        public LoanGroupBatchRepaymentController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        public groupLoanBatchDisbursementModel Get()
        {
            return new groupLoanBatchDisbursementModel
            {
                loans = new List<LoanViewModel>()
            };
        }

        [HttpGet]
        public BatchRepayModel GetModel()
        {
            return new BatchRepayModel
            {
                repayments = new List<BatchRepaymentViewModel>()
            };
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<LoanViewModel> Get(int id)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == id);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            return le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10
                && p.disbursementDate == null)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new LoanViewModel
                {
                    loanId = l.loanID,
                    clientId = c.clientID,
                    clientName = c.surName + " " + c.otherNames,
                    loanNumberWithName = l.loanNo + " - " + c.surName + " " + c.otherNames,
                    amountApproved = l.amountApproved,
                    amountDisbursed = 0.00
                })
                .OrderBy(p => p.loanNumberWithName)
                .ToList();
        }

        // GET: api/
        [HttpPost]
        public IEnumerable<LoanBatchCashRepaymetModel> GetGroupsLoanByDueRepaymByDate(BatchRepayModel input)
        {
            if (input == null) return null;

            var group = le.loanGroups

            .Include(p => p.loanGroupClients)
            .FirstOrDefault(p => p.loanGroupId == input.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var grps = le.repaymentSchedules
                .Include(p => p.loan)
                .Where(p => p.loan.disbursementDate != null && p.loan.loanTypeID == 10 && p.repaymentDate <= input.repaymentDate.Date
                    && groupClients.Contains(p.loan.clientID)
                 && (p.principalBalance > 0 || p.interestBalance > 0))
                .Select(p => new LoanBatchCashRepaymetModel
                {
                    repaymentScheduleId = p.repaymentScheduleID,
                    repaymentDate = p.repaymentDate,
                    loanId = p.loanID,
                    clientFullName = p.loan.client.surName + ", " + p.loan.client.otherNames,
                    loanNumber = p.loan.loanNo,
                    amountDisbursed = p.loan.amountDisbursed,
                    amountDue = p.interestPayment + p.principalPayment,
                    paid = false,
                    clientId = p.loan.clientID
                })
                .ToList()
                .GroupBy(p => new
                {
                    p.loanNumber,
                    p.loanId,
                    p.clientFullName,
                    p.amountDisbursed,
                    p.paid
                })
                .Select(p => new LoanBatchCashRepaymetModel
                {
                    repaymentScheduleId = p.Max(r => r.repaymentScheduleId),
                    repaymentDate = p.Min(r => r.repaymentDate),
                    loanId = p.Key.loanId,
                    loanNumber = p.Key.loanNumber,
                    clientFullName = p.Key.clientFullName,
                    amountDisbursed = p.Key.amountDisbursed,
                    paid = p.Key.paid,
                    amountDue = p.Sum(r => r.amountDue),
                    dueSchedule = p.Max(r => r.amountDue),
                    clientId = p.Max(r => r.clientId),


                })
                .OrderByDescending(p => p.repaymentDate)
                .ThenBy(p => p.clientFullName)
                .ToList();
            foreach (var item in grps)
            {
                var lg = le.loanGroupClients.FirstOrDefault(p => p.clientId == item.clientId);
                if (lg != null)
                {
                    item.groupAddedDate = lg.created;
                    var cg = le.loanGroups.FirstOrDefault(p => p.loanGroupId == lg.loanGroupId);
                    if (cg != null)
                    {
                        item.groupName = cg.loanGroupName;
                    }
                }
                item.Branch=helper.GetBranchNameForClient(item.clientId);
            }
            return grps.OrderBy(p => p.Branch).ThenBy(r => r.clientFullName).ThenBy(r => r.groupName);
        }

        //Get all Group loans by Group Days for repayment
        [HttpPost]
        public IEnumerable<LoanBatchCashRepaymetModel> GetGroupsLoanByDueRepaymForDay(BatchGroupDayRepayModel input)
        {
            if (input == null) return null;
            var staff = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());


            var days = le.loanGroupClients
                .Include(p => p.loanGroup)
                .Where(p => p.loanGroup.loanGroupDayId == input.dayId);

            if (days == null) throw new ApplicationException("Day Selected doesn't exist");
            var daysClients = days
                //.Where(p=>p.loanGroup.staff.userName==staff.userName)
                .Select(p => p.clientId).ToList();

            var grps = le.repaymentSchedules
                .Include(p => p.loan)
                .Where(p => p.loan.disbursementDate != null && p.loan.loanTypeID == 10 && p.repaymentDate <= input.repaymentDate.Date
                    && daysClients.Contains(p.loan.clientID)
                 && (p.principalBalance > 0 || p.interestBalance > 0))
                .Select(p => new LoanBatchCashRepaymetModel
                {
                    repaymentScheduleId = p.repaymentScheduleID,
                    repaymentDate = p.repaymentDate,
                    loanId = p.loanID,
                    clientFullName = p.loan.client.surName + ", " + p.loan.client.otherNames,
                    loanNumber = p.loan.loanNo,
                    amountDisbursed = p.loan.amountDisbursed,
                    amountDue = p.interestPayment + p.principalPayment,
                    paid = false,
                    clientId = p.loan.clientID
                })
                .ToList()
                .GroupBy(p => new
                {
                    p.loanNumber,
                    p.loanId,
                    p.clientFullName,
                    p.amountDisbursed,
                    p.paid
                })
                .Select(p => new LoanBatchCashRepaymetModel
                {
                    repaymentScheduleId = p.Max(r => r.repaymentScheduleId),
                    repaymentDate = p.Min(r => r.repaymentDate),
                    loanId = p.Key.loanId,
                    loanNumber = p.Key.loanNumber,
                    clientFullName = p.Key.clientFullName,
                    amountDisbursed = p.Key.amountDisbursed,
                    paid = p.Key.paid,
                    amountDue = p.Sum(r => r.amountDue),
                    dueSchedule = p.Max(r => r.amountDue),
                    clientId = p.Max(r => r.clientId)
                })
                .OrderByDescending(p => p.repaymentDate)
                .ThenBy(p => p.clientFullName)
                .ToList();
            foreach (var item in grps)
            {
                var lg = le.loanGroupClients.FirstOrDefault(p => p.clientId == item.clientId);
                if (lg != null)
                {
                    item.groupAddedDate = lg.created;
                    var cg = le.loanGroups.FirstOrDefault(p => p.loanGroupId == lg.loanGroupId);
                    if (cg != null)
                    {
                        item.groupName = cg.loanGroupName;
                    }
                }
            }
            return grps.OrderBy(p => p.groupName).ThenBy(r => r.clientFullName).ThenBy(r => r.groupAddedDate);
        }

        [HttpPost]
        public IEnumerable<LoanBatchCheckListModel> GetGroupsLoanPendingCheckList(BatchCheckListVM input)
        {
            if (input == null) return null;

            var group = le.loanGroups

            .Include(p => p.loanGroupClients)
            .FirstOrDefault(p => p.loanGroupId == input.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var grps = le.loans
                .Where(p => p.loanStatusID == 1 && p.loanTypeID == 10 && p.applicationDate <= input.approvalDate.Date
                    && groupClients.Contains(p.clientID))
                .Select(p => new LoanBatchCheckListModel
                {

                    approvalDate = p.applicationDate,
                    loanId = p.loanID,
                    clientFullName = p.client.surName + ", " + p.client.otherNames,
                    loanNumber = p.loanNo,
                    amountRequested = p.amountRequested,
                    amountApproved = p.amountRequested,
                    approved = false,
                    clientId = p.clientID
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
                .Select(p => new LoanBatchCheckListModel
                {

                    approvalDate = p.Min(r => r.approvalDate),
                    loanId = p.Key.loanId,
                    loanNumber = p.Key.loanNumber,
                    clientFullName = p.Key.clientFullName,
                    amountRequested = p.Key.amountRequested,
                    approved = p.Key.approved,
                    amountApproved = p.Sum(r => r.amountApproved),
                    clientId = p.Max(r => r.clientId),


                })
                .OrderByDescending(p => p.approvalDate)
                .ThenBy(p => p.clientFullName)
                .ToList();
            var genericCheckListItems = le.genericCheckLists.ToList();
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



            }
            return grps.OrderBy(p => p.clientFullName).ThenBy(r => r.groupName).ThenBy(r => r.groupAddedDate);
        }
        [HttpPost]
        public IEnumerable<LoanBatchCheckListModel> GetGroupsLoanPendingApproval(BatchCheckListVM input)
        {
            if (input == null) return null;

            var group = le.loanGroups

            .Include(p => p.loanGroupClients)
            .FirstOrDefault(p => p.loanGroupId == input.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var grps = le.loans
                .Where(p => p.loanStatusID == 2 && p.loanTypeID == 10 && p.applicationDate <= input.approvalDate.Date
                    && groupClients.Contains(p.clientID))
                .Select(p => new LoanBatchCheckListModel
                {

                    approvalDate = p.applicationDate,
                    loanId = p.loanID,
                    clientFullName = p.client.surName + ", " + p.client.otherNames,
                    loanNumber = p.loanNo,
                    amountRequested = p.amountRequested,
                    amountApproved = p.amountRequested,
                    approved = false,
                    clientId = p.clientID
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
                .Select(p => new LoanBatchCheckListModel
                {

                    approvalDate = p.Min(r => r.approvalDate),
                    loanId = p.Key.loanId,
                    loanNumber = p.Key.loanNumber,
                    clientFullName = p.Key.clientFullName,
                    amountRequested = p.Key.amountRequested,
                    approved = p.Key.approved,
                    amountApproved = p.Sum(r => r.amountApproved),
                    clientId = p.Max(r => r.clientId)


                })
                .OrderByDescending(p => p.approvalDate)
                .ThenBy(p => p.clientFullName)
                .ToList();
            foreach (var item in grps)
            {
                var lg = le.loanGroupClients.FirstOrDefault(p => p.clientId == item.clientId);
                if (lg != null)
                {
                    item.groupAddedDate = lg.created;
                    var cg = le.loanGroups.FirstOrDefault(p => p.loanGroupId == lg.loanGroupId);
                    if (cg != null)
                    {
                        item.groupName = cg.loanGroupName;
                    }
                }
            }
            return grps.OrderBy(p => p.clientFullName).ThenBy(r => r.groupName).ThenBy(r => r.groupAddedDate);
        }

        [AuthorizationFilter()]
        [HttpPost]
        public IEnumerable<CombinedLoanBatchDisburseCheckListModel> GetBatchCombinedDisburseChecklist(CombinedBatchDisburseChecklistVM input)
        {
            if (input == null) return null;

            var group = le.loanGroups
            .Include(p => p.loanGroupClients)
            .FirstOrDefault(p => p.loanGroupId == input.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var grps = le.loans.Include(c => c.client)
                .Where(p => p.loanStatusID == 1 && p.loanTypeID == 10
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
            }
            return grps.OrderBy(p => p.clientFullName).ThenBy(r => r.groupName).ThenBy(r => r.groupAddedDate);
        }

        // GET: api/
        [HttpPost]
        public IEnumerable<NewSavingsVM> GetSavings(BatchRepayModel input)
        {
            if (input == null) return null;

            var group = le.loanGroups
                 .Include(p => p.client)
                 .Include(p => p.loanGroupClients)
                 .FirstOrDefault(p => p.loanGroupId == input.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var savings = le.savings
                .Include(p => p.client)
               .Where(p => groupClients.Contains(p.clientID))
               .Join(le.clients, p => p.clientID, p => p.clientID, (p, l) => new NewSavingsVM
               {
                   savingID = p.savingID,
                   savingNo = p.savingNo,
                   principalBalance = p.principalBalance,
                   clientID = p.clientID,
                   savingPlanAmount = p.savingPlanAmount,
                   interestBalance = p.interestBalance,
                   clientName = l.surName + " " + l.otherNames,
                   clientAccNum = l.accountNumber,
                   branchID=l.branchID
               })
               .OrderBy(C => C.clientName)
               .ToList();
            foreach (var save in savings)
            {
                //Get branch for client
                save.Branch = GetBranchNameForClient(save.clientID);
            }

            savings= savings.OrderBy(p=>p.Branch).ToList();
            return savings;
        }

        [HttpPost]
        public IEnumerable<BatchRepaymentViewModel> GetLoanForCombind(BatchRepayModel repayM)
        {
            //var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());

            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == repayM.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var data = le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10
                && p.disbursementDate != null)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new BatchRepaymentViewModel
                {
                    loanId = l.loanID,
                    loanNo = l.loanNo,
                    clientID = l.clientID,
                    clientAccNum = c.accountNumber,
                    clientName = c.surName + " " + c.otherNames,
                    loanNumberWithClient = c.surName + " " + c.otherNames + ", " + l.loanNo,
                    amountDisbursed = l.amountDisbursed,
                    paymentModeId = 1,
                    paymentTypeId = 1,
                    branchID=c.branchID
                })
                .OrderBy(p => p.clientName)
                .ThenBy(p => p.loanNumberWithClient)
                .ToList();

            List<BatchRepaymentViewModel> dataToReturn = new List<BatchRepaymentViewModel>();

            foreach (var record in data)
            {
                var schd = le.repaymentSchedules.FirstOrDefault(p => p.loanID == record.loanId
                && p.interestBalance > 0 && p.principalBalance > 0);
                if (schd != null)
                {
                    record.amountDue = schd.principalPayment + schd.interestPayment;
                    record.paymentAmount= schd.principalPayment + schd.interestPayment;
                    dataToReturn.Add(record);
                }

            }
            return dataToReturn;
        }
        [AuthorizationFilter()]
        [HttpPost]
        public IEnumerable<CombinedPaymentVM> GetCombined(BatchRepayModel input)
        {
            if (input == null) return null;
            var savings = GetSavings(input);
            var loans = GetLoanForCombind(input);
            List<CombinedPaymentVM> combined = new List<CombinedPaymentVM>();
            foreach (var sav in savings)
            {
                var existingClient = combined.FirstOrDefault(c => c.clientID == sav.clientID);
                if (existingClient != null && existingClient.savingID == 0)
                {
                    existingClient.savingID = sav.savingID;
                    existingClient.savingNo = sav.savingNo;
                    existingClient.savingPlanAmount = sav.savingPlanAmount;
                    existingClient.savingsPaid = sav.paid;
                    existingClient.principalSavingsBalance = sav.principalBalance;
                    existingClient.interestSavingsBalance = sav.interestBalance;
                    existingClient.BranchID = sav.branchID;
                }
                else
                {
                    CombinedPaymentVM comb = new CombinedPaymentVM();
                    comb.savingID = sav.savingID;
                    comb.savingNo = sav.savingNo;
                    comb.savingPlanAmount = sav.savingPlanAmount;
                    comb.savingsPaid = sav.paid;
                    comb.principalSavingsBalance = sav.principalBalance;
                    comb.interestSavingsBalance = sav.interestBalance;
                    comb.clientID = sav.clientID;
                    comb.clientName = sav.clientName;
                    comb.clientAccNum = sav.clientAccNum;
                    comb.BranchID = sav.branchID;
                    combined.Add(comb);
                }
            }

            foreach (var ln in loans)
            {
                var existingClient = combined
                    .FirstOrDefault(c => c.clientID == ln.clientID);
                if (existingClient != null && existingClient.loanId == 0)
                {
                    existingClient.loanId = ln.loanId;
                    existingClient.loanNo = ln.loanNo;
                    existingClient.loanNumberWithClient = ln.loanNumberWithClient;
                    existingClient.paymentAmount = ln.paymentAmount;
                    existingClient.paymentModeId = ln.paymentModeId;
                    existingClient.paymentTypeId = ln.paymentTypeId;
                    existingClient.principalAmount = ln.principalAmount;
                    existingClient.principalLoanBalance = ln.principalBalance;
                    existingClient.repaymentScheduleId = ln.repaymentScheduleId;
                    existingClient.amountDisbursed = ln.amountDisbursed;
                    existingClient.amountDue = ln.amountDue;
                    existingClient.bankId = ln.bankId;
                    existingClient.cashCollateral = ln.cashCollateral;
                    existingClient.chequeNumber = ln.chequeNumber;
                    existingClient.BranchID = ln.branchID;

                }
                else
                {
                    CombinedPaymentVM comb = new CombinedPaymentVM();
                    comb.loanId = ln.loanId;
                    comb.loanNo = ln.loanNo;
                    comb.loanNumberWithClient = ln.loanNumberWithClient;
                    comb.paymentAmount = ln.paymentAmount;
                    comb.paymentModeId = ln.paymentModeId;
                    comb.paymentTypeId = ln.paymentTypeId;
                    comb.principalAmount = ln.principalAmount;
                    comb.principalLoanBalance = ln.principalBalance;
                    comb.repaymentScheduleId = ln.repaymentScheduleId;
                    comb.amountDisbursed = ln.amountDisbursed;
                    comb.amountDue = ln.amountDue;
                    comb.bankId = ln.bankId;
                    comb.cashCollateral = ln.cashCollateral;
                    comb.chequeNumber = ln.chequeNumber;
                    comb.BranchID = ln.branchID;
                    comb.clientID = ln.clientID;
                    comb.clientName = ln.clientName;
                    comb.clientAccNum = ln.clientAccNum;
                    combined.Add(comb);
                }

                
            }
            //Check for login user
            var user = User?.Identity?.Name?.Trim()?.ToLower();
            if (!helper.IsOwner(user))
            {
                var staffBranchId = helper.GetBranchIdForUser(user);
                combined = combined.Where(p => p.BranchID == staffBranchId).ToList();
            }

            foreach (var item in combined)
            {
                item.Branch = helper.GetBranchNameForClient(item.clientID);
            }
            return combined;
        }





        private string attemptReservation(int savingsId, double savingsPlanAmount, DateTime paymentDate, string currentUserName)
        {
            using (var con = new SqlConnection(ConfigurationManager.AppSettings["RAW_CON_STR"]))
            {
                SqlCommand cmd = new SqlCommand("[ln].[sp_attempt_reservation]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@savingId", savingsId);
                cmd.Parameters.Add("@amount", savingsPlanAmount);
                cmd.Parameters.Add("@reservationTypeId", 2);
                cmd.Parameters.Add("@reservedBy", currentUserName);
                cmd.Parameters.Add("@naration", savingsId);

                var tranParam = cmd.Parameters.Add("@transactionId", System.Data.SqlDbType.NVarChar, 100);
                tranParam.Direction = System.Data.ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                return tranParam.Value.ToString();


            }

        }
        private string reserveAndDeposit(int savingsId, double savingsPlanAmount, DateTime paymentDate)
        {
            try
            {
                using (var ctx = new coreLoansEntities())
                {
                    using (var ent = new coreSecurityEntities())
                    {
                        var currentUserName = LoginHelper.getCurrentUser(ent);
                        if (currentUserName == null)
                        {
                            throw new ArgumentException("No User Identified");
                        }



                        var tranParam = new System.Data.Entity.Core.Objects.ObjectParameter("transactionId", typeof(string));
                        var depositParam = new System.Data.Entity.Core.Objects.ObjectParameter("savingAdditionalId", typeof(int));

                        //var reserveResults = ctx.sp_attempt_reservation(itemToBeSaved.savingID, itemToBeSaved.savingPlanAmount, 2,
                        //    currentUserName, "DEPOSIT ON " + paymentDate.ToString("DD / MMM / YYYY"), tranParam

                        var transactionId = attemptReservation(savingsId, savingsPlanAmount, paymentDate, currentUserName);
                        var depositResults = ctx.sp_attempt_deposit(savingsId, savingsPlanAmount, currentUserName,
                            paymentDate, null, "", 1, "DEPOSIT ON " + paymentDate.ToString("DD / MMM / YYYY"),
                            transactionId, depositParam);
                    }
                }
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "";
        }

        [AuthorizationFilter()]
        [HttpPost]
        public UpdateSavingsResultVM PostNewSavings(UpdateSavingsVM save)
        {

            using (var ent = new coreSecurityEntities())
            {
                var currentUserName = LoginHelper.getCurrentUser(ent);
                if (currentUserName == null)
                {
                    throw new ArgumentException("No User Identified");
                }

                var ct = le.cashiersTills.FirstOrDefault(p => p.userName == currentUserName);
                if (ct == null)
                {
                    throw new ArgumentException("There is no till defined for the currently logged in user (" + currentUserName + ")", "coreERP©: Failed");
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == save.paymentDate.Date
                    && p.open == true);
                if (ctd == null)
                {
                    throw new ArgumentException("The till for the selected date has not been opened for this user (" + currentUserName + ")", "coreERP©: Failed");
                }
                save.paymentDate = save.paymentDate.Date;
                UpdateSavingsResultVM results = new UpdateSavingsResultVM
                {
                    successfulSavings = new List<SavingResultsVM>(),
                    failedSavings = new List<SavingResultsVM>()
                };
                foreach (var input in save.savings)
                {
                    if (input.paid)
                    {
                        PostSingleSavingsRecord(save, results, input);
                    }

                }
                return results;
            }
        }

        private void PostSingleSavingsRecord(UpdateSavingsVM save, UpdateSavingsResultVM results, NewSavingsVM input)
        {
            //save.paymentDate = save.paymentDate.Date;
            var resultMsg = reserveAndDeposit(input.savingID, input.savingPlanAmount, save.paymentDate);
            if (string.IsNullOrEmpty(resultMsg))
            {
                results.successfulSavings.Add(new SavingResultsVM(input));
            }
            else
            {
                var failedItems = new SavingResultsVM(input);
                failedItems.failureReason = resultMsg;
                results.failedSavings.Add(failedItems);
            }
        }

        private void PostSingleSavingsRecordCombined(UpdateCombinedSavingsVM save, UpdateCombinedResultVM results, CombinedPaymentVM input)
        {
            save.paymentDate = save.paymentDate.Date;
            var resultMsg = reserveAndDeposit(input.savingID, input.savingPlanAmount, save.paymentDate);
            if (string.IsNullOrEmpty(resultMsg))
            {
                results.successfulSavings.Add(new CombinedResultsVM(input));
            }
            else
            {
                var failedItems = new CombinedResultsVM(input);
                failedItems.failureReason = resultMsg;
                results.failedSavings.Add(failedItems);
            }
        }

        private void PostSingleLoanRecordCombined(UpdateCombinedSavingsVM save, UpdateCombinedResultVM results, CombinedPaymentVM input)
        {
            save.paymentDate = save.paymentDate.Date;
            var resultMsg = reserveAndDeposit(input.loanId, input.paymentAmount, save.paymentDate);
            if (string.IsNullOrEmpty(resultMsg))
            {
                results.successfulSavings.Add(new CombinedResultsVM(input));
            }
            else
            {
                var failedItems = new CombinedResultsVM(input);
                failedItems.failureReason = resultMsg;
                results.failedSavings.Add(failedItems);
            }
        }

        [AuthorizationFilter()]
        [HttpPost]
        public UpdateCombinedResultVM PostBatchCombined(UpdateCombinedSavingsVM save)
        {
            using (var ent = new coreSecurityEntities())
            {
                var loginUser = LoginHelper.getCurrentUser(ent);

                var currentUserName = LoginHelper.getCurrentUser(ent);
                if (currentUserName == null)
                {
                    throw new ArgumentException("No User Identified");
                }

                var ct = le.cashiersTills.FirstOrDefault(p => p.userName == currentUserName);
                if (ct == null)
                {
                    throw new ArgumentException("There is no till defined for the currently logged in user (" + currentUserName + ")", "coreERP©: Failed");
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == save.paymentDate.Date
                    && p.open == true);
                if (ctd == null)
                {
                    throw new ArgumentException("The till for the selected date has not been opened for this user (" + currentUserName + ")", "coreERP©: Failed");
                }
                UpdateCombinedResultVM results = new UpdateCombinedResultVM
                {
                    successfulSavings = new List<CombinedResultsVM>(),
                    failedSavings = new List<CombinedResultsVM>(),
                    paymentDate = save.paymentDate.Date
                };
                foreach (var input in save.combinedItems)
                {
                    if (input.savingsPaid && input.savingPlanAmount > 0 && input.savingID > 0)
                    {
                        PostSingleSavingsRecordCombined(save, results, input);
                    }
                    if (input.paymentAmount > 0 && input.loanId > 0 && input.savingsPaid)
                    {
                        PostSingleLoan(loginUser, input.paymentAmount, input.bankId, input.chequeNumber,
                    input.clientID, input.loanId, input.paymentModeId, save.paymentDate, ct.cashiersTillID,
                    input.paymentTypeId, input.cashCollateral, input.repaymentScheduleId);
                        if (!results.successfulSavings.Any(c => c.clientID == input.clientID))
                        {
                            results.successfulSavings.Add(new CombinedResultsVM(input));
                        }
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

                return results;
            }
        }

        //Get Group Loans by group Id
        [HttpPost]
        public IEnumerable<BatchRepaymentViewModel> Get(BatchRepayModel repayM)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == repayM.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var data = le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10
                && p.disbursementDate != null)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new BatchRepaymentViewModel
                {
                    loanId = l.loanID,
                    clientID = l.clientID,
                    clientAccNum = c.accountNumber,
                    clientName = c.surName + " " + c.otherNames,
                    loanNumberWithClient = c.surName + " " + c.otherNames + ", " + l.loanNo,
                    amountDisbursed = l.amountDisbursed,
                    paymentModeId = 1,
                    paymentTypeId = 1
                })
                .OrderByDescending(p => p.clientName)
                .ThenBy(p => p.loanNumberWithClient)
                .ToList();

            List<BatchRepaymentViewModel> dataToReturn = new List<BatchRepaymentViewModel>();

            foreach (var record in data)
            {
                var schd = le.repaymentSchedules.FirstOrDefault(p => p.loanID == record.loanId
                && p.interestBalance > 0 && p.principalBalance > 0);
                if (schd != null)
                {
                    record.amountDue = schd.principalPayment + schd.interestPayment;
                    dataToReturn.Add(record);
                }

            }
            return dataToReturn;
        }

        [AuthorizationFilter()]
        [HttpPost]
        public bool PostBashCashPayment(BatchCashPaymentVM input)
        {
            if (input.repaymentItemsList.Any())
            {
                var batch = new BatchRepayModel { repaymentDate = input.repaymentDate, repayments = new List<BatchRepaymentViewModel>() };
                foreach (var rep in input.repaymentItemsList.Where(p => p.paid))
                {
                    batch.repayments.Add(new BatchRepaymentViewModel
                    {
                        loanId = rep.loanId,
                        repaymentScheduleId = rep.repaymentScheduleId,
                        paymentAmount = rep.dueSchedule,
                        paymentModeId = 1,
                        paymentTypeId = 1,
                        clientID = rep.clientId
                    });
                }
                PayBatch(batch);
                return true;
            }
            else
            {
                throw new ApplicationException("No data to post");
            }
            return false;

        }

        [AuthorizationFilter()]
        [HttpPost]
        public bool PostBatchChecklist(BatchCheckListVM input)
        {
            if (input.approvalItemsList.Any())
            {
                var batch = new BatchCheckListVM { approvalDate = input.approvalDate, approvalItemsList = new List<LoanBatchCheckListModel>() };
                foreach (var rep in input.approvalItemsList.Where(p => p.approved))
                {
                    batch.approvalItemsList.Add(new LoanBatchCheckListModel
                    {
                        loanId = rep.loanId,
                        amountApproved = rep.amountApproved
                    });
                }
                PostBatchChecklistInner(batch);
                return true;
            }
            else
            {
                throw new ApplicationException("No data to post");
            }
            return false;

        }
        [AuthorizationFilter()]
        [HttpPost]
        public bool PostBatchApproval(BatchCheckListVM input)
        {
            if (input.approvalItemsList.Any())
            {
                var batch = new BatchCheckListVM { approvalDate = input.approvalDate, approvalItemsList = new List<LoanBatchCheckListModel>() };
                foreach (var rep in input.approvalItemsList.Where(p => p.approved))
                {
                    batch.approvalItemsList.Add(new LoanBatchCheckListModel
                    {
                        loanId = rep.loanId,
                        amountApproved = rep.amountApproved
                    });
                }
                PostBatchApprovalInner(batch);
                return true;
            }
            else
            {
                throw new ApplicationException("No data to post");
            }
            return false;

        }


        [AuthorizationFilter()]
        [HttpPost]
        public string PayBatch(BatchRepayModel batchRepayments)
        {
            if (batchRepayments == null) return null;

            StringBuilder lns = new StringBuilder();
            foreach (var repay in batchRepayments.repayments)
            {
                //amount paid is less than one(1)
                if (repay.paymentAmount < 1)
                    lns.Append(repay.loanNumberWithClient + "has repayment amount is less than one(1) <br/>");
            }
            if (lns.ToString() != "")
            {
                throw new ApplicationException(lns.ToString());
            }
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var currentUser =
                (new coreSecurityEntities()).users
                .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == currentUser.user_name.ToLower());

            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                                currentUser.full_name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == batchRepayments.repaymentDate.Date
                && p.open);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" +
                                                currentUser.full_name + ")");
            }

            foreach (var repay in batchRepayments.repayments)
            {
                PostSingleLoan(loginUser, repay.paymentAmount, repay.bankId, repay.chequeNumber,
                    repay.clientID, repay.loanId, repay.paymentModeId, batchRepayments.repaymentDate, ct.cashiersTillID,
                    repay.paymentTypeId, repay.cashCollateral, repay.repaymentScheduleId);
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw x;
            }

            return "Batch disbursement has been received successfully";
        }

        private void PostSingleLoan(string loginUser,
            double paymentAmount, int? bankId, string chequeNo, int clientId, int loanId, int paymentModeId, DateTime repaymentDate,
            int cashierTillId, int repaymentTypeId, double cashCollateral, int repaymentScheduleId)
        {
            var currentLoan = le.loans.FirstOrDefault(p => p.loanID == loanId);
            var currentSchedule =
                le.repaymentSchedules.FirstOrDefault(p => p.repaymentScheduleID == repaymentScheduleId);
            var cond = currentSchedule != null;
            if (currentLoan != null)
            {
                var crtp = new cashierReceipt
                {
                    amount = paymentAmount,
                    bankID = bankId,
                    checkNo = chequeNo,
                    clientID = clientId,
                    loanID = loanId,
                    paymentModeID = paymentModeId,
                    posted = false,
                    txDate = repaymentDate.Date,
                    cashierTillID = cashierTillId,
                    repaymentTypeID = repaymentTypeId
                };
                //check payment Type apply the amount paid
                if (repaymentTypeId == 2) crtp.principalAmount = paymentAmount;
                else if (repaymentTypeId == 3) crtp.interestAmount = paymentAmount;
                else if (repaymentTypeId == 6) crtp.feeAmount = paymentAmount;
                else if (repaymentTypeId == 7) crtp.addInterestAmount = paymentAmount;
                le.cashierReceipts.Add(crtp);

                var sav = le.savings.FirstOrDefault(p => p.clientID == currentLoan.clientID);
                using (var db = new core_dbEntities())
                {
                    if (sav != null && db.comp_prof.First().comp_name.ToLower().Contains("ttl"))
                    {
                        if (sav.savingID > 0)
                        {
                            sav.amountInvested += cashCollateral;
                            var da = new savingAdditional
                            {
                                checkNo = chequeNo,
                                savingAmount = cashCollateral,
                                naration = "Cash Collateral",
                                bankID = bankId,
                                fxRate = 0,
                                localAmount = cashCollateral,
                                interestBalance = 0,
                                savingDate = repaymentDate.Date,
                                creation_date = DateTime.Now,
                                creator = loginUser,
                                principalBalance = cashCollateral,
                                modeOfPaymentID = paymentModeId,
                                posted = false,
                                closed = false
                            };
                            sav.principalBalance += cashCollateral;
                            sav.availablePrincipalBalance += cashCollateral;
                            sav.savingAdditionals.Add(da);
                            sav.modification_date = DateTime.Now;
                            sav.last_modifier = User.Identity.Name;
                        }
                    }
                }



            }
        }

        private string PostBatchChecklistInner(BatchCheckListVM batchApprovals)
        {
            if (batchApprovals == null) return null;
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var currentUser =
                (new coreSecurityEntities()).users
                .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());

            foreach (var checklist in batchApprovals.approvalItemsList)
            {

                var currentLoan = le.loans.FirstOrDefault(p => p.loanID == checklist.loanId);
                currentLoan.modification_date = DateTime.Now;
                currentLoan.last_modifier = currentUser.user_name;
                currentLoan.loanStatusID = 2;//mark as approved status 3
                currentLoan.checkedBy = currentUser.user_name;


            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw x;
            }

            return "Batch disbursement has been received successfully";
        }
        private string PostBatchApprovalInner(BatchCheckListVM batchApprovals)
        {
            if (batchApprovals == null) return null;
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var currentUser =
                (new coreLogic.coreSecurityEntities()).users
                .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());

            foreach (var checklist in batchApprovals.approvalItemsList)
            {

                var currentLoan = le.loans.FirstOrDefault(p => p.loanID == checklist.loanId);
                currentLoan.modification_date = DateTime.Now;
                currentLoan.last_modifier = currentUser.user_name;
                currentLoan.loanStatusID = 3;
                currentLoan.finalApprovalDate = batchApprovals.approvalDate;
                currentLoan.approvedBy = currentUser.user_name;
                currentLoan.amountApproved = checklist.amountApproved;
                currentLoan.approvalComments = $"Batch Approval by {currentUser.full_name} on {DateTime.Now:dd-MMM-yyyy}";


            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw x;
            }

            return "Batch disbursement has been received successfully";
        }

        [AuthorizationFilter()]
        [HttpPost]
        public bool PostBatchCombinedDisburseChecklist(CombinedBatchDisburseChecklistVM input)
        {

            if (input.approvalItemsList.Any())
            {
                var batch = new CombinedBatchDisburseChecklistVM
                {
                    groupId = input.groupId,
                    approvalDate = input.approvalDate,
                    disbursementDate = DateTime.Now,
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
                        approvalDate = rep.approvalDate,
                    });

                }
                PostBatchDisburseChecklistInner(batch);
                return true;
            }
            else
            {
                throw new ApplicationException("No data to post");
            }
        }

        private string PostBatchDisburseChecklistInner(CombinedBatchDisburseChecklistVM batchApprovals)
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
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var user =
                (new coreLogic.coreSecurityEntities()).users
                .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());


            if (user.accessLevel.disbursementLimit < totalBatchAmount)
            {
                throw new ApplicationException("The amount to be disbursed is beyond your access level");
            }
            var ctl =
                le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == user.user_name.ToLower().Trim());
            if (ctl == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                               user.full_name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ctl.cashiersTillID && p.tillDay == batchApprovals.approvalDate.Date
                && p.open);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" +
                                               user.full_name + ")");
            }

            foreach (var checklist in batchApprovals.approvalItemsList)
            {
                var currentLoan = le.loans
                    .FirstOrDefault(p => p.loanID == checklist.loanId);
                if (currentLoan != null)
                {
                    currentLoan.checkedBy = user.user_name;
                    currentLoan.amountDisbursed = checklist.amountDisbursed;
                    currentLoan.modification_date = DateTime.Now;
                    currentLoan.last_modifier = user.user_name;
                    currentLoan.loanStatusID = 3;
                    currentLoan.disbursedBy = user.user_name;
                    currentLoan.amountApproved = checklist.amountApproved;
                    currentLoan.finalApprovalDate = checklist.approvalDate.Date;
                    currentLoan.disbursementDate = checklist.approvalDate.Date;
                    currentLoan.approvedBy = user.user_name;
                    currentLoan.approvalComments = $"Batch Checklist and Disbursement by {user.full_name} on {DateTime.Now:dd-MMM-yyyy}";

                    var cd = new cashierDisbursement();
                    cd.cashierTillID = ctl.cashiersTillID;
                    cd.txDate = DateTime.Now.Date;
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
                if (currentLoan.amountDisbursed <= 0)
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

            return "Batch checklist and disbursement successfully received.";
        }


        #region OTHER HELPERS

        public string GetBranchNameForClient(int clientId)
        {
            try
            {
                var client = le.clients.SingleOrDefault(e => e.clientID == clientId);
                string branchName = le.branches.FirstOrDefault(e => e.branchID == client.branchID).branchName;
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

        #endregion OTHER HELPERS


    }
}
