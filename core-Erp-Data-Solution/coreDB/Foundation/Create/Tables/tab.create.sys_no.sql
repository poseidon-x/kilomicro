use coreDB
go
 
CREATE TABLE sys_no
( 
  name nvarchar(30) not null constraint pk_sys_no primary key, 
  value int not null constraint df_sys_no_value default(1),
  step int not null constraint df_sys_no_step default(1),
  prefix nvarchar(10) not null constraint df_sys_no_prefix default(''),
  suffix nvarchar(10) not null constraint df_sys_no_suffix default(''),
  creation_date datetime not null default(getdate()) 
	constraint ck_sys_no_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_sys_no_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_sys_no_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

 
