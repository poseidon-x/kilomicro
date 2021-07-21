use coreDB
go

alter table ln.[collection] add
	constraint fk_collection_loanProduct foreign key (loanProductID)
	references ln.loanProduct (loanProductID)
go