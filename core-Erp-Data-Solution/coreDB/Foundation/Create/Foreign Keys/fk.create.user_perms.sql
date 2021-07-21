use coreDB
go

alter table user_perms add
	constraint fk_user_perms_perm foreign key (perm_code)
	references perms(perm_code) on delete cascade on update cascade 
go
	
alter table user_perms add
	constraint fk_user_perms_user foreign key (user_name)
	references users(user_name) on delete cascade on update cascade 
go
		
alter table user_perms add
	constraint fk_user_perms_module foreign key (module_id)
	references modules(module_id) on delete cascade on update cascade 
go
	