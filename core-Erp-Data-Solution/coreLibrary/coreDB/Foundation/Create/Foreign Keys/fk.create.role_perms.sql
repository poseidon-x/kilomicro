use coreDB
go

alter table role_perms add
	constraint fk_role_perms_perm foreign key (perm_code)
	references perms(perm_code) on delete cascade on update cascade 
go
	
alter table role_perms add
	constraint fk_role_perms_role foreign key (role_name)
	references roles(role_name) on delete cascade on update cascade 
go
		
alter table role_perms add
	constraint fk_role_perms_module foreign key (module_id)
	references modules(module_id) on delete cascade on update cascade 
go
	