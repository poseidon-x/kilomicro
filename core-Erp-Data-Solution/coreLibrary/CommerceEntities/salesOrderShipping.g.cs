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
    
    public partial class salesOrderShipping
    {
        public long salesOrderShippingId { get; set; }
        public long salesOrderId { get; set; }
        public int shippingMethodId { get; set; }
        public string shipTo { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string cityName { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
        public string countryName { get; set; }
    
        public virtual salesOrder salesOrder { get; set; }
        public virtual shippingMethod shippingMethod { get; set; }
    }
}