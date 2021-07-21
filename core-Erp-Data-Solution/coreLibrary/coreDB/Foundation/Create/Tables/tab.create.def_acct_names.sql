use coreDB
go
 
CREATE TABLE def_acct_names
(
  code nvarchar(5) not null constraint pk_def_acct_names primary key constraint ck_def_acct_namess_code check(datalength(ltrim(rtrim(code)))>0),
  description nvarchar(100) not null constraint uk_def_acct_names_desc unique constraint ck_def_acct_names_desc check(datalength(ltrim(rtrim(description)))>0),
  creation_date datetime null default(getdate()) constraint ck_def_acct_namess_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_def_acct_namess_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_def_acct_namess_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_def_acct_namess_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
