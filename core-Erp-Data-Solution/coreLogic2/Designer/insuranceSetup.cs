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
    
    public partial class insuranceSetup
    {
        public int insuranceSetupID { get; set; }
        public int loanTypeID { get; set; }
        public double insurancePercent { get; set; }
        public Nullable<int> insuranceAccountID { get; set; }
        public bool isEnabled { get; set; }
    
        public virtual loanType loanType { get; set; }
    }
}
