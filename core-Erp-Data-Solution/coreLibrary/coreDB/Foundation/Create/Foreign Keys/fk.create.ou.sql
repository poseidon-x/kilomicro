use coreDB
go

alter table ou add
	constraint fk_ou_cat foreign key (ou_cat_id)
	references ou_cat(ou_cat_id) on delete cascade on update cascade 
go
	