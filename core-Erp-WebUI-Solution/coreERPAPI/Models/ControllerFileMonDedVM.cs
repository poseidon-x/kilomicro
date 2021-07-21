using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class ControllerFileMonDedVM
    {
        public double amountDisbursed { get; set; }
        public bool authorized { get; set; }
        public double balBF { get; set; }
        public int clientID { get; set; }
        public string employeeName { get; set; }
        public DateTime? disbursementDate { get; set; }
        public bool duplicate { get; set; }
        public int fileDetailID { get; set; }
        public int fileID { get; set; }
        public int? LoanCount { get; set; }
        public int loanID { get; set; }
        public string loanNo { get; set; }
        public string managementUnit { get; set; }
        public double monthlyDeduction { get; set; }
        public bool notFound { get; set; }
        public string oldID { get; set; }
        public double? origAmt { get; set; }
        public double overage { get; set; }
        public long? RecordNumber { get; set; }
        public bool refunded { get; set; }
        public string remarks { get; set; }
        public int? repaymentScheduleID { get; set; }
        public string staffID { get; set; }
    }
}