use coreDB
go

alter table user_gl_ou_gl_ou add
	constraint fk_user_gl_ou_gl_ou foreign key (cost_center_id)
	references gl_ou(ou_id) on delete cascade on update cascade 
go
	