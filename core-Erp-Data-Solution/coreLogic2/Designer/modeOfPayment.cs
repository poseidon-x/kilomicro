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
    
    public partial class modeOfPayment
    {
        public modeOfPayment()
        {
            this.loanRepayments = new HashSet<loanRepayment>();
            this.loanTranches = new HashSet<loanTranch>();
            this.depositAdditionals = new HashSet<depositAdditional>();
            this.depositWithdrawals = new HashSet<depositWithdrawal>();
            this.cashierDisbursements = new HashSet<cashierDisbursement>();
            this.cashierReceipts = new HashSet<cashierReceipt>();
            this.investmentAdditionals = new HashSet<investmentAdditional>();
            this.investmentWithdrawals = new HashSet<investmentWithdrawal>();
            this.savingAdditionals = new HashSet<savingAdditional>();
            this.savingWithdrawals = new HashSet<savingWithdrawal>();
            this.susuContributions = new HashSet<susuContribution>();
            this.regularSusuContributions = new HashSet<regularSusuContribution>();
        }
    
        public int modeOfPaymentID { get; set; }
        public string modeOfPaymentName { get; set; }
    
        public virtual ICollection<loanRepayment> loanRepayments { get; set; }
        public virtual ICollection<loanTranch> loanTranches { get; set; }
        public virtual ICollection<depositAdditional> depositAdditionals { get; set; }
        public virtual ICollection<depositWithdrawal> depositWithdrawals { get; set; }
        public virtual ICollection<cashierDisbursement> cashierDisbursements { get; set; }
        public virtual ICollection<cashierReceipt> cashierReceipts { get; set; }
        public virtual ICollection<investmentAdditional> investmentAdditionals { get; set; }
        public virtual ICollection<investmentWithdrawal> investmentWithdrawals { get; set; }
        public virtual ICollection<savingAdditional> savingAdditionals { get; set; }
        public virtual ICollection<savingWithdrawal> savingWithdrawals { get; set; }
        public virtual ICollection<susuContribution> susuContributions { get; set; }
        public virtual ICollection<regularSusuContribution> regularSusuContributions { get; set; }
    }
}