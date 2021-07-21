use coreDB
go
 
CREATE TABLE user_perms
( 
  user_perm_id int identity(1,1) not null constraint pk_user_perms primary key,
  perm_code nchar(1) not null 
	 constraint ck_user_perm_code check(datalength(ltrim(rtrim(perm_code)))>0),
  user_name nvarchar(30) not null constraint ck_user_perm_user_name check(datalength(ltrim(rtrim(user_name)))>0),
  module_id int not null constraint ck_user_perm_module_id check(module_id > 0),
  allow bit not null constraint df_user_perms_allow default(1),
  creation_date datetime not null default(getdate()) constraint ck_user_perm_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_user_perm_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_user_perm_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
  
 alter table user_perms add
	constraint uk_user_perms unique
	(
		perm_code,
		user_name,
		module_id
	)