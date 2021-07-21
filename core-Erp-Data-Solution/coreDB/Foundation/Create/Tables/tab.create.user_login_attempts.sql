use coreDB
go
 
CREATE TABLE user_login_attempts
( 
  user_login_attempt_id bigint identity(1,1) not null constraint pk_user_login_attempts primary key,
  user_name nvarchar(30) not null
	 constraint ck_user_login_attempts_user_name check(datalength(ltrim(rtrim(user_name)))>0),
  password nvarchar(256) not null constraint ck_user_login_attempts_password check(datalength(ltrim(rtrim(password)))>0),
  ip_address nvarchar(100),
  login_attempt_date datetime not null
	constraint df_login_attempt_time default(getdate()),
  was_successfull bit not null,
  creation_date datetime not null default(getdate()) 
	constraint ck_user_login_attempts_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_user_login_attempts_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_user_login_attempts_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

 
