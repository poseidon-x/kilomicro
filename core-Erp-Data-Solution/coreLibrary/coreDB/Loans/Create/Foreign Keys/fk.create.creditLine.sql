use coreDB
go

alter table ln.creditLine add
	constraint fk_creditLine_client foreign key(clientId)
	references ln.client(clientID)

alter table ln.creditLine add
	constraint fk_creditLine_loan foreign key(loanId)
	references ln.loan(loanID)
go
