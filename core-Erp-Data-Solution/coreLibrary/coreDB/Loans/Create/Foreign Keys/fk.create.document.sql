use coreDB
go

alter table ln.loanDocument add
	constraint fk_loanDocument_loan foreign key (loanID)
	references ln.loan(loanID)
go

alter table ln.loanDocument add
	constraint fk_loanDocument_document foreign key (documentID)
	references ln.document(documentID)
go


alter table ln.clientDocument add
	constraint fk_clientDocument_client foreign key (clientID)
	references ln.client(clientID)
go

alter table ln.clientDocument add
	constraint fk_clientDocument_document foreign key (documentID)
	references ln.document(documentID)
go