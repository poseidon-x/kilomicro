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
    
    public partial class CommerceEntities : DbContext, ICommerceEntities
    {
        public CommerceEntities()
            : base("name=CommerceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual IDbSet<arInvoice> arInvoices { get; set; }
        public virtual IDbSet<arInvoiceLine> arInvoiceLines { get; set; }
        public virtual IDbSet<arPayment> arPayments { get; set; }
        public virtual IDbSet<arPaymentLine> arPaymentLines { get; set; }
        public virtual IDbSet<creditMemo> creditMemoes { get; set; }
        public virtual IDbSet<creditMemoReason> creditMemoReasons { get; set; }
        public virtual IDbSet<invoiceStatu> invoiceStatus { get; set; }
        public virtual IDbSet<paymentTerm> paymentTerms { get; set; }
        public virtual IDbSet<customer> customers { get; set; }
        public virtual IDbSet<customerBusinessAddress> customerBusinessAddresses { get; set; }
        public virtual IDbSet<customerContactPerson> customerContactPersons { get; set; }
        public virtual IDbSet<customerEmail> customerEmails { get; set; }
        public virtual IDbSet<customerPhone> customerPhones { get; set; }
        public virtual IDbSet<customerShippingAddress> customerShippingAddresses { get; set; }
        public virtual IDbSet<brand> brands { get; set; }
        public virtual IDbSet<inventoryItem> inventoryItems { get; set; }
        public virtual IDbSet<inventoryItemDetail> inventoryItemDetails { get; set; }
        public virtual IDbSet<inventoryMethod> inventoryMethods { get; set; }
        public virtual IDbSet<inventoryTransfer> inventoryTransfers { get; set; }
        public virtual IDbSet<inventoryTransferDetail> inventoryTransferDetails { get; set; }
        public virtual IDbSet<inventoryTransferDetailLine> inventoryTransferDetailLines { get; set; }
        public virtual IDbSet<location> locations { get; set; }
        public virtual IDbSet<locationType> locationTypes { get; set; }
        public virtual IDbSet<openningBalance> openningBalances { get; set; }
        public virtual IDbSet<openningBalanceBatch> openningBalanceBatches { get; set; }
        public virtual IDbSet<product> products { get; set; }
        public virtual IDbSet<productCategory> productCategories { get; set; }
        public virtual IDbSet<productMake> productMakes { get; set; }
        public virtual IDbSet<productStatu> productStatus { get; set; }
        public virtual IDbSet<productSubCategory> productSubCategories { get; set; }
        public virtual IDbSet<shrinkage> shrinkages { get; set; }
        public virtual IDbSet<shrinkageBatch> shrinkageBatches { get; set; }
        public virtual IDbSet<shrinkageReason> shrinkageReasons { get; set; }
        public virtual IDbSet<unitOfMeasurement> unitOfMeasurements { get; set; }
        public virtual IDbSet<paymentMethod> paymentMethods { get; set; }
        public virtual IDbSet<salesOrder> salesOrders { get; set; }
        public virtual IDbSet<salesOrderBilling> salesOrderBillings { get; set; }
        public virtual IDbSet<salesOrderline> salesOrderlines { get; set; }
        public virtual IDbSet<salesOrderShipping> salesOrderShippings { get; set; }
        public virtual IDbSet<salesType> salesTypes { get; set; }
        public virtual IDbSet<shippingMethod> shippingMethods { get; set; }
        public virtual IDbSet<creditMemoLine> creditMemoLines { get; set; }
        public virtual IDbSet<workOrder> workOrders { get; set; }
        public virtual IDbSet<speciality> specialities { get; set; }
        public virtual IDbSet<specialityCategory> specialityCategories { get; set; }
        public virtual IDbSet<workOrderActivity> workOrderActivities { get; set; }
        public virtual IDbSet<jobCard> jobCards { get; set; }
        public virtual IDbSet<jobCardLabourDetail> jobCardLabourDetails { get; set; }
        public virtual IDbSet<jobCardMaterialDetail> jobCardMaterialDetails { get; set; }
        public virtual IDbSet<actualPerfomance> actualPerfomances { get; set; }
        public virtual IDbSet<assemblyLine> assemblyLines { get; set; }
        public virtual IDbSet<assemblyLineStaff> assemblyLineStaffs { get; set; }
        public virtual IDbSet<assemblyLineType> assemblyLineTypes { get; set; }
        public virtual IDbSet<assemblyWorkStage> assemblyWorkStages { get; set; }
        public virtual IDbSet<billOfMaterial> billOfMaterials { get; set; }
        public virtual IDbSet<billOfMaterialDetail> billOfMaterialDetails { get; set; }
        public virtual IDbSet<durationType> durationTypes { get; set; }
        public virtual IDbSet<factory> factories { get; set; }
        public virtual IDbSet<factoryType> factoryTypes { get; set; }
        public virtual IDbSet<manufacturingCalender> manufacturingCalenders { get; set; }
        public virtual IDbSet<manufacturingCalenderStaff> manufacturingCalenderStaffs { get; set; }
        public virtual IDbSet<manufacturingScrap> manufacturingScraps { get; set; }
        public virtual IDbSet<scrapReason> scrapReasons { get; set; }
        public virtual IDbSet<workStageType> workStageTypes { get; set; }
    }
}