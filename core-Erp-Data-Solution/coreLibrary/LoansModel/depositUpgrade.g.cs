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
    
    public partial class depositUpgrade
    {
        public int depositUpgradeId { get; set; }
        public System.DateTime upgradeDate { get; set; }
        public int upgradeDepositTypeID { get; set; }
        public int previousDepositTypeID { get; set; }
        public int previousDepositId { get; set; }
        public int newDepositId { get; set; }
        public double balanceCD { get; set; }
        public double topUpAmount { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
        public double annualInterestRate { get; set; }
        public int depositPeriodInDays { get; set; }
        public System.DateTime maturityDate { get; set; }
        public int topupPaymentModeId { get; set; }
        public Nullable<int> topupBankId { get; set; }
        public string topupCheckNo { get; set; }
        public double interestRate { get; set; }
    
        public virtual deposit deposit { get; set; }
        public virtual deposit deposit1 { get; set; }
        public virtual depositType depositType { get; set; }
        public virtual depositType depositType1 { get; set; }
        public virtual modeOfPayment modeOfPayment { get; set; }
    }
}
