use coreDB
go

alter table ln.clientActivityLog add
	constraint fk_clientActivityLog_clientActivityType foreign key (clientActivityTypeID)
	references ln.clientActivityType(clientActivityTypeID)
go

alter table ln.clientActivityLog add
	constraint fk_clientActivityLog_client foreign key (clientID)
	references ln.client(clientID)
go

alter table ln.clientActivityLog add
	constraint fk_clientActivityLog_loan foreign key (loanID)
	references ln.loan(loanID)
go

alter table ln.clientActivityLog add
	constraint fk_clientActivityLog_staff foreign key (responsibleStaffID)
	references fa.staff(staffID)
go
