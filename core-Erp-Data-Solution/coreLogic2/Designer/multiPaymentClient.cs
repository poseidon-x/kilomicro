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
    
    public partial class multiPaymentClient
    {
        public multiPaymentClient()
        {
            this.multiPayments = new HashSet<multiPayment>();
        }
    
        public int multiPaymentClientID { get; set; }
        public int cashierReceiptID { get; set; }
        public string clientName { get; set; }
        public double amount { get; set; }
        public System.DateTime invoiceDate { get; set; }
        public bool approved { get; set; }
        public bool refunded { get; set; }
        public double balance { get; set; }
        public byte[] version { get; set; }
        public double checkAmount { get; set; }
        public bool posted { get; set; }
    
        public virtual cashierReceipt cashierReceipt { get; set; }
        public virtual ICollection<multiPayment> multiPayments { get; set; }
    }
}
