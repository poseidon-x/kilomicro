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
    
    public partial class jobCard
    {
        public jobCard()
        {
            this.jobCardLabourDetails = new HashSet<jobCardLabourDetail>();
            this.jobCardMaterialDetails = new HashSet<jobCardMaterialDetail>();
        }
    
        public long jobCardId { get; set; }
        public string jobNumber { get; set; }
        public Nullable<long> revisionNumber { get; set; }
        public System.DateTime jobDate { get; set; }
        public System.DateTime orderStartingDate { get; set; }
        public long customerId { get; set; }
        public string workOrderNumber { get; set; }
        public double standardMarkUpRate { get; set; }
        public double standardHourlyBillingRate { get; set; }
        public bool invoiced { get; set; }
        public bool fulfilled { get; set; }
        public bool signed { get; set; }
        public bool approved { get; set; }
        public double totalLabour { get; set; }
        public long totalMaterial { get; set; }
        public double vat { get; set; }
        public double nhil { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
        public string approvedBy { get; set; }
        public Nullable<System.DateTime> approvelDate { get; set; }
        public string invoicedBy { get; set; }
        public Nullable<System.DateTime> invoiceDate { get; set; }
        public string signedBy { get; set; }
        public Nullable<System.DateTime> signDate { get; set; }
        public string fulfilledBy { get; set; }
        public Nullable<System.DateTime> fulfillmentDate { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual ICollection<jobCardLabourDetail> jobCardLabourDetails { get; set; }
        public virtual ICollection<jobCardMaterialDetail> jobCardMaterialDetails { get; set; }
    }
}
