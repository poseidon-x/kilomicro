using System;
using System.Collections.Generic;

namespace coreErp.Models.Loan
{
    public class LoanBatchCashRepaymetModel
    {
        public int loanId { get; set; }
        public string clientFullName { get; set; }
        public string loanNumber { get; set; }
        public double amountDisbursed { get; set; }
        public int repaymentScheduleId { get; set; }
        public DateTime repaymentDate { get; set; }
        public double amountDue { get; set; }
        public bool paid { get; set; }
        public double dueSchedule { get; set; }
        public string groupName { get; set; }
        public int clientId { get; set; }
        public DateTime groupAddedDate { get; set; }
        public string Branch { get; set; }

    }

    public class LoanBatchCheckListModel
    {
        public int loanId { get; set; }
        public string clientFullName { get; set; }
        public string loanNumber { get; set; }
        public double amountRequested { get; set; }

        public DateTime approvalDate { get; set; }
        public double amountApproved { get; set; }
        public bool approved { get; set; }
        public string groupName { get; set; }
        public int clientId { get; set; }
        public DateTime groupAddedDate { get; set; }
        public List<LoansCheckListModel> checkListItems { get; set; }
    }
    public class LoansCheckListModel {
        public int checklistId { get; set; }
        public string description { get; set; }
        public bool isMandatory { get; set; }
        public int categoryId { get; set; }

    }


    public class CombinedLoanBatchDisburseCheckListModel
    {
        //Disbursement
        public string loanNumberWithName { get; set; }
        public double amountDisbursed { get; set; }
        public int? bankId { get; set; }
        public string chequeNumber { get; set; }
        public int paymentModeId { get; set; }
        public DateTime disbursementDate { get; set; }
       
        //checklist
        public int loanId { get; set; }
        public string clientFullName { get; set; }
        public string loanNumber { get; set; }
        public double amountRequested { get; set; }

        public DateTime approvalDate { get; set; }
        public double amountApproved { get; set; }
        public bool approved { get; set; }
        public string groupName { get; set; }
        public int clientId { get; set; }
        public DateTime groupAddedDate { get; set; }
        public List<LoansCheckListModel> checkListItems { get; set; }

        public string BranchName { get; set; }

    }

}