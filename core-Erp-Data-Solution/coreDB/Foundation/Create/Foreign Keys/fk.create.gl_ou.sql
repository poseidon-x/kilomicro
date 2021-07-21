use coreDB
go

alter table gl_ou add
	constraint fk_gl_ou_cat foreign key (ou_cat_id)
	references gl_ou_cat(ou_cat_id) on delete cascade on update cascade 
go
	