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
    
    public partial class staffDeduction
    {
        public int staffDeductionID { get; set; }
        public int staffID { get; set; }
        public int deductionTypeID { get; set; }
        public double amount { get; set; }
        public double percentValue { get; set; }
        public bool isEnabled { get; set; }
        public byte[] version { get; set; }
    
        public virtual deductionType deductionType { get; set; }
        public virtual staff staff { get; set; }
    }
}
