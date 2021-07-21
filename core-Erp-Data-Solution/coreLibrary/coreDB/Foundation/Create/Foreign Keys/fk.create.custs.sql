use coreDB
go

alter table custs add
	constraint fk_custs_type foreign key (cust_type_id)
	references cust_types(cust_type_id) on delete cascade on update cascade 
go

alter table custs add
	constraint fk_custs_currency foreign key (currency_id)
	references currencies(currency_id) on delete no action on update cascade 
go
	
alter table custs add
	constraint fk_custs_ar_acct foreign key (ar_acct_id)
	references accts(acct_id) 
go
		
alter table custs add
	constraint fk_custs_vat_acct foreign key (vat_acct_id)
	references accts(acct_id)  
go
	