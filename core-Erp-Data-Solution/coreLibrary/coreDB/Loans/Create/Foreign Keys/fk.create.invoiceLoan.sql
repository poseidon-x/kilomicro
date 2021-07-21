use coreDB
go

alter table ln.invoiceLoan add
	constraint fk_invoiceLoan_client foreign key (clientID)
	references ln.client(clientID)
go

alter table ln.invoiceLoan add
	constraint fk_invoiceLoan_supplier foreign key (supplierID)
	references ln.supplier(supplierID)
go

alter table ln.invoiceLoan add
	constraint fk_invoiceLoan_invoiceLoanMaster foreign key (invoiceLoanMasterID)
	references ln.invoiceLoanMaster(invoiceLoanMasterID)
go

alter table ln.invoiceLoanMaster add
	constraint fk_invoiceLoanMaster_client foreign key (clientID)
	references ln.client(clientID)
go

alter table ln.invoiceLoanConfig add
	constraint fk_invoiceLoanConfig_client foreign key (clientID)
	references ln.client(clientID)
go

alter table ln.invoiceLoanConfig add
	constraint fk_invoiceLoanConfig_supplier foreign key (supplierID)
	references ln.supplier(supplierID)
go
