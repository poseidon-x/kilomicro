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
    
    public partial class assetDepreciation
    {
        public int assetDepreciationID { get; set; }
        public int assetID { get; set; }
        public Nullable<System.DateTime> drepciationDate { get; set; }
        public int period { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public double assetValue { get; set; }
        public double depreciationAmount { get; set; }
        public double proposedAmount { get; set; }
        public byte[] version { get; set; }
    
        public virtual asset asset { get; set; }
    }
}
