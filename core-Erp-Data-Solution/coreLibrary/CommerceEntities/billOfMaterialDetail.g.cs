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
    
    public partial class billOfMaterialDetail
    {
        public int billOfMaterialDetailId { get; set; }
        public Nullable<int> billOfMaterialId { get; set; }
        public int productId { get; set; }
        public int unitOfMeasurementId { get; set; }
        public double quantity { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual product product { get; set; }
        public virtual unitOfMeasurement unitOfMeasurement { get; set; }
        public virtual billOfMaterial billOfMaterial { get; set; }
    }
}
