use coreDB
go

alter table gl.v_head add
	constraint fk_v_bank_acct_id foreign key (bank_acct_id)
	references accts(acct_id) on delete no action on update no action 
go
	 
alter table gl.v_head add
	constraint fk_v_head_currency foreign key (currency_id)
	references currencies(currency_id) on update cascade 
go
	  
alter table gl.v_head add
	constraint fk_v_head_cust foreign key (cust_id)
	references custs(cust_id)  on delete no action on update no action 
go
	   
alter table gl.v_head add
	constraint fk_v_head_sup foreign key (sup_id)
	references sups(sup_id) on delete no action on update no action 
go
	 