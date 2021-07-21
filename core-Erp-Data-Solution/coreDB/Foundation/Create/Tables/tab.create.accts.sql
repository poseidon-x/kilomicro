use coreDB
go
 
CREATE TABLE accts
(
  acct_id int identity(1,1) not null constraint pk_accts primary key,
  acct_head_id int not null constraint ck_acct_cat check (acct_head_id >0),
  acc_name nvarchar(250) not null constraint ck_acct_name check(datalength(ltrim(rtrim(acc_name)))>0),
  acc_num nvarchar(20) not null constraint ck_acct_num check(datalength(ltrim(rtrim(acc_num)))>0),
  currency_id int not null constraint ck_acct_cur check(datalength(ltrim(rtrim(currency_id)))>0),
  acc_is_active bit not null constraint df_acc_is_active default (1),
  creation_date datetime null default(getdate()) constraint ck_acct_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_acct_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_acct_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_acct_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
alter table accts add
	constraint uk_acct_name unique (acct_head_id, acc_name)
GO
	
alter table accts add
	constraint uk_acct_num unique (acc_num)
GO
 
