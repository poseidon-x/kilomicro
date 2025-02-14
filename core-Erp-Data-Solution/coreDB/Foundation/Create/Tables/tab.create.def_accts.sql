﻿use coreDB
go
 
CREATE TABLE def_accts
(
  def_acct_id int identity(1,1) not null constraint pk_def_accts primary key,
  code nvarchar(5) not null constraint uk_def_accts unique constraint ck_def_accts_code check(datalength(ltrim(rtrim(code)))>0),
  acct_id int not null constraint ck_def_acct_id check(acct_id > 0),
  creation_date datetime null default(getdate()) constraint ck_def_accts_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_def_accts_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_def_accts_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_def_accts_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
