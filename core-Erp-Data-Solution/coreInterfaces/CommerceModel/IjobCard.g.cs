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
    
    public interface IjobCard
    {
        long jobCardId { get; set; }
        string jobNumber { get; set; }
        Nullable<long> revisionNumber { get; set; }
        System.DateTime jobDate { get; set; }
        System.DateTime orderStartingDate { get; set; }
        long customerId { get; set; }
        string workOrderNumber { get; set; }
        double standardMarkUpRate { get; set; }
        double standardHourlyBillingRate { get; set; }
        bool invoiced { get; set; }
        bool fulfilled { get; set; }
        bool signed { get; set; }
        bool approved { get; set; }
        double totalLabour { get; set; }
        long totalMaterial { get; set; }
        double vat { get; set; }
        double nhil { get; set; }
        string creator { get; set; }
        System.DateTime created { get; set; }
        string modifier { get; set; }
        System.DateTime modified { get; set; }
        string approvedBy { get; set; }
        Nullable<System.DateTime> approvelDate { get; set; }
        string invoicedBy { get; set; }
        Nullable<System.DateTime> invoiceDate { get; set; }
        string signedBy { get; set; }
        Nullable<System.DateTime> signDate { get; set; }
        string fulfilledBy { get; set; }
        Nullable<System.DateTime> fulfillmentDate { get; set; }
    
        Icustomer customer { get; set; }
        ICollection<IjobCardLabourDetail> jobCardLabourDetails { get; set; }
        ICollection<IjobCardMaterialDetail> jobCardMaterialDetails { get; set; }
    } 
}
