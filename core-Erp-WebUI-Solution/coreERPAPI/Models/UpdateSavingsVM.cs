using coreERP.Models;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class UpdateSavingsVM
    {
        public DateTime paymentDate { get; set; }
        public List<NewSavingsVM> savings { get; set; }
        
    }

    public class UpdateCombinedSavingsVM
    {
        public DateTime paymentDate { get; set; }
        public List<CombinedPaymentVM> combinedItems { get; set; }

    }

    public class SavingResultsVM : NewSavingsVM
    {
        public SavingResultsVM (NewSavingsVM input)
        {
            this.savingID = input.savingID;
            this.clientID = input.clientID;
            this.principalBalance = input.principalBalance;
            this.savingNo = input.savingNo;
            this.interestBalance = input.interestBalance;
            this.savingPlanAmount = input.savingPlanAmount;
        }
       
        public string failureReason { get; set; }
    }

    public class CombinedResultsVM : CombinedPaymentVM
    {
        public CombinedResultsVM(CombinedPaymentVM input)
        {
            this.amountDisbursed = input.amountDisbursed;
            this.amountDue = input.amountDue;
            this.bankId = input.bankId;
            this.cashCollateral = input.cashCollateral;
            this.chequeNumber = input.chequeNumber;
            this.clientAccNum = input.clientAccNum;
            this.clientName = input.clientName;
            this.clientID = input.clientID;
            this.interestAmount = input.interestAmount;
            this.interestSavingsBalance = input.interestSavingsBalance;
            this.loanId = input.loanId;
            this.loanNumberWithClient = input.loanNumberWithClient;
            this.paymentAmount = input.paymentAmount;
            this.paymentModeId = input.paymentModeId;
            this.paymentTypeId = input.paymentTypeId;
            this.principalAmount = input.principalAmount;
            this.principalLoanBalance = input.principalLoanBalance;
            this.principalSavingsBalance = input.principalSavingsBalance;
            this.repaymentScheduleId = input.repaymentScheduleId;
            this.savingID = input.savingID;
            this.savingNo = input.savingNo;
            this.savingPlanAmount = input.savingPlanAmount;
            this.savingsPaid = input.savingsPaid;
            this.scheduleDate = input.scheduleDate;
        }

        public string failureReason { get; set; }
    }

    public class UpdateCombinedResultVM
    {
        public DateTime paymentDate { get; set; }
        public List<CombinedResultsVM> successfulSavings { get; set; }
        public List<CombinedResultsVM> failedSavings { get; set; }
    }

    public class UpdateSavingsResultVM
    {
        public DateTime paymentDate { get; set; }
        public List<SavingResultsVM> successfulSavings { get; set; }
        public List<SavingResultsVM> failedSavings { get; set; }
    }
}