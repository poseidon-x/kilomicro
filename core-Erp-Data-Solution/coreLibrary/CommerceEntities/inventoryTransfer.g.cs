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
    
    public partial class inventoryTransfer
    {
        public inventoryTransfer()
        {
            this.inventoryTransferDetails = new HashSet<inventoryTransferDetail>();
        }
    
        public int inventoryTransferId { get; set; }
        public int fromLocationId { get; set; }
        public int toLocationId { get; set; }
        public Nullable<System.DateTime> requisitionDate { get; set; }
        public bool approved { get; set; }
        public Nullable<System.DateTime> approvedDate { get; set; }
        public string approver { get; set; }
        public string enteredBy { get; set; }
        public bool delivered { get; set; }
        public Nullable<System.DateTime> deliveredDate { get; set; }
        public string receivedBy { get; set; }
        public bool posted { get; set; }
        public Nullable<System.DateTime> postedDate { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
        public string postedBy { get; set; }
        public string postingComments { get; set; }
        public string approvalComments { get; set; }
        public string deliveredBy { get; set; }
        public string deliveryCosmments { get; set; }
        public string deliveryComments { get; set; }
        public string receiptComments { get; set; }
        public Nullable<System.DateTime> receiptDate { get; set; }
    
        public virtual location location { get; set; }
        public virtual location location1 { get; set; }
        public virtual ICollection<inventoryTransferDetail> inventoryTransferDetails { get; set; }
    }
}