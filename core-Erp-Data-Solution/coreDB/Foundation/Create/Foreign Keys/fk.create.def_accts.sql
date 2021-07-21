use coreDB
go

alter table def_accts add
	constraint fk_def_accts foreign key (acct_id)
	references accts(acct_id) on delete cascade on update cascade 
go
	