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
    
    public partial class arInvoiceLine
    {
        public arInvoiceLine()
        {
            this.creditMemoLines = new HashSet<creditMemoLine>();
        }
    
        public long arInvoiceLineId { get; set; }
        public long arInvoiceId { get; set; }
        public long inventoryItemId { get; set; }
        public int lineNumber { get; set; }
        public string description { get; set; }
        public Nullable<int> acct_id { get; set; }
        public double unitPrice { get; set; }
        public double quantity { get; set; }
        public int unitOfMeasurementId { get; set; }
        public double discountAmount { get; set; }
        public double discountPercentage { get; set; }
        public double totalAmount { get; set; }
        public double netAmount { get; set; }
        public bool isVat { get; set; }
        public bool isNHIL { get; set; }
        public bool isWith { get; set; }
        public double unitPriceLocal { get; set; }
        public double discountAmountLocal { get; set; }
        public double totalAmountLocal { get; set; }
        public double netAmountLocal { get; set; }
        public int accountId { get; set; }
        public int discountAccountId { get; set; }
        public int vatAccountId { get; set; }
        public int nhilAccountId { get; set; }
        public int withAccountId { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual arInvoice arInvoice { get; set; }
        public virtual ICollection<creditMemoLine> creditMemoLines { get; set; }
    }
}
