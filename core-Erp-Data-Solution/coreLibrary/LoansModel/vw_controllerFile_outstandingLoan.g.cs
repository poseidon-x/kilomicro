//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_controllerFile_outstandingLoan
    {
        public int clientID { get; set; }
        public int loanID { get; set; }
        public string staffID { get; set; }
        public int fileDetailID { get; set; }
        public int fileID { get; set; }
        public string managementUnit { get; set; }
        public string oldID { get; set; }
        public double balBF { get; set; }
        public string employeeName { get; set; }
        public double monthlyDeduction { get; set; }
        public Nullable<double> origAmt { get; set; }
        public Nullable<int> repaymentScheduleID { get; set; }
        public bool authorized { get; set; }
        public bool duplicate { get; set; }
        public bool refunded { get; set; }
        public bool notFound { get; set; }
        public double overage { get; set; }
        public string remarks { get; set; }
        public string loanNo { get; set; }
        public double amountDisbursed { get; set; }
        public Nullable<System.DateTime> disbursementDate { get; set; }
        public Nullable<long> RecordNumber { get; set; }
        public Nullable<int> LoanCount { get; set; }
    }
}
