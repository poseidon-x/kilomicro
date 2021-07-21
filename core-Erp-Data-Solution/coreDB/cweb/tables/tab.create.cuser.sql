use coreDB
go
 
CREATE TABLE cuser
( 
  user_name nvarchar(30) not null constraint pk_cuser_name primary key
	 constraint ck_cuser_name check(datalength(ltrim(rtrim(user_name)))>0),
  password nvarchar(256) not null constraint ck_cuser_password check(datalength(ltrim(rtrim(password)))>0),
  full_name nvarchar(100) null 
	constraint ck_cuser_full_name check(full_name is null or datalength(ltrim(rtrim(full_name)))>0), 
  is_active bit not null constraint df_cuser_active default(1),
  client_id int null, 
  last_activity_date datetime null,
  last_login_date datetime null,
  last_password_changed_date datetime null, 
  is_onLine bit null,
  is_locked_out bit null,
  last_locked_out_date datetime null,
  login_failure_count int not null default(0),
  creation_date datetime not null default(getdate()) constraint ck_cuser_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_cuser_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_cuser_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

 
