﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public interface ICommerceEntities : IDisposable
    {
    	int SaveChanges();
    	System.Data.Entity.Infrastructure.DbContextConfiguration Configuration {get;}
    	System.Data.Entity.Database Database {get;}
        IDbSet<arInvoice> arInvoices { get; set; }
        IDbSet<arInvoiceLine> arInvoiceLines { get; set; }
        IDbSet<arPayment> arPayments { get; set; }
        IDbSet<arPaymentLine> arPaymentLines { get; set; }
        IDbSet<creditMemo> creditMemoes { get; set; }
        IDbSet<creditMemoReason> creditMemoReasons { get; set; }
        IDbSet<invoiceStatu> invoiceStatus { get; set; }
        IDbSet<paymentTerm> paymentTerms { get; set; }
        IDbSet<customer> customers { get; set; }
        IDbSet<customerBusinessAddress> customerBusinessAddresses { get; set; }
        IDbSet<customerContactPerson> customerContactPersons { get; set; }
        IDbSet<customerEmail> customerEmails { get; set; }
        IDbSet<customerPhone> customerPhones { get; set; }
        IDbSet<customerShippingAddress> customerShippingAddresses { get; set; }
        IDbSet<brand> brands { get; set; }
        IDbSet<inventoryItem> inventoryItems { get; set; }
        IDbSet<inventoryItemDetail> inventoryItemDetails { get; set; }
        IDbSet<inventoryMethod> inventoryMethods { get; set; }
        IDbSet<inventoryTransfer> inventoryTransfers { get; set; }
        IDbSet<inventoryTransferDetail> inventoryTransferDetails { get; set; }
        IDbSet<inventoryTransferDetailLine> inventoryTransferDetailLines { get; set; }
        IDbSet<location> locations { get; set; }
        IDbSet<locationType> locationTypes { get; set; }
        IDbSet<openningBalance> openningBalances { get; set; }
        IDbSet<openningBalanceBatch> openningBalanceBatches { get; set; }
        IDbSet<product> products { get; set; }
        IDbSet<productCategory> productCategories { get; set; }
        IDbSet<productMake> productMakes { get; set; }
        IDbSet<productStatu> productStatus { get; set; }
        IDbSet<productSubCategory> productSubCategories { get; set; }
        IDbSet<shrinkage> shrinkages { get; set; }
        IDbSet<shrinkageBatch> shrinkageBatches { get; set; }
        IDbSet<shrinkageReason> shrinkageReasons { get; set; }
        IDbSet<unitOfMeasurement> unitOfMeasurements { get; set; }
        IDbSet<paymentMethod> paymentMethods { get; set; }
        IDbSet<salesOrder> salesOrders { get; set; }
        IDbSet<salesOrderBilling> salesOrderBillings { get; set; }
        IDbSet<salesOrderline> salesOrderlines { get; set; }
        IDbSet<salesOrderShipping> salesOrderShippings { get; set; }
        IDbSet<salesType> salesTypes { get; set; }
        IDbSet<shippingMethod> shippingMethods { get; set; }
        IDbSet<creditMemoLine> creditMemoLines { get; set; }
        IDbSet<workOrder> workOrders { get; set; }
        IDbSet<speciality> specialities { get; set; }
        IDbSet<specialityCategory> specialityCategories { get; set; }
        IDbSet<workOrderActivity> workOrderActivities { get; set; }
        IDbSet<jobCard> jobCards { get; set; }
        IDbSet<jobCardLabourDetail> jobCardLabourDetails { get; set; }
        IDbSet<jobCardMaterialDetail> jobCardMaterialDetails { get; set; }
        IDbSet<actualPerfomance> actualPerfomances { get; set; }
        IDbSet<assemblyLine> assemblyLines { get; set; }
        IDbSet<assemblyLineStaff> assemblyLineStaffs { get; set; }
        IDbSet<assemblyLineType> assemblyLineTypes { get; set; }
        IDbSet<assemblyWorkStage> assemblyWorkStages { get; set; }
        IDbSet<billOfMaterial> billOfMaterials { get; set; }
        IDbSet<billOfMaterialDetail> billOfMaterialDetails { get; set; }
        IDbSet<durationType> durationTypes { get; set; }
        IDbSet<factory> factories { get; set; }
        IDbSet<factoryType> factoryTypes { get; set; }
        IDbSet<manufacturingCalender> manufacturingCalenders { get; set; }
        IDbSet<manufacturingCalenderStaff> manufacturingCalenderStaffs { get; set; }
        IDbSet<manufacturingScrap> manufacturingScraps { get; set; }
        IDbSet<scrapReason> scrapReasons { get; set; }
        IDbSet<workStageType> workStageTypes { get; set; }
    }
}