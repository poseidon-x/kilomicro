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
    
    public partial class staffLoan
    {
        public staffLoan()
        {
            this.payMasterLoans = new HashSet<payMasterLoan>();
            this.staffLoanRepayments = new HashSet<staffLoanRepayment>();
            this.staffLoanSchedules = new HashSet<staffLoanSchedule>();
        }
    
        public int staffLoanID { get; set; }
        public int loanTypeID { get; set; }
        public int staffID { get; set; }
        public double principal { get; set; }
        public double principalBalance { get; set; }
        public bool attractsInterest { get; set; }
        public double rate { get; set; }
        public Nullable<System.DateTime> approvedDate { get; set; }
        public Nullable<System.DateTime> disbursementDate { get; set; }
        public double interestAccumulated { get; set; }
        public double interestBalance { get; set; }
        public System.DateTime deductionStartsDate { get; set; }
        public string memo { get; set; }
        public Nullable<int> bankID { get; set; }
        public string checkNo { get; set; }
        public bool posted { get; set; }
        public string approvedBy { get; set; }
        public string enteredBy { get; set; }
        public System.DateTime creationDate { get; set; }
        public byte[] version { get; set; }
    
        public virtual ICollection<payMasterLoan> payMasterLoans { get; set; }
        public virtual staffLoanType staffLoanType { get; set; }
        public virtual ICollection<staffLoanRepayment> staffLoanRepayments { get; set; }
        public virtual ICollection<staffLoanSchedule> staffLoanSchedules { get; set; }
        public virtual staff staff { get; set; }
    }
}
