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
    
    public partial class performanceContractTarget
    {
        public int performanceContractTargetID { get; set; }
        public int performanceContractItemID { get; set; }
        public int performanceScoreID { get; set; }
        public string targetCreteria { get; set; }
    
        public virtual performanceContractItem performanceContractItem { get; set; }
        public virtual performanceScore performanceScore { get; set; }
    }
}
