//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic.Designer
{
    using System;
    using System.Collections.Generic;
    
    public partial class staffLoanRepayment
    {
        public int staffLoanScheduleID { get; set; }
        public int staffLoanID { get; set; }
        public double principalPaid { get; set; }
        public double interestPaid { get; set; }
        public System.DateTime repaymentDate { get; set; }
        public double balanceAfter { get; set; }
        public string repaymentType { get; set; }
        public string enteredBy { get; set; }
        public System.DateTime creationDate { get; set; }
        public string checkedBy { get; set; }
        public byte[] version { get; set; }
    
        public virtual staffLoan staffLoan { get; set; }
    }
}