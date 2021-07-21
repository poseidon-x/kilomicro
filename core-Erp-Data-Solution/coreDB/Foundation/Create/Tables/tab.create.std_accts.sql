use coreDB
go
 
CREATE TABLE std_accts
(
  code nvarchar(5) not null constraint pk_std_accts primary key constraint ck_std_accts_code check(datalength(ltrim(rtrim(code)))>0),
  description nvarchar(100) not null constraint uk_std_accts_desc unique constraint ck_std_accts_desc check(datalength(ltrim(rtrim(description)))>0),
  creation_date datetime null default(getdate()) constraint ck_std_accts_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_std_accts_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_std_accts_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_std_accts_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
