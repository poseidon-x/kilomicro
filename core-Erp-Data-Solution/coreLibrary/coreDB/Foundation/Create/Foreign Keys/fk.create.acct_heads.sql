use coreDB
go

alter table acct_heads add
	constraint fk_acct_heads foreign key (acct_cat_id)
	references acct_cats(acct_cat_id) on delete cascade on update cascade 
go
	