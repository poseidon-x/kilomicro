use coreDB
go

alter table gl.pc_head add
	constraint fk_pc_head_accts foreign key (pc_acct_id)
	references accts(acct_id) on delete no action on update no action 
go
	 
alter table gl.pc_head add
	constraint fk_pc_head_currency foreign key (currency_id)
	references currencies(currency_id) on update cascade 
go
	 