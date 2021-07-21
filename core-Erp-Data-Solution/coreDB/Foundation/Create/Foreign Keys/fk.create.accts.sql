use coreDB
go

alter table accts add
	constraint fk_acct__acct_head foreign key (acct_head_id)
	references acct_heads(acct_head_id) on delete cascade on update cascade 
go
	
alter table accts add
	constraint fk_acct_cur foreign key (currency_id)
	references currencies(currency_id)  
go
	