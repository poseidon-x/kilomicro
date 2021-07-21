use coreDB
go

alter table user_roles add
	constraint fk_user_roles_user foreign key (user_name)
	references users(user_name) on delete cascade on update cascade 
go
	
alter table user_roles add
	constraint fk_user_roles_role foreign key (role_name)
	references roles(role_name) on delete cascade on update cascade 
go
	