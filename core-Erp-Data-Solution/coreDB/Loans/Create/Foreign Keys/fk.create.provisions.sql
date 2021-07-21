use coreDB
go

alter table ln.loanProvision add
	constraint fk_loanProvision_loan foreign key (loanID)
	references ln.loan(loanID)
go

alter table ln.loanProvision add
	constraint fk_loanProvision_provisionClass foreign key (provisionClassID)
	references ln.provisionClass(provisionClassID)
go
