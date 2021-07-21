use coreDB
go
 
CREATE TABLE role_perms
( 
  role_perm_id int identity(1,1) not null constraint pk_role_perms primary key,
  perm_code nchar(1) not null 
	 constraint ck_role_perm_code check(datalength(ltrim(rtrim(perm_code)))>0),
  role_name nvarchar(30) not null constraint ck_role_perm_role_name check(datalength(ltrim(rtrim(role_name)))>0),
  module_id int not null constraint ck_role_perm_module_id check(module_id > 0),
  allow bit not null constraint df_role_perms_allow default(1),
  creation_date datetime not null default(getdate()) constraint ck_role_perm_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_role_perm_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_role_perm_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
  
 alter table role_perms add
	constraint uk_role_perms unique
	(
		perm_code,
		role_name,
		module_id
	)