using coreData.Constants;
using coreERP.Models;
using coreERP.Providers;
using coreErpApi.Controllers.Models;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setup
{
    public class CombinedPaymentController:ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        HelperMethod helper;
        public CombinedPaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            helper = new HelperMethod();

        }


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
                   branchID = l.branchID
               })
               .OrderBy(C => C.clientName)
               .ToList();
            foreach (var save in savings)
            {
                //Get branch for client
                save.Branch =helper.GetBranchNameForClient(save.clientID);
            }

            savings = savings.OrderBy(p => p.Branch).ToList();
            return savings;
        }

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
                    branchID = c.branchID
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
                    record.paymentAmount = schd.principalPayment + schd.interestPayment;
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
                    paymentDate = save.paymentDate
                };
                foreach (var input in save.combinedItems)
                {
                    if (input.savingsPaid && input.savingPlanAmount > 0 && input.savingID > 0)
                    {
                        PostSingleSavingsRecordCombined(save, results, input);
                    }
                    if (input.paymentAmount > 0 && input.loanId > 0 && input.savingsPaid)
                    {
                        PostSingleLoan(input,loginUser,save.paymentDate, ct.cashiersTillID);
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
                }

                return results;
            }
        }
        private void PostSingleLoan(CombinedPaymentVM input,string loginUser,DateTime repaymentDate,int cashierTillId)
        {
            var currentLoan = le.loans.FirstOrDefault(p => p.loanID == input.loanId);
            
            if (currentLoan != null)
            {
                var crtp = new cashierReceipt
                {
                    amount = input.paymentAmount,
                    bankID = input.bankId,
                    checkNo = input.chequeNumber,
                    clientID = input.clientID,
                    loanID = input.loanId,
                    paymentModeID = input.paymentModeId,
                    posted = false,
                    txDate = repaymentDate.Date,
                    cashierTillID = cashierTillId,
                    repaymentTypeID = input.paymentTypeId
                };
                //check payment Type apply the amount paid
                if (input.paymentTypeId == 2) crtp.principalAmount = input.paymentAmount;
                else if (input.paymentTypeId == 3) crtp.interestAmount = input.paymentAmount;
                else if (input.paymentTypeId == 6) crtp.feeAmount = input.paymentAmount;
                else if (input.paymentTypeId == 7) crtp.addInterestAmount = input.paymentAmount;
                le.cashierReceipts.Add(crtp);                

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
            }
            return string.Empty;
        }

        private string attemptReservation(int savingsId, double savingsPlanAmount, DateTime paymentDate, string currentUserName)
        {
            using (var con = new SqlConnection(ConfigurationManager.AppSettings["RAW_CON_STR"]))
            {
                SqlCommand cmd = new SqlCommand("[ln].[sp_attempt_reservation]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@savingId", savingsId);
                cmd.Parameters.AddWithValue("@amount", savingsPlanAmount);
                cmd.Parameters.AddWithValue("@reservationTypeId", 2);
                cmd.Parameters.AddWithValue("@reservedBy", currentUserName);
                cmd.Parameters.AddWithValue("@naration", savingsId);

                var tranParam = cmd.Parameters.Add("@transactionId", System.Data.SqlDbType.NVarChar, 100);
                tranParam.Direction = System.Data.ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                return tranParam.Value.ToString();


            }

        }
    }
}