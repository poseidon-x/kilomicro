use coreDB
go

alter table ln.supplierContact add
	constraint fk_supplierCOntact_supplier foreign key(supplierID)
	references ln.supplier(supplierID)
go
