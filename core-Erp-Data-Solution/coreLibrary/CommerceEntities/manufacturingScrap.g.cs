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
    
    public partial class manufacturingScrap
    {
        public int manufacturingScrapId { get; set; }
        public int actualPerfomanceId { get; set; }
        public double quantity { get; set; }
        public int unitOfMeasurementId { get; set; }
        public int scrapReasonId { get; set; }
        public System.DateTime scrapDate { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual unitOfMeasurement unitOfMeasurement { get; set; }
        public virtual actualPerfomance actualPerfomance { get; set; }
        public virtual scrapReason scrapReason { get; set; }
    }
}
