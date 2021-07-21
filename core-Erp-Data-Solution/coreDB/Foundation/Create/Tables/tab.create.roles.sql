use coreDB
go
 
CREATE TABLE roles
( 
  role_name nvarchar(30) not null constraint pk_roles primary key constraint ck_role_name check(datalength(ltrim(rtrim(role_name)))>0),
  description nvarchar(250) null,
  is_active bit not null constraint df_role_active default(1),
  creation_date datetime not null default(getdate()) constraint ck_role_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_role_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_role_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

 
