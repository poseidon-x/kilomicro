use coreDB
go
 
CREATE TABLE bank_accts
(
  bank_acct_id int identity(1,1) not null constraint pk_bank_acct primary key,
  branch_id int not null constraint ck_bank_acct_branch check(branch_id >0),
  bank_acct_num nvarchar(100) not null constraint uk_bank_acct_num unique
	constraint ck_bank_acct_num check(datalength(ltrim(rtrim(bank_acct_num)))>0),
  gl_acct_id int not null constraint ck_bank_acct_branch_location check(gl_acct_id >0),
  bank_acct_desc nvarchar(250) null constraint bank_acct_desc check(datalength(ltrim(rtrim(bank_acct_desc)))>0),
  creation_date datetime null default(getdate()) constraint ck_bank_acct_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_bank_acct_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_bank_acct_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_bank_acct_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 