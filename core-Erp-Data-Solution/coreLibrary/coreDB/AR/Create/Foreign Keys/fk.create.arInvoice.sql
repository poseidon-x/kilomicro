use coreDB
go

alter table ar.arInvoice add
	constraint fk_arInvoice_customer foreign key (customerId)
	references crm.customer (customerId)
go

alter table ar.arInvoice add
	constraint fk_arInvoice_currency foreign key (currencyId)
	references dbo.currencies (currency_id)
go

alter table ar.arInvoice add constraint fk_arInvoice_salesOrder 
	foreign key (salesOrderID) references so.salesOrder (salesOrderID)
go

alter table ar.arInvoice add constraint fk_arInvoice_invoiceStatus 
	foreign key (invoiceStatusID) references ar.invoiceStatus (invoiceStatusID)
go

alter table ar.arInvoice add constraint fk_arInvoice_paymentTerm 
	foreign key (paymentTermID) references ar.paymentTerm (paymentTermID)
go

alter table ar.arInvoice add constraint fk_arInvoice_custs 
	foreign key (customerId) references crm.customer (customerId)
go

alter table ar.arInvoiceLine add constraint fk_arInvoiceLine_arInvoice
	foreign key (arInvoiceID) references ar.arInvoice (arInvoiceID)
go

alter table ar.arInvoiceLine add constraint fk_arInvoiceLine_inventoryItem
	foreign key (inventoryItemID) references iv.inventoryItem (inventoryItemID)
go
