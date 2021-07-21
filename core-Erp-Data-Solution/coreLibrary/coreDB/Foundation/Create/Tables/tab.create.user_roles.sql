use coreDB
go
 
CREATE TABLE user_roles
( 
  user_role_id int identity(1,1) not null constraint pk_user_roles primary key, 
  user_name nvarchar(30) not null,
  role_name nvarchar(30) not null,
  creation_date datetime not null default(getdate()) 
	constraint ck_user_role_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null 
	constraint ck_user_role_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null 
	constraint ck_user_role_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
) 
GO

 
alter table user_roles add
	constraint uk_user_roles unique (user_name, role_name)