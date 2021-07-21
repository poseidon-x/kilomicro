use coreDB
go

alter table sups add
	constraint fk_sups_type foreign key (sup_type_id)
	references sup_types(sup_type_id) on delete cascade on update cascade 
go

alter table sups add
	constraint fk_sups_currency foreign key (currency_id)
	references currencies(currency_id) on delete no action on update cascade 
go
	
alter table sups add
	constraint fk_sups_ap_acct foreign key (ap_acct_id)
	references accts(acct_id) 
go
		
alter table sups add
	constraint fk_sups_vat_acct foreign key (vat_acct_id)
	references accts(acct_id)  
go
	