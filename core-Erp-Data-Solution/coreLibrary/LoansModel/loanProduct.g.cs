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
    
    public partial class loanProduct
    {
        public loanProduct()
        {
            this.collections = new HashSet<collection>();
            this.loanProductHistories = new HashSet<loanProductHistory>();
            this.prLoanDetails = new HashSet<prLoanDetail>();
        }
    
        public int loanProductID { get; set; }
        public string loanProductName { get; set; }
        public double loanTenure { get; set; }
        public double rate { get; set; }
        public double minAge { get; set; }
        public double maxAge { get; set; }
        public double procFeeRate { get; set; }
    
        public virtual ICollection<collection> collections { get; set; }
        public virtual ICollection<loanProductHistory> loanProductHistories { get; set; }
        public virtual ICollection<prLoanDetail> prLoanDetails { get; set; }
    }
}