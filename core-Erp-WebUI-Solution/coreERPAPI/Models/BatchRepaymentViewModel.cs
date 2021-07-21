using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class BatchRepaymentViewModel
    {
        public string clientName { get; set; }
        public string clientAccNum { get; set; }
        public int clientID { get; set; }

        public int loanId { get; set; }
        public string loanNo { get; set; }
        public int repaymentScheduleId { get; set; }
        public DateTime scheduleDate { get; set; }
        public string loanNumberWithClient { get; set; }
        public double amountDisbursed { get; set; }
        public double principalBalance { get; set; }
        public int paymentTypeId { get; set; }
        public double interestAmount { get; set; }
        public double principalAmount { get; set; }
        public double amountDue { get; set; }
        public double paymentAmount { get; set; }
        public int paymentModeId { get; set; }
        public int? bankId { get; set; }
        public string chequeNumber { get; set; }
        public double cashCollateral { get; set; }
        public int? branchID { get; set; }
    }
}