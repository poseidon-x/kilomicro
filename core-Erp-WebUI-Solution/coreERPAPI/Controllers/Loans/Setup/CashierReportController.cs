using coreERP.Models;
using coreERP.Models.Loan;
using coreERP.Models.Reports;
using coreERP.Providers;
using coreErpApi;
using coreErpApi.Controllers.Models;
using coreLogic;
using coreReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class CashierReportController : ApiController
    {
        IcoreLoansEntities le;
        IcoreSecurityEntities ctx;
        IreportEntities rent;
        HelperMethod helper;
        public CashierReportController()
        {
            le = new coreLoansEntities();
            ctx = new coreSecurityEntities();
            rent = new reportEntities();
            helper = new HelperMethod();
        }



        //GET ALL THE CASHIERS
        [HttpGet]
        public IEnumerable<CashierUserViewModel> GetAllCashiers()
        {
            var cashierTills = le.cashiersTills.ToList();
            List<CashierUserViewModel> cashiers = new List<CashierUserViewModel>();
            var loginUser = User?.Identity?.Name?.Trim()?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginUser);
            var isOwner = helper.IsOwner(loginUser);
            foreach (var till in cashierTills)
            {
                var secTillUser = ctx.users.FirstOrDefault(p => p.user_name == till.userName);
                if (secTillUser != null)
                {
                    CashierUserViewModel cashierUser = new CashierUserViewModel
                    {
                        cashierUserName = secTillUser.user_name,
                        cashierFullName = secTillUser.full_name
                    };
                    cashiers.Add(cashierUser);
                }
            }
            if (!isOwner && !isBranchAdmin)
            {
                cashiers = cashiers.Where(p => p.cashierUserName.Trim().ToLower() == loginUser).ToList();
            }
            var orderedCashiers = cashiers.OrderBy(p => p.cashierFullName).ToList();
            return orderedCashiers;
        }


        //GET ALL THE FIELD AGENTS
        [HttpGet]
        public IEnumerable<AgentViewModel> GetAllAgents()
        {
            var agents = le.agents.ToList();
            List<AgentViewModel> fieldAgents = new List<AgentViewModel>();
            foreach (var agent in agents)
            {
                if (agent != null)
                {
                    AgentViewModel agentModel = new AgentViewModel
                    {
                        agentId = agent.agentID,
                        agentNameWithNo = agent.surName + " " + agent.otherNames + " (" + agent.agentNo + " )"
                    };
                    fieldAgents.Add(agentModel);
                }
            }
            var orderedAgents = fieldAgents.OrderBy(p => p.agentNameWithNo).ToList();
            return orderedAgents;
        }


        #region ADMIN CASHIER REPORT

        //GET THE ARREARS 
        #region ARREARS
        //GET ALL ARREARS for Admin/systems owner
        [HttpPost]
        public KendoResponse GetAdminArrears(CashierReportInputModel cashierModel)
        {// get list of entries per pages and with filters

            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();
            var endDate = cashierModel.EndDate;
            var selectedUserName = cashierModel.CashierUsername;
            var isSysOwner = helper.IsOwner(currentUserName);
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUserName);
            if (!isSysOwner && !isBranchAdmin)
            {
                return new KendoResponse(new vwOutstandingLoanNew[0], 0);
            }
            var endOfDayDate = endDate;
            endOfDayDate = endOfDayDate.Date.AddDays(1).AddSeconds(-1);
            reportEntities repEnt = new reportEntities();
            var arrears = repEnt.spGetLoanArrearsWithDays(endOfDayDate)
            .Where(p =>
               ((p.Payable ?? 0) - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0))) > 0).ToList();

            var arrResult = arrears.Select(p => new vwOutstandingLoanNew
            {
                outstanding = (p.Payable ?? 0) - ((p.WriteOffAmount ?? 0) + (p.Paid ?? 0)),
                clientName = p.clientName,
                loanNo = p.loanNo,
                loanGroupName = p.loanGroupName,
                LastRepaymentDate = p.LastRepaymentDate,
                LastDueDate = p.LastDueDate,
                disbursementDate = p.disbursementDate,
                amountDisbursed = p.amountDisbursed.Value,
                Payable = p.Payable.Value,
                Paid = p.Paid.Value,
                WriteOffDate = p.WriteOffDate,
                WriteOffAmount = p.WriteOffAmount.Value,
                loanGroupId = p.loanGroupId,
                clientID = p.clientID,
                DaysDefault=p.daysDue              
            })
             .Where(p => p.outstanding >= 1)
             .OrderBy(p => p.loanGroupName)
             .ToList();

            foreach (var loanOut in arrResult)
            {
                if (loanOut.loanGroupId == null)
                {
                    if (string.IsNullOrWhiteSpace(loanOut.Officer))
                        loanOut.Officer = "No Officer Assigned";
                    if (string.IsNullOrWhiteSpace(loanOut.loanGroupName))
                        loanOut.loanGroupName = "No Group";
                }
                else
                {
                    loanOut.Officer = helper.GetGroupOfficerFullName(loanOut.loanGroupId.Value);
                    loanOut.OfficerUserName = helper.GetGroupOfficerUserName(loanOut.loanGroupId.Value);
                }
                loanOut.BranchName = helper.GetBranchNameForClient(loanOut.clientID);
                loanOut.BranchId = helper.GetBranchIdForClient(loanOut.clientID);
                loanOut.ClientPhone = helper.GetClientPhoneNumber(loanOut.clientID);
            }
            //Check for Branch Admin, to display only his branch's arrears
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUserName);
                arrResult = arrResult.Where(p => p.BranchId == userBranchId).ToList();
            }
            //If a user is selected in the combo box
            if (!string.IsNullOrWhiteSpace(selectedUserName))
            {
                arrResult = arrResult.Where(p => p.OfficerUserName.ToLower().Trim() == selectedUserName.ToLower().Trim()).ToList();
            }
            var query = arrResult.OrderBy(p => p.BranchName).ThenBy(p => p.clientName).AsQueryable();
            var data = query.ToArray();
            return new KendoResponse(data, query.Count());
        }

        #endregion ARREARS


        //GET THE SAVINGS AND OUTSTANDINGS  
        #region OUTSTANDING

        [HttpPost]
        public KendoResponse GetCombinedSavingAndOutstandingLoans(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new CombinedSavingAndOutstandingLoan[0], 0);
            }

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;

            var outstandingLoans = GetOutstandingLoans(endDate, userName);
            var savings = GetSavings(userName, endDate);

            List<CombinedSavingAndOutstandingLoan> combined = new List<CombinedSavingAndOutstandingLoan>();

            foreach (var outstand in outstandingLoans)
            {

                CombinedSavingAndOutstandingLoan comb = new CombinedSavingAndOutstandingLoan();
                comb.AmountDisbursed = outstand.LoanAmount;
                comb.ClientName = outstand.ClientName;
                comb.DisbursementDate = outstand.DisbursementDate;
                comb.DisbursedDateToString = outstand.DisbursementDate.ToShortDateString().ToString();
                comb.LastRepaymentDate = outstand.ExpiryDate;
                comb.LoanGroupName = string.IsNullOrWhiteSpace(outstand.LoanGroupName) ? helper.GetGroupNameForClient(outstand.ClientId) : outstand.LoanGroupName;
                comb.LoanNo = outstand.LoanNo;
                comb.Officer = outstand.Officer;
                comb.Outstanding = outstand.OutstandingAmount;
                comb.Paid = outstand.Paid;
                comb.WriteOffAmount = outstand.WriteOffAmount;
                comb.ClientID = outstand.ClientId;
                combined.Add(comb);

            }
            foreach (var saving in savings)
            {
                var clientWithSaving = combined.FirstOrDefault(c => c.ClientID == saving.ClientId);
                if (clientWithSaving != null && clientWithSaving.SavingId == 0)
                {
                    clientWithSaving.SavingId = saving.SavingId;
                    clientWithSaving.SavingNo = saving.SavingNo;
                    clientWithSaving.AvailableInterestBalance = saving.AvailableInterestBalance;
                    clientWithSaving.AvailablePrincipalBalance = saving.AvailablePrincipalBalance;
                    clientWithSaving.PrincipalBalance = saving.PrincipalBalance;
                    clientWithSaving.InterestBalance = saving.InterestBalance;
                    clientWithSaving.ClientID = saving.ClientId;
                    clientWithSaving.ClientName = saving.ClientName;
                    clientWithSaving.SavingBalance = saving.PrincipalBalance + saving.InterestBalance;
                    clientWithSaving.LoanGroupName = helper.GetGroupNameForClient(saving.ClientId);
                }
                else
                {
                    CombinedSavingAndOutstandingLoan combItem = new CombinedSavingAndOutstandingLoan();
                    combItem.SavingId = saving.SavingId;
                    combItem.SavingNo = saving.SavingNo;
                    combItem.AvailableInterestBalance = saving.AvailableInterestBalance;
                    combItem.AvailablePrincipalBalance = saving.AvailablePrincipalBalance;
                    combItem.PrincipalBalance = saving.PrincipalBalance;
                    combItem.InterestBalance = saving.InterestBalance;
                    combItem.ClientID = saving.ClientId;
                    combItem.ClientName = saving.ClientName;
                    combItem.LoanGroupName = helper.GetGroupNameForClient(saving.ClientId);
                    combItem.SavingBalance = saving.PrincipalBalance + saving.InterestBalance;
                    if (combItem.SavingBalance > 0)
                        combined.Add(combItem);

                }
            }

            //Get the Assigned Officer for each client
            foreach (var clientItem in combined)
            {
                if (clientItem.LoanGroupId == null)
                {
                    var groupId = helper.GetGroupIdForClient(clientItem.ClientID);
                    if (groupId == null)
                        clientItem.Officer = !string.IsNullOrWhiteSpace(clientItem.Officer) ? clientItem.Officer : "No Officer Assigned";
                    else
                        clientItem.Officer = helper.GetGroupOfficerName(groupId.Value);
                }
                else
                {
                    clientItem.Officer = helper.GetGroupOfficerName(clientItem.LoanGroupId.Value);
                }
                //Get the branch for client
                clientItem.Branch = helper.GetBranchNameForClient(clientItem.ClientID);
                clientItem.BranchId = helper.GetBranchIdForClient(clientItem.ClientID);

            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                combined = combined.Where(p => p.BranchId == userBranchId).ToList();
            }
            var query = combined.OrderBy(p => p.Branch).ThenBy(p => p.LoanGroupName).AsQueryable();

            var data = query.ToArray();
            return new KendoResponse(data, query.Count());
        }

        public List<NewSavingModel> GetSavings(string userName, DateTime? endDate)
        {
            endDate = endDate.Value.Date.AddDays(1).AddSeconds(-1);
            var savings = rent.vwSavingStatements
                .Where(p => p.date <= endDate)
               .GroupBy(p => new
               {
                   p.loanNo,
                   p.clientID,
                   p.clientName,
                   p.loanID,
                   p.staffName
               }
               )
              .Select(p => new NewSavingModel
              {
                  ClientId = p.Key.clientID,
                  ClientName = p.Key.clientName,
                  AvailableInterestBalance = p.Sum(e => e.interestAccruedAmount - e.intWithdrawalAmount),
                  AvailablePrincipalBalance = p.Sum(e => e.depositAmount - e.princWithdrawalAmount),
                  PrincipalBalance = p.Sum(e => e.depositAmount - e.princWithdrawalAmount),
                  InterestBalance = p.Sum(e => e.interestAccruedAmount - e.intWithdrawalAmount),
                  SavingId = p.Key.loanID,
                  SavingNo = p.Key.loanNo,
                  StaffId = 0,
                  Creator = p.Key.staffName
              })
              .Where(c => (c.PrincipalBalance + c.InterestBalance) >= 1)
              .OrderBy(c => c.ClientName)
              .ToList();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                var sortedForUser = savings.Where(s => s.Creator.ToLower().Trim() == userName.ToLower().Trim()).ToList();
                return sortedForUser;
            }
            return savings;
        }

        public List<LoanOutstandingModel> GetOutstandingLoans(DateTime endDate, string userName)
        {
            var endOfDayDate = endDate.Date.AddDays(1).AddSeconds(-1);
            reportEntities repEnt = new reportEntities();
            var outStandingResult = repEnt.spOutstanding(endOfDayDate).Where(p =>
                //p.expiryDate <= endOfDayDate &&
                ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)) > 0).ToList();

            var outResult = outStandingResult.Select(p => new LoanOutstandingModel
            {
                ClientId = p.clientID,
                ClientName = p.clientName,
                LoanAmount = p.amountDisbursed,
                LoanGroupName = string.IsNullOrWhiteSpace(p.LoanGroupName) ? "No Group" : p.LoanGroupName,
                LoanId = p.loanID,
                LoanNo = p.loanNo,
                OutstandingAmount = ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)),
                DaysDefault = p.daysDue,
                ExpiryDate = p.expiryDate,
                WriteOffAmount = p.writtenOff,
                Collateral = p.fairValue,
                DisbursementDate = p.disbursementDate,
                Paid = p.totalPaid,
                StaffId = p.StaffID
            })
            .Where(p => p.OutstandingAmount >= 1)
            .OrderBy(p => p.LoanGroupName)
            .ToList();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                //Get Staff Id for selected user
                var selectedUserStaffId = helper.GetStaffIdForUser(userName.ToLower());
                outResult = outResult.Where(p => p.StaffId == selectedUserStaffId).ToList();
            }
            return outResult.ToList();

        }

        #endregion OUTSTANDING


        //GET LOAN REPAYMENT

        #region LOAN REPAYMENT

        [HttpPost]
        public KendoResponse GetLoanRepayments(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new CashierRepayments[0], 0);
            }
            var startDate = cashierModel.StartDate.Date;
            var endDate = cashierModel.EndDate.AddDays(1).AddSeconds(-1);
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            var resRep = rent.vwCashierRepaymentsGroupeds.Where(p =>
                     (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
                     && (userName.Trim() == "" || p.userName == userName) && (fieldAgent.Trim() == "" || p.agentName == fieldAgent)
                     && ((p.repaymentTypeName == "Interest Only" || p.repaymentTypeName == "Principal and Interest"
                         || p.repaymentTypeName == "Principal Only" || p.repaymentTypeName == "Penalty")))
                .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var repayments = resRep.Select(o => new CashierRepayments
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanGroupName = o.loanGroupName,
                loanGroupNumber = o.loanGroupNumber,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName,
                GroupName = o.loanGroupName
            }).ToList();
            foreach (var payment in repayments)
            {
                payment.Branch = helper.GetBranchNameForClient(payment.clientID);
                payment.BranchId = helper.GetBranchIdForClient(payment.clientID);
            }

            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                repayments = repayments.Where(p => p.BranchId == userBranchId).ToList();
            }
            var payResult = repayments.OrderBy(p => p.Branch).ThenBy(p => p.repaymentDate).ToList();
            return new KendoResponse(payResult.ToArray(), payResult.Count());
        }

        #endregion LOAN REPAYMENT

        //GET THE CLIENT SERVICE CHARGES

        #region CLIENT SERVICE CHARGES
        [HttpPost]
        public KendoResponse GetClientServiceCharges(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new CashierRepayments[0], 0);
            }

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var clientServiceRep = rent.vwCashierRepaymentsGroupeds.Where(p =>
                     (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
                     && ((p.repaymentTypeName == "Client Service Charge"))
                 ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var charges = clientServiceRep.Select(o => new CashierRepayments
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanGroupName = o.loanGroupName,
                loanGroupNumber = o.loanGroupNumber,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName,
                GroupName = o.loanGroupName
            }).ToList();
            foreach (var charge in charges)
            {
                charge.Branch = helper.GetBranchNameForClient(charge.clientID);
                charge.BranchId = helper.GetBranchIdForClient(charge.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                charges = charges.Where(p => p.BranchId == userBranchId).ToList();
            }
            var cResult = charges.OrderBy(r => r.Branch).ThenBy(r => r.repaymentDate).ToList();
            return new KendoResponse(cResult.ToArray(), cResult.Count);
        }


        #endregion CLIENT SERVICE CHARGES


        //GET SUSU REPORT

        #region SUSU REPORT
        [HttpPost]
        public KendoResponse GetSusuReport(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new OtherCashierRepayment[0], 0);
            }
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;

            var resSusu = rent.vwCashierRepayments.Where(p =>
             (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
             && (fieldAgent == "" || p.agentName == fieldAgent)
             && ((p.repaymentTypeName == "Group Susu Contribution" || p.repaymentTypeName == "Normal Susu Contribution"))
         )
         .Where(p => p.posted)
         .OrderBy(p => p.repaymentDate)
         .ToList();
            var susPayments = resSusu.Select(o => new OtherCashierRepayment
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName
            }).ToList();
            foreach (var susu in susPayments)
            {
                susu.Branch = helper.GetBranchNameForClient(susu.clientID);
                susu.GroupName = helper.GetGroupNameForClient(susu.clientID);
                susu.BranchId = helper.GetBranchIdForClient(susu.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                susPayments = susPayments.Where(p => p.BranchId == userBranchId).ToList();
            }
            var susuResult = susPayments.OrderBy(r => r.Branch).ThenBy(r => r.repaymentDate).ToList();
            return new KendoResponse(susuResult.ToArray(), susuResult.Count);
        }


        #endregion SUSU REPORT

        //GET FEES REPORT

        #region FEES REPORT
        [HttpPost]
        public KendoResponse GetFeesReport(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new OtherCashierRepayment[0], 0);
            }
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;

            var resFees = rent.vwCashierRepayments.Where(p =>
                     (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
                     && ((p.repaymentTypeName == "Processing Fee" || p.repaymentTypeName == "Application Fee"
                     || p.repaymentTypeName == "Commission") || (p.feePaid > 0))
                 ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                 .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var feePayments = resFees.Select(o => new OtherCashierRepayment
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName
            }).ToList();
            foreach (var fee in feePayments)
            {
                fee.Branch = helper.GetBranchNameForClient(fee.clientID);
                fee.GroupName = helper.GetGroupNameForClient(fee.clientID);
                fee.BranchId = helper.GetBranchIdForClient(fee.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                feePayments = feePayments.Where(p => p.BranchId == userBranchId).ToList();
            }
            var feeResult = feePayments.OrderBy(r => r.Branch).ThenBy(r => r.repaymentDate).ToList();
            return new KendoResponse(feeResult.ToArray(), feeResult.Count);
        }


        #endregion FEES REPORT

        //GET DISBURSEMENT REPORT

        #region DISBURSEMENT REPORT
        [HttpPost]
        public KendoResponse GetDisbursementReport(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new CashierDisbursement[0], 0);
            }
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resDisb = rent.vwCashierDisbs.Where(p => p.disbursementDate >= startDate
                && p.disbursementDate <= endDate)
                .Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .ToList();
            var disbsmnts = resDisb.Select(o => new CashierDisbursement
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                clientID = o.clientID,
                clientName = o.clientName,
                full_name = o.full_name,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                posted = o.posted,
                userName = o.userName,
                amountDisbursed = o.amountDisbursed,
                loanAmount = o.loanAmount,
                disbursementDate = o.disbursementDate
            }).ToList();
            foreach (var disb in disbsmnts)
            {
                disb.Branch = helper.GetBranchNameForClient(disb.clientID);
                disb.GroupName = helper.GetGroupNameForClient(disb.clientID);
                disb.BranchId = helper.GetBranchIdForClient(disb.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                disbsmnts = disbsmnts.Where(p => p.BranchId == userBranchId).ToList();
            }
            var disbResult = disbsmnts.OrderBy(r => r.Branch).ThenBy(r => r.disbursementDate).ToList();
            return new KendoResponse(disbResult.ToArray(), disbResult.Count);
        }


        #endregion DISBURSEMENT REPORT

        //GET DEPOSIT ADDITIONAL REPORT

        #region DEPOSIT ADDITIONAL REPORT
        [HttpPost]
        public KendoResponse GetDepositAdditionals(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new DepositAdditionalModel[0], 0);
            }

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resDA = rent.vwDepositAdditionals.Where(p => p.depositDate >= startDate
                 && p.depositDate <= endDate)
                 .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                 .Where(p => p.posted)
                 .OrderBy(r => r.branchName)
                 .ToList();
            var results = resDA.Select(e => new DepositAdditionalModel
            {
                accountNumber = e.accountNumber,
                agentName = e.agentName,
                depositAdditionalID = e.depositAdditionalID,
                depositAmount = e.depositAmount,
                balance = e.balance,
                branchName = e.branchName,
                clientID = e.clientID,
                clientName = e.clientName,
                creation_date = e.creation_date,
                creator = e.creator,
                depositDate = e.depositDate,
                depositID = e.depositID,
                depositNo = e.depositNo,
                depositTypeID = e.depositTypeID,
                depositTypeName = e.depositTypeName,
                interestExpected = e.interestExpected,
                firstDepositDate = e.firstDepositDate,
                interest = e.interest,
                interestBalance = e.interestBalance,
                interestRate = e.interestRate,
                staffID = e.staffID,
                staffName = e.staffName,
                modeOfPaymentName = e.modeOfPaymentName,
                maturityDate = e.maturityDate,
                naration = e.naration,
                paidInt = e.paidInt,
                staffNo = e.staffNo,
                paidPrinc = e.paidPrinc,
                posted = e.posted,
                principal = e.principal,
                principalBalance = e.principalBalance
            }).ToList();
            foreach (var dep in results)
            {
                dep.GroupName = helper.GetGroupNameForClient(dep.clientID);
                dep.BranchId = helper.GetBranchIdForClient(dep.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                results = results.Where(p => p.BranchId == userBranchId).ToList();
            }
            return new KendoResponse(results.ToArray(), results.Count);
        }


        #endregion DEPOSIT ADDITIONAL REPORT

        //GET DEPOSIT ADDITIONAL REPORT

        #region DEPOSIT WITHDRAWALS REPORT
        [HttpPost]
        public KendoResponse GetDepositWithdrawals(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new DepositWithdrawalModel[0], 0);
            }

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resDW = rent.vwDepositWithdrawals.Where(p => p.withdrawalDate >= startDate
                && p.withdrawalDate <= endDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .OrderBy(r => r.branchName)
                .ToList();
            var results = resDW.Select(e => new DepositWithdrawalModel
            {
                accountNumber = e.accountNumber,
                agentName = e.agentName,
                depositWithdrawalID = e.depositWithdrawalID,
                amountWithdrawn = e.amountWithdrawn,
                branchName = e.branchName,
                clientID = e.clientID,
                clientName = e.clientName,
                creation_date = e.creation_date,
                creator = e.creator,
                withdrawalDate = e.withdrawalDate,
                depositID = e.depositID,
                depositNo = e.depositNo,
                depositTypeID = e.depositTypeID,
                depositTypeName = e.depositTypeName,
                interestExpected = e.interestExpected,
                interestWithdrawal = e.interestWithdrawal,
                interestBalance = e.interestBalance,
                interestRate = e.interestRate,
                posted = e.posted,
                principalWithdrawal = e.principalWithdrawal,
                principalBalance = e.principalBalance
            }).ToList();
            foreach (var dep in results)
            {
                dep.GroupName = helper.GetGroupNameForClient(dep.clientID);
                dep.BranchId = helper.GetBranchIdForClient(dep.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                results = results.Where(p => p.BranchId == userBranchId).ToList();
            }
            return new KendoResponse(results.ToArray(), results.Count);
        }


        #endregion DEPOSIT WITHDRAWALS REPORT

        //GET SAVINGS ADDITIONAL REPORT

        #region SAVINGS ADDITIONAL REPORT
        [HttpPost]
        public KendoResponse GetSavingAdditionals(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new SavingAddWithModel[0], 0);
            }

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resSA = rent.vwSavingAdditionals.Where(p => p.savingDate >= startDate
                && p.savingDate <= endDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .ToList();
            var results = resSA.Select(r => new SavingAddWithModel
            {
                clientID = r.clientID,
                clientName = r.clientName,
                principalBalance = r.principalBalance,
                interestBalance = r.interestBalance,
                TransDate = r.savingDate,
                savingAmount = r.savingAmount,
                savingNo = r.savingNo,
                modeOfPaymentName = r.modeOfPaymentName,
                creator = r.creator.ToUpper(),
                interestRate = r.interestRate,
                SavingTypeName = r.SavingTypeName,
                SavingBalance = r.principalBalance + r.interestBalance
            })
            .ToList();
            foreach (var save in results)
            {
                save.Branch = helper.GetBranchNameForClient(save.clientID);
                save.GroupName = helper.GetGroupNameForClient(save.clientID);
                save.BranchId = helper.GetBranchIdForClient(save.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                results = results.Where(p => p.BranchId == userBranchId).ToList();
            }
            var saveResult = results.OrderBy(r => r.Branch).ThenBy(r => r.TransDate).ToList();
            return new KendoResponse(saveResult.ToArray(), saveResult.Count);
        }


        #endregion SAVINGS ADDITIONAL REPORT

        //GET SAVINGS WITHDRAWALS REPORT

        #region SAVINGS WITHDRAWALS REPORT
        [HttpPost]
        public KendoResponse GetSavingWithdrawals(CashierReportInputModel cashierModel)
        {
            var currentUser = User?.Identity?.Name?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUser);
            var isOwner = helper.IsOwner(currentUser);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new SavingAddWithModel[0], 0);
            }
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resSW = rent.vwSavingWithdrawals.Where(p => p.withdrawalDate >= startDate
                && p.withdrawalDate <= endDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted && p.amountWithdrawn >= 1)
                .ToList();
            var results = resSW.Select(r => new SavingAddWithModel
            {
                clientID = r.clientID,
                clientName = r.clientName,
                principalBalance = r.principalBalance,
                interestBalance = r.interestBalance,
                TransDate = r.withdrawalDate,
                amountWithdrawn = r.amountWithdrawn,
                savingNo = r.savingNo,
                modeOfPaymentName = r.modeOfPaymentName,
                creator = r.creator.ToUpper(),
                interestRate = r.interestRate,
                SavingTypeName = r.SavingTypeName,
                SavingBalance = r.principalBalance + r.interestBalance
            })
            .ToList();
            foreach (var save in results)
            {
                save.Branch = helper.GetBranchNameForClient(save.clientID);
                save.GroupName = helper.GetGroupNameForClient(save.clientID);
                save.BranchId = helper.GetBranchIdForClient(save.clientID);
            }
            //Check for Branch Admin, to display only his branch's data
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUser);
                results = results.Where(p => p.BranchId == userBranchId).ToList();
            }
            var saveResult = results.OrderBy(r => r.Branch).ThenBy(r => r.TransDate).ToList();
            return new KendoResponse(saveResult.ToArray(), saveResult.Count);
        }


        #endregion SAVINGS WITHDRAWALS REPORT


        #endregion ADMIN CASHIER REPORT


        #region INDIVIDUAL CASHIER REPORTS

        //GET THE SAVINGS AND OUTSTANDINGS  
        #region INDIVIDUAL OUTSTANDING

        [HttpPost]
        public KendoResponse GetSavingAndOutstandingLoans(CashierReportInputModel cashierModel)
        {
            // var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var officerGroupIds = le.loanGroups
                .Where(p => p.staff.userName.ToLower().Trim() == loginOfficer.ToLower().Trim())
                .Select(p => p.loanGroupId).ToList();
            var outstandingLoans = GetIndividualOutstandingLoans(endDate, loginOfficer, officerGroupIds);
            var savings = GetIndividualSavings(endDate);
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            List<CombinedSavingAndOutstandingLoan> combined = new List<CombinedSavingAndOutstandingLoan>();
            foreach (var outstand in outstandingLoans)
            {
                CombinedSavingAndOutstandingLoan comb = new CombinedSavingAndOutstandingLoan();
                comb.AmountDisbursed = outstand.LoanAmount;
                comb.ClientName = outstand.ClientName;
                comb.DisbursementDate = outstand.DisbursementDate;
                comb.DisbursedDateToString = outstand.DisbursementDate.ToShortDateString().ToString();
                comb.LastRepaymentDate = outstand.ExpiryDate;
                comb.LoanGroupName = string.IsNullOrWhiteSpace(outstand.LoanGroupName) ? helper.GetGroupNameForClient(outstand.ClientId) : outstand.LoanGroupName;
                comb.LoanNo = outstand.LoanNo;
                comb.Officer = outstand.Officer;
                comb.Outstanding = outstand.OutstandingAmount;
                comb.Paid = outstand.Paid;
                comb.WriteOffAmount = outstand.WriteOffAmount;
                comb.ClientID = outstand.ClientId;
                comb.LoanGroupId = outstand.LoanGroupId;
                combined.Add(comb);

            }
            foreach (var saving in savings)
            {
                var clientWithSaving = combined.FirstOrDefault(c => c.ClientID == saving.ClientId);
                if (clientWithSaving != null && clientWithSaving.SavingId == 0)
                {
                    clientWithSaving.SavingId = saving.SavingId;
                    clientWithSaving.SavingNo = saving.SavingNo;
                    clientWithSaving.AvailableInterestBalance = saving.AvailableInterestBalance;
                    clientWithSaving.AvailablePrincipalBalance = saving.AvailablePrincipalBalance;
                    clientWithSaving.PrincipalBalance = saving.PrincipalBalance;
                    clientWithSaving.InterestBalance = saving.InterestBalance;
                    clientWithSaving.ClientID = saving.ClientId;
                    clientWithSaving.ClientName = saving.ClientName;
                    clientWithSaving.SavingBalance = saving.PrincipalBalance + saving.InterestBalance;
                    clientWithSaving.LoanGroupName = helper.GetGroupNameForClient(saving.ClientId);
                }
                else
                {
                    CombinedSavingAndOutstandingLoan combItem = new CombinedSavingAndOutstandingLoan();
                    combItem.SavingId = saving.SavingId;
                    combItem.SavingNo = saving.SavingNo;
                    combItem.AvailableInterestBalance = saving.AvailableInterestBalance;
                    combItem.AvailablePrincipalBalance = saving.AvailablePrincipalBalance;
                    combItem.PrincipalBalance = saving.PrincipalBalance;
                    combItem.InterestBalance = saving.InterestBalance;
                    combItem.ClientID = saving.ClientId;
                    combItem.ClientName = saving.ClientName;
                    combItem.LoanGroupId = helper.GetGroupIdForClient(saving.ClientId);

                    var gOfficer = helper.GetGroupOfficerUserName(combItem.LoanGroupId ?? 0);
                    combItem.Officer = //string.IsNullOrWhiteSpace(saving.Creator) ?
                        (string.IsNullOrWhiteSpace(gOfficer) ? "No Officer Assigned" : gOfficer);
                       // : saving.Creator;
                    combItem.LoanGroupName = helper.GetGroupNameForClient(saving.ClientId);
                    combItem.SavingBalance = saving.PrincipalBalance + saving.InterestBalance;
                    if (combItem.SavingBalance > 0)
                        combined.Add(combItem);
                }
            }

            foreach (var item in combined)
            {
                item.Branch = helper.GetBranchNameForClient(item.ClientID);
                item.BranchId = helper.GetBranchIdForClient(item.ClientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                {
                    combined = combined.Where(p => p.BranchId == userBranchId).ToList();
                }
                else
                {
                    combined = combined.Where(p => p.BranchId == userBranchId && officerGroupIds.Contains(p.LoanGroupId ?? 0)).ToList();
                }
            }
            var query = combined.OrderBy(p => p.Branch).ThenBy(p => p.LoanGroupName).AsQueryable();

            var data = query.ToArray();
            return new KendoResponse(data, query.Count());
        }


        public List<LoanOutstandingModel> GetIndividualOutstandingLoans(DateTime endDate, string loginOfficer, List<int> officerGroupIds)
        {
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);
            var isOwner = helper.IsOwner(loginOfficer);
            var endOfDayDate = endDate.Date.AddDays(1).AddSeconds(-1);
            reportEntities repEnt = new reportEntities();

            var outstandings = new List<LoanOutstandingModel>();
            var outResult = repEnt.spOutstanding(endOfDayDate)
                .Where(p =>
                ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)) > 0)
                .ToList();

            outstandings = outResult.Select(p => new LoanOutstandingModel
            {
                ClientId = p.clientID,
                ClientName = p.clientName,
                LoanAmount = p.amountDisbursed,
                LoanGroupName = string.IsNullOrWhiteSpace(p.LoanGroupName) ? "No Group" : p.LoanGroupName,
                LoanId = p.loanID,
                LoanNo = p.loanNo,
                OutstandingAmount = ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)),
                DaysDefault = p.daysDue,
                ExpiryDate = p.expiryDate,
                WriteOffAmount = p.writtenOff,
                Collateral = p.fairValue,
                DisbursementDate = p.disbursementDate,
                Paid = p.totalPaid
            })
            .Where(p => p.OutstandingAmount >= 1)
            .OrderBy(p => p.LoanGroupName)
            .ToList();
            foreach (var outstand in outstandings)
            {
                var groupId = helper.GetGroupIdForClient(outstand.ClientId);
                if (groupId != null)
                {
                    outstand.Officer = helper.GetGroupOfficerUserName(groupId.Value);
                    outstand.LoanGroupId = groupId.Value;
                }
                else
                {
                    outstand.Officer = !string.IsNullOrWhiteSpace(outstand.Officer) ? outstand.Officer : "No Officer Assigned";
                }
                outstand.BranchId = helper.GetBranchIdForClient(outstand.ClientId);
            }
            if (!isOwner)
            {
                if (isBranchAdmin)
                {
                    var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                    outstandings = outstandings.Where(p => p.BranchId == userBranchId).ToList();
                }
                else
                {
                    outstandings = outstandings?.Where(e => officerGroupIds.Contains(e.LoanGroupId))?.OrderBy(p => p.LoanGroupName)?.ToList();
                }
            }

            return outstandings;
        }

        public List<NewSavingModel> GetIndividualSavings(DateTime? endDate)
        {
            endDate = endDate.Value.Date.AddDays(1).AddSeconds(-1);
            var savings = rent.vwSavingStatements
                .Where(p => p.date <= endDate)
                .GroupBy(p => new
                {
                    p.loanNo,
                    p.clientID,
                    p.clientName,
                    p.loanID,
                    p.staffName
                }
                )
               .Select(p => new NewSavingModel
               {
                   ClientId = p.Key.clientID,
                   ClientName = p.Key.clientName,
                   AvailableInterestBalance = p.Sum(e => e.interestAccruedAmount - e.intWithdrawalAmount),
                   AvailablePrincipalBalance = p.Sum(e => e.depositAmount - e.princWithdrawalAmount),
                   PrincipalBalance = p.Sum(e => e.depositAmount - e.princWithdrawalAmount),
                   InterestBalance = p.Sum(e => e.interestAccruedAmount - e.intWithdrawalAmount),
                   SavingId = p.Key.loanID,
                   SavingNo = p.Key.loanNo,
                   StaffId = 0,
                   Creator = p.Key.staffName,
               })
               .Where(c => (c.PrincipalBalance + c.InterestBalance) > 0)
               .OrderBy(c => c.ClientName)
               .ToList();
            return savings;

        }
        #endregion INDIVIDUAL OUTSTANDING


        //GET LOAN REPAYMENT

        #region INDIVIDUAL LOAN REPAYMENT

        [HttpPost]
        public KendoResponse GetIndividualLoanRepayments(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User?.Identity?.Name?.Trim()?.ToLower();
            var startDate = cashierModel.StartDate.Date;
            var endDate = cashierModel.EndDate.AddDays(1).AddSeconds(-1);
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            var resRep = rent.vwCashierRepaymentsGroupeds
                .Where(p =>
                     (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
                     && ((p.repaymentTypeName == "Interest Only" || p.repaymentTypeName == "Principal and Interest"
                         || p.repaymentTypeName == "Principal Only" || p.repaymentTypeName == "Penalty"))
                 )
                 .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var repayments = resRep.Select(o => new CashierRepayments
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanGroupName = o.loanGroupName,
                loanGroupNumber = o.loanGroupNumber,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName,
                GroupName = o.loanGroupName

            }).ToList();
            foreach (var payment in repayments)
            {
                payment.Branch = helper.GetBranchNameForClient(payment.clientID);
                payment.BranchId = helper.GetBranchIdForClient(payment.clientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                {
                    repayments = repayments.Where(p => p.BranchId == userBranchId).ToList();
                }
                else
                {
                    repayments = repayments.Where(p => p.BranchId == userBranchId
                    && p.userName.ToLower().Trim() == loginOfficer.ToLower().Trim())
                    .ToList();
                }
            }
            var payResult = repayments.OrderBy(p => p.Branch).ThenBy(p => p.repaymentDate).ToList();
            var resCount = resRep.Count();
            var dataToReturn = payResult.ToArray();
            return new KendoResponse(dataToReturn, resCount);

        }

        #endregion INDIVIDUAL LOAN REPAYMENT


        //GET THE CLIENT SERVICE CHARGES

        #region INDIVIDUAL CLIENT SERVICE CHARGES
        [HttpPost]
        public KendoResponse GetIndividualClientServiceCharges(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var clientServiceRep = rent.vwCashierRepaymentsGroupeds.Where(p =>
                     (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
                     && ((p.repaymentTypeName == "Client Service Charge"))
                 )
                .Where(p => p.posted).OrderBy(p => p.repaymentDate).ToList();
            var charges = clientServiceRep.Select(o => new CashierRepayments
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanGroupName = o.loanGroupName,
                loanGroupNumber = o.loanGroupNumber,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName,
                GroupName = o.loanGroupName
            }).ToList();
            foreach (var charge in charges)
            {
                charge.Branch = helper.GetBranchNameForClient(charge.clientID);
                charge.BranchId = helper.GetBranchIdForClient(charge.clientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                {
                    charges = charges.Where(r => r.BranchId == userBranchId).ToList();
                }
                else
                {
                    charges = charges.Where(r => r.BranchId == userBranchId
                    && r.userName.Trim().ToLower() == loginOfficer.Trim().ToLower())
                    .ToList();
                }

            }
            var cResult = charges.OrderBy(r => r.Branch).ThenBy(r => r.repaymentDate).ToList();
            return new KendoResponse(cResult.ToArray(), cResult.Count);
        }


        #endregion INDIVIDUAL CLIENT SERVICE CHARGES

        //GET SUSU REPORT

        #region INDIVIDUAL SUSU REPORT
        [HttpPost]
        public KendoResponse GetIndividualSusuReport(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resSusu = rent.vwCashierRepayments.Where(p =>
             (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
             && (fieldAgent == "" || p.agentName == fieldAgent)
             && ((p.repaymentTypeName == "Group Susu Contribution" || p.repaymentTypeName == "Normal Susu Contribution"))
         )
         .Where(p => p.posted)
         .OrderBy(p => p.repaymentDate)
         .ToList();
            var susPayments = resSusu.Select(o => new OtherCashierRepayment
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName

            }).ToList();
            foreach (var susu in susPayments)
            {
                susu.Branch = helper.GetBranchNameForClient(susu.clientID);
                susu.GroupName = helper.GetGroupNameForClient(susu.clientID);
                susu.BranchId = helper.GetBranchIdForClient(susu.clientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                {
                    susPayments = susPayments.Where(p => p.BranchId == userBranchId).ToList();
                }
                else
                {
                    susPayments = susPayments.Where(p => p.BranchId == userBranchId
                    && p.userName.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
                }
            }
            var susuResult = susPayments.OrderBy(r => r.Branch).ThenBy(r => r.repaymentDate).ToList();
            return new KendoResponse(susuResult.ToArray(), susuResult.Count);
        }


        #endregion INDIVIDUAL SUSU REPORT

        //GET FEES REPORT

        #region INDIVIDUAL FEES REPORT
        [HttpPost]
        public KendoResponse GetIndividualFeesReport(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            var userName = cashierModel.CashierUsername;
            var fieldAgent = cashierModel.FieldAgentName;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resFees = rent.vwCashierRepayments.Where(p =>
                     (p.repaymentDate >= startDate && p.repaymentDate <= endDate)
                     && ((p.repaymentTypeName == "Processing Fee" || p.repaymentTypeName == "Application Fee"
                     || p.repaymentTypeName == "Commission") || (p.feePaid > 0))
                 )
                 .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var feePayments = resFees.Select(o => new OtherCashierRepayment
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                amountPaid = o.amountPaid,
                cashierTillID = o.cashierTillID,
                clientID = o.clientID,
                clientName = o.clientName,
                feePaid = o.feePaid,
                full_name = o.full_name,
                interestPaid = o.interestPaid,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                penaltyPaid = o.penaltyPaid,
                posted = o.posted,
                principalPaid = o.principalPaid,
                repaymentDate = o.repaymentDate,
                repaymentTypeName = o.repaymentTypeName,
                userName = o.userName
            }).ToList();
            foreach (var fee in feePayments)
            {
                fee.Branch = helper.GetBranchNameForClient(fee.clientID);
                fee.GroupName = helper.GetGroupNameForClient(fee.clientID);
                fee.BranchId = helper.GetBranchIdForClient(fee.clientID);

            }
            if (!helper.IsOwner(loginOfficer))
            {

                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                    feePayments = feePayments.Where(p => p.BranchId == userBranchId).ToList();
                else
                    feePayments = feePayments.Where(p => p.BranchId == userBranchId && p.userName.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
            }
            var feeResult = feePayments.OrderBy(r => r.Branch).ThenBy(r => r.repaymentDate).ToList();
            return new KendoResponse(feeResult.ToArray(), feeResult.Count);
        }


        #endregion INDIVIDUAL FEES REPORT

        //GET DISBURSEMENT REPORT

        #region INDIVIDUAL DISBURSEMENT REPORT
        [HttpPost]
        public KendoResponse GetIndividualDisbursementReport(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User?.Identity?.Name?.Trim()?.ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resDisb = rent.vwCashierDisbs.Where(p => p.disbursementDate >= startDate
                && p.disbursementDate <= endDate)
                .Where(p => p.posted)
                .ToList();
            var disbsmnts = resDisb.Select(o => new CashierDisbursement
            {
                accountNumber = o.accountNumber,
                agentName = o.agentName,
                clientID = o.clientID,
                clientName = o.clientName,
                full_name = o.full_name,
                loanID = o.loanID,
                loanNo = o.loanNo,
                modeOfPaymentName = o.modeOfPaymentName,
                posted = o.posted,
                userName = o.userName,
                amountDisbursed = o.amountDisbursed,
                loanAmount = o.loanAmount,
                disbursementDate = o.disbursementDate
            }).ToList();
            foreach (var disb in disbsmnts)
            {
                disb.Branch = helper.GetBranchNameForClient(disb.clientID);
                disb.GroupName = helper.GetGroupNameForClient(disb.clientID);
                disb.BranchId = helper.GetBranchIdForClient(disb.clientID);
            }

            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                    disbsmnts = disbsmnts.Where(r => r.BranchId == userBranchId).ToList();
                else
                    disbsmnts = disbsmnts.Where(r => r.BranchId == userBranchId && r.userName.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
            }
            var disbResult = disbsmnts.OrderBy(r => r.Branch).ThenBy(r => r.disbursementDate).ToList();
            return new KendoResponse(disbResult.ToArray(), disbResult.Count);

        }


        #endregion DISBURSEMENT REPORT

        //GET DEPOSIT ADDITIONAL REPORT

        #region INDIVIDUAL DEPOSIT ADDITIONAL REPORT
        [HttpPost]
        public KendoResponse GetIndividualDepositAdditionals(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resDA = rent.vwDepositAdditionals.Where(p => p.depositDate >= startDate
                 && p.depositDate <= endDate)
                 .Where(p => p.posted)
                 .OrderBy(p => p.branchName)
                 .ToList().Select(e => new DepositAdditionalModel
                 {
                     accountNumber = e.accountNumber,
                     agentName = e.agentName,
                     depositAdditionalID = e.depositAdditionalID,
                     depositAmount = e.depositAmount,
                     balance = e.balance,
                     branchName = e.branchName,
                     clientID = e.clientID,
                     clientName = e.clientName,
                     creation_date = e.creation_date,
                     creator = e.creator,
                     depositDate = e.depositDate,
                     depositID = e.depositID,
                     depositNo = e.depositNo,
                     depositTypeID = e.depositTypeID,
                     depositTypeName = e.depositTypeName,
                     //interestExpected = e.interestExpected,
                     firstDepositDate = e.firstDepositDate,
                     interest = e.interest,
                     interestBalance = e.interestBalance,
                     interestRate = e.interestRate,
                     staffID = e.staffID,
                     staffName = e.staffName,
                     modeOfPaymentName = e.modeOfPaymentName,
                     maturityDate = e.maturityDate,
                     naration = e.naration,
                     //paidInt = e.paidInt,
                     staffNo = e.staffNo,
                     paidPrinc = e.paidPrinc,
                     //posted = e.posted,
                     principal = e.principal,
                     principalBalance = e.principalBalance
                 }).ToList();
            foreach (var dep in resDA)
            {
                dep.GroupName = helper.GetGroupNameForClient(dep.clientID);
                dep.BranchId = helper.GetBranchIdForClient(dep.clientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                    resDA = resDA.Where(p => p.BranchId == userBranchId).ToList();
                else
                    resDA = resDA.Where(p => p.BranchId == userBranchId && p.staffName.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
            }
            return new KendoResponse(resDA.ToArray(), resDA.Count);
        }


        #endregion INDIVIDUAL DEPOSIT ADDITIONAL REPORT

        //GET DEPOSIT WITHDRAWALS REPORT
        #region INDIVIDUAL DEPOSIT WITHDRAWALS REPORT
        [HttpPost]
        public KendoResponse GetIndividualDepositWithdrawals(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);
            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resDW = rent.vwDepositWithdrawals.Where(p => p.withdrawalDate >= startDate
                && p.withdrawalDate <= endDate)
                .Where(p => p.posted)
                .OrderBy(p => p.branchName)
                .ToList().Select(e => new DepositWithdrawalModel
                {
                    accountNumber = e.accountNumber,
                    agentName = e.agentName,
                    depositWithdrawalID = e.depositWithdrawalID,
                    amountWithdrawn = e.amountWithdrawn,
                    branchName = e.branchName,
                    clientID = e.clientID,
                    clientName = e.clientName,
                    creation_date = e.creation_date,
                    creator = e.creator,
                    withdrawalDate = e.withdrawalDate,
                    depositID = e.depositID,
                    depositNo = e.depositNo,
                    depositTypeID = e.depositTypeID,
                    depositTypeName = e.depositTypeName,
                    interestExpected = e.interestExpected,
                    interestWithdrawal = e.interestWithdrawal,
                    interestBalance = e.interestBalance,
                    interestRate = e.interestRate,
                    //posted = e.posted,
                    principalWithdrawal = e.principalWithdrawal,
                    principalBalance = e.principalBalance
                }).ToList();

            foreach (var wid in resDW)
            {
                wid.GroupName = helper.GetGroupNameForClient(wid.clientID);
                wid.BranchId = helper.GetBranchIdForClient(wid.clientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                    resDW = resDW.Where(p => p.BranchId == userBranchId).ToList();
                else
                    resDW = resDW.Where(p => p.BranchId == userBranchId && p.creator.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
            }
            return new KendoResponse(resDW.ToArray(), resDW.Count);
        }


        #endregion INDIVIDUAL DEPOSIT WITHDRAWALS REPORT

        //GET SAVINGS ADDITIONAL REPORT

        #region INDIVIDUAL SAVINGS ADDITIONAL REPORT
        [HttpPost]
        public KendoResponse GetIndividualSavingAdditionals(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resSA = rent.vwSavingAdditionals.Where(p => p.savingDate >= startDate && p.savingDate <= endDate)
                .Where(p => p.posted)
                .Select(r => new SavingAddWithModel
                {
                    clientID = r.clientID,
                    clientName = r.clientName,
                    principalBalance = r.principalBalance,
                    interestBalance = r.interestBalance,
                    TransDate = r.savingDate,
                    savingAmount = r.savingAmount,
                    savingNo = r.savingNo,
                    modeOfPaymentName = r.modeOfPaymentName,
                    creator = r.creator.ToUpper(),
                    interestRate = r.interestRate,
                    SavingTypeName = r.SavingTypeName,
                    SavingBalance = r.principalBalance + r.interestBalance

                }).ToList();
            foreach (var save in resSA)
            {
                save.Branch = helper.GetBranchNameForClient(save.clientID);
                save.GroupName = helper.GetGroupNameForClient(save.clientID);
                save.BranchId = helper.GetBranchIdForClient(save.clientID);
            }
            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                    resSA = resSA.Where(p => p.BranchId == userBranchId).ToList();
                else
                    resSA = resSA.Where(p => p.BranchId == userBranchId && p.creator.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
            }
            var saveResult = resSA.OrderBy(r => r.Branch).ThenBy(r => r.TransDate).ToList();
            return new KendoResponse(saveResult.ToArray(), saveResult.Count);

        }


        #endregion INDIVIDUAL SAVINGS ADDITIONAL REPORT

        //GET SAVINGS WITHDRAWALS REPORT

        #region INDIVIDUAL SAVINGS WITHDRAWALS REPORT
        [HttpPost]
        public KendoResponse GetIndividualSavingWithdrawals(CashierReportInputModel cashierModel)
        {
            var loginOfficer = User.Identity.Name.Trim().ToLower();
            var isBranchAdmin = helper.IsBranchAdminOwner(loginOfficer);

            var startDate = cashierModel.StartDate;
            var endDate = cashierModel.EndDate;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var resSW = rent.vwSavingWithdrawals.Where(p => p.withdrawalDate >= startDate
                && p.withdrawalDate <= endDate)
                //.Where(p => p.creator.ToLower().Trim() == loginOfficer.ToLower().Trim())
                .Where(p => p.posted && p.amountWithdrawn >= 1)
                .Select(r => new SavingAddWithModel
                {
                    clientID = r.clientID,
                    clientName = r.clientName,
                    principalBalance = r.principalBalance,
                    interestBalance = r.interestBalance,
                    TransDate = r.withdrawalDate,
                    amountWithdrawn = r.amountWithdrawn,
                    savingNo = r.savingNo,
                    modeOfPaymentName = r.modeOfPaymentName,
                    creator = r.creator.ToUpper(),
                    interestRate = r.interestRate,
                    SavingTypeName = r.SavingTypeName,
                    SavingBalance = r.principalBalance + r.interestBalance

                })
                .ToList();
            foreach (var save in resSW)
            {
                save.Branch = helper.GetBranchNameForClient(save.clientID);
                save.GroupName = helper.GetGroupNameForClient(save.clientID);
                save.BranchId = helper.GetBranchIdForClient(save.clientID);
            }

            if (!helper.IsOwner(loginOfficer))
            {
                var userBranchId = helper.GetBranchIdForUser(loginOfficer);
                if (isBranchAdmin)
                    resSW = resSW.Where(p => p.BranchId == userBranchId).ToList();
                else
                    resSW = resSW.Where(p => p.BranchId == userBranchId && p.creator.Trim().ToLower() == loginOfficer.Trim().ToLower()).ToList();
            }
            var saveResult = resSW.OrderBy(r => r.Branch).ThenBy(r => r.TransDate).ToList();
            return new KendoResponse(saveResult.ToArray(), saveResult.Count);
        }


        #endregion INDIVIDUAL SAVINGS WITHDRAWALS REPORT


        #endregion INDIVIDUAL CASHIER REPORT
    }
}