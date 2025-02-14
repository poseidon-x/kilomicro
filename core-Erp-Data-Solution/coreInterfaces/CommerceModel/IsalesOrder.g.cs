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
    
    public interface IsalesOrder
    {
        long salesOrderId { get; set; }
        Nullable<long> customerId { get; set; }
        string customerName { get; set; }
        string orderNumber { get; set; }
        System.DateTime salesDate { get; set; }
        double totalAmount { get; set; }
        double balance { get; set; }
        System.DateTime requiredDate { get; set; }
        Nullable<System.DateTime> shippedDate { get; set; }
        Nullable<int> locationId { get; set; }
        int salesTypeId { get; set; }
        int currencyId { get; set; }
        double buyRate { get; set; }
        double sellRate { get; set; }
        double totalAmountLocal { get; set; }
        double balanceLocal { get; set; }
        Nullable<int> accountId { get; set; }
        int paymentTermId { get; set; }
        string creator { get; set; }
        System.DateTime created { get; set; }
        string modifier { get; set; }
        System.DateTime modified { get; set; }
        Nullable<bool> isInvoiced { get; set; }
        Nullable<System.DateTime> invoicedDate { get; set; }
    
        ICollection<IarInvoice> arInvoices { get; set; }
        Icustomer customer { get; set; }
        IsalesType salesType { get; set; }
        ICollection<IsalesOrderBilling> salesOrderBillings { get; set; }
        ICollection<IsalesOrderline> salesOrderlines { get; set; }
        ICollection<IsalesOrderShipping> salesOrderShippings { get; set; }
    } 
}
