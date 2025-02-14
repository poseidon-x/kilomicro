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
    
    public partial class repaymentSchedule
    {
        public repaymentSchedule()
        {
            this.controllerFileDetails = new HashSet<controllerFileDetail>();
        }
    
        public int repaymentScheduleID { get; set; }
        public int loanID { get; set; }
        public System.DateTime repaymentDate { get; set; }
        public double interestPayment { get; set; }
        public double principalPayment { get; set; }
        public double balanceBF { get; set; }
        public double balanceCD { get; set; }
        public double interestBalance { get; set; }
        public double principalBalance { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public double proposedInterestWriteOff { get; set; }
        public double interestWritenOff { get; set; }
        public bool edited { get; set; }
        public double origInterestPayment { get; set; }
        public double additionalInterest { get; set; }
        public double origPrincipalCD { get; set; }
        public double origPrincipalBF { get; set; }
        public double penaltyAmount { get; set; }
        public Nullable<double> origPrincipalPayment { get; set; }
        public Nullable<double> additionalInterestBalance { get; set; }
        public byte[] version { get; set; }
    
        public virtual loan loan { get; set; }
        public virtual ICollection<controllerFileDetail> controllerFileDetails { get; set; }
    }
}
