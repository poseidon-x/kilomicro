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
    
    public partial class regularSusuAccount
    {
        public regularSusuAccount()
        {
            this.regularSusuContributions = new HashSet<regularSusuContribution>();
            this.regularSusuContributionSchedules = new HashSet<regularSusuContributionSchedule>();
            this.regularSusuWithdrawals = new HashSet<regularSusuWithdrawal>();
        }
    
        public int regularSusuAccountID { get; set; }
        public int clientID { get; set; }
        public System.DateTime applicationDate { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public double contributionRate { get; set; }
        public Nullable<System.DateTime> dueDate { get; set; }
        public Nullable<System.DateTime> disbursementDate { get; set; }
        public double amountEntitled { get; set; }
        public double amountTaken { get; set; }
        public double balance { get; set; }
        public Nullable<System.DateTime> approvalDate { get; set; }
        public string enteredBy { get; set; }
        public string verifiedBy { get; set; }
        public string approvedBy { get; set; }
        public Nullable<int> staffID { get; set; }
        public Nullable<int> agentID { get; set; }
        public Nullable<int> loanID { get; set; }
        public double contributionAmount { get; set; }
        public string disbursedBy { get; set; }
        public Nullable<System.DateTime> entitledDate { get; set; }
        public string regularSusuAccountNo { get; set; }
        public Nullable<int> regularSusuGroupID { get; set; }
        public double netAmountEntitled { get; set; }
        public double interestAmount { get; set; }
        public Nullable<int> modeOfPaymentID { get; set; }
        public string checkNo { get; set; }
        public Nullable<int> bankID { get; set; }
        public bool posted { get; set; }
        public bool authorized { get; set; }
        public double regularSusCommissionAmount { get; set; }
        public bool exited { get; set; }
        public Nullable<System.DateTime> exitDate { get; set; }
        public string exitApprovedBy { get; set; }
        public double commissionPaid { get; set; }
        public double interestPaid { get; set; }
        public double principalPaid { get; set; }
        public Nullable<System.DateTime> lastEOD { get; set; }
        public bool isDormant { get; set; }
        public bool convertedToLoan { get; set; }
        public bool appliedToLoan { get; set; }
        public double interestAmountApplied { get; set; }
        public byte[] version { get; set; }
    
        public virtual staff staff { get; set; }
        public virtual agent agent { get; set; }
        public virtual client client { get; set; }
        public virtual loan loan { get; set; }
        public virtual ICollection<regularSusuContribution> regularSusuContributions { get; set; }
        public virtual ICollection<regularSusuContributionSchedule> regularSusuContributionSchedules { get; set; }
        public virtual ICollection<regularSusuWithdrawal> regularSusuWithdrawals { get; set; }
    }
}