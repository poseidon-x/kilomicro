use coreDB
go

alter table so.salesOrder add
	constraint fk_salesOrder_customer foreign key (customerId)
	references crm.customer (customerId)
go

alter table so.salesOrder add
	constraint fk_salesOrder_currency foreign key (currencyId)
	references dbo.currencies (currency_id)
go

alter table so.salesOrder add
	constraint fk_salesOrder_location foreign key (locationID)
	references iv.location (locationID)
go

alter table so.salesOrder add
	constraint fk_salesOrder_salesType foreign key (salesTypeID)
	references so.salesType (salesTypeID)
go

alter table so.salesOrderLine add
	constraint fk_salesOrderLine_salesOrder foreign key (salesOrderID)
	references so.salesOrder (salesOrderID)
go

alter table so.salesOrderLine add
	constraint fk_salesOrderLine_inventoryItem foreign key (inventoryItemID)
	references iv.inventoryItem (inventoryItemID)
go

alter table so.salesOrderLine add
	constraint fk_salesOrderLine_accts foreign key (accountId)
	references dbo.accts (acct_id)
go

alter table so.salesOrderLine add
	constraint fk_salesOrderLine_unitOfMeasurement foreign key (unitOfMeasurementID)
	references iv.unitOfMeasurement (unitOfMeasurementID)
go

alter table so.salesOrderShipping add
	constraint fk_salesOrderShipping_salesOrder foreign key (salesOrderID)
	references so.salesOrder (salesOrderID)
go

alter table so.salesOrderShipping add
	constraint fk_salesOrderShipping_shippingMethod foreign key (shippingMethodID)
	references so.shippingMethod (shippingMethodID)
go

alter table so.salesOrderBilling add
	constraint fk_salesOrderBilling_salesOrder foreign key (salesOrderID)
	references so.salesOrder (salesOrderID)
go
