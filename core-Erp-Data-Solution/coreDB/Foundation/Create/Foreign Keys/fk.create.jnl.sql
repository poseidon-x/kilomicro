use coreDB
go

alter table jnl add
	constraint fk_jnl_batch foreign key (jnl_batch_id)
	references jnl_batch(jnl_batch_id) on delete cascade on update cascade 
go

alter table jnl add
	constraint fk_jnl_acct foreign key (acct_id)
	references accts(acct_id) on delete no action on update cascade 
go
	
alter table jnl add
	constraint fk_jnl_currency foreign key (currency_id)
	references currencies(currency_id) 
go
	 
alter table jnl add
	constraint fk_jnl_cost_center foreign key (cost_center_id)
	references gl_ou(ou_id) 
go