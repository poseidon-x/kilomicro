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
    
    public partial class repaymentMode
    {
        public repaymentMode()
        {
            this.loans = new HashSet<loan>();
            this.loanFinancials = new HashSet<loanFinancial>();
        }
    
        public int repaymentModeID { get; set; }
        public string repaymentModeName { get; set; }
    
        public virtual ICollection<loan> loans { get; set; }
        public virtual ICollection<loanFinancial> loanFinancials { get; set; }
    }
}