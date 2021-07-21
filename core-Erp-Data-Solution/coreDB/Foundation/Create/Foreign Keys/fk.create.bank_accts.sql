use coreDB
go

alter table bank_accts add
	constraint fk_bank_accts foreign key (branch_id)
	references bank_branches(branch_id) on delete cascade on update cascade 
go
	
alter table bank_accts add
	constraint fk_bank_accts_gl foreign key (gl_acct_id)
	references accts(acct_id) on delete cascade on update cascade 
go
	