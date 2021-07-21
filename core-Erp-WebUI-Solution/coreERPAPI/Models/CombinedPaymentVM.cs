using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class CombinedPaymentVM
    {
        //Savings Fields
        public string savingNo { get; set; }
        public int savingID { get; set; }
        public double principalSavingsBalance { get; set; }
        public double savingPlanAmount { get; set; }
        public double interestSavingsBalance { get; set; }
        public bool savingsPaid { get; set; }


        //Loan Fields
        public string loanNo { get; set; }
        public int loanId { get; set; }
        public int repaymentScheduleId { get; set; }
        public DateTime scheduleDate { get; set; }
        public string loanNumberWithClient { get; set; }
        public double amountDisbursed { get; set; }
        public double principalLoanBalance { get; set; }
        public int paymentTypeId { get; set; }
        public double interestAmount { get; set; }
        public double principalAmount { get; set; }
        public double amountDue { get; set; }
        public double paymentAmount { get; set; }
        public int paymentModeId { get; set; }
        public int? bankId { get; set; }
        public string chequeNumber { get; set; }
        public double cashCollateral { get; set; }

        //Common Fields
        public int clientID { get; set; }
        public string clientName { get; set; }
        public string clientAccNum { get; set; }
        public string Branch { get; set; }
        public int? BranchID { get; set; }
    }

    public class GroupCombinedBatchPaymentModel
    {
        public DateTime repaymentDate { get; set; }
        public IEnumerable<CombinedPaymentVM> payments { get; set; }
    }
}