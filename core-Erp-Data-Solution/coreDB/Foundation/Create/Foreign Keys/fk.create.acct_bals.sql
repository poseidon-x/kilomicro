use coreDB
go

alter table dbo.acct_bals add
	constraint fk_acct_bal_acct foreign key (acct_id)
	references dbo.accts(acct_id) on delete cascade on update cascade 
go
	
alter table dbo.acct_bals add
	constraint fk_acct_bal_acct_period foreign key (acct_period)
	references dbo.acct_period(acct_period) on delete cascade on update cascade 
go
	