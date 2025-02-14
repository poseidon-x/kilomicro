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
    
    public partial class cashierTransactionReceipt
    {
        public cashierTransactionReceipt()
        {
            this.cashierTransactionReceiptCurrencies = new HashSet<cashierTransactionReceiptCurrency>();
        }
    
        public int cashierTransactionReceiptId { get; set; }
        public System.DateTime receiptDate { get; set; }
        public int transactionId { get; set; }
        public int transactionTypeId { get; set; }
        public double totalReceiptAmount { get; set; }
        public int cashierTillId { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
        public double balanceCD { get; set; }
        public double balanceBD { get; set; }
    
        public virtual cashiersTill cashiersTill { get; set; }
        public virtual transactionType transactionType { get; set; }
        public virtual ICollection<cashierTransactionReceiptCurrency> cashierTransactionReceiptCurrencies { get; set; }
    }
}
