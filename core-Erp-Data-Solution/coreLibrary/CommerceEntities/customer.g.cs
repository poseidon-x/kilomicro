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
    
    public partial class customer
    {
        public customer()
        {
            this.arInvoices = new HashSet<arInvoice>();
            this.arInvoices1 = new HashSet<arInvoice>();
            this.customerBusinessAddresses = new HashSet<customerBusinessAddress>();
            this.customerContactPersons = new HashSet<customerContactPerson>();
            this.customerEmails = new HashSet<customerEmail>();
            this.customerPhones = new HashSet<customerPhone>();
            this.customerShippingAddresses = new HashSet<customerShippingAddress>();
            this.salesOrders = new HashSet<salesOrder>();
            this.arPayments = new HashSet<arPayment>();
            this.workOrders = new HashSet<workOrder>();
            this.jobCards = new HashSet<jobCard>();
        }
    
        public long customerId { get; set; }
        public string customerNumber { get; set; }
        public string customerName { get; set; }
        public int paymentTermID { get; set; }
        public int currencyId { get; set; }
        public double balance { get; set; }
        public double balanceLocal { get; set; }
        public string contactPersonName { get; set; }
        public int glAccountId { get; set; }
        public int locationId { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual ICollection<arInvoice> arInvoices { get; set; }
        public virtual ICollection<arInvoice> arInvoices1 { get; set; }
        public virtual ICollection<customerBusinessAddress> customerBusinessAddresses { get; set; }
        public virtual ICollection<customerContactPerson> customerContactPersons { get; set; }
        public virtual ICollection<customerEmail> customerEmails { get; set; }
        public virtual ICollection<customerPhone> customerPhones { get; set; }
        public virtual ICollection<customerShippingAddress> customerShippingAddresses { get; set; }
        public virtual ICollection<salesOrder> salesOrders { get; set; }
        public virtual ICollection<arPayment> arPayments { get; set; }
        public virtual ICollection<workOrder> workOrders { get; set; }
        public virtual ICollection<jobCard> jobCards { get; set; }
    }
}
