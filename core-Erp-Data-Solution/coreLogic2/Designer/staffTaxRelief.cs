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
    
    public partial class staffTaxRelief
    {
        public int staffTaxReliefID { get; set; }
        public int staffID { get; set; }
        public int taxReliefTypeID { get; set; }
        public double amount { get; set; }
        public bool isEnabled { get; set; }
    
        public virtual staff staff { get; set; }
        public virtual taxReliefType taxReliefType { get; set; }
    }
}
