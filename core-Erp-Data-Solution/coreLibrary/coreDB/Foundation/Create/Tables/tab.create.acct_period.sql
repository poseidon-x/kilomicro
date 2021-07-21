use coreDB
go
 
CREATE TABLE dbo.acct_period
(
  acct_period int not null constraint pk_acct_period primary key,
  close_date datetime not null,
  creation_date datetime null default(getdate()) constraint ck_acct_period_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_acct_period_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_acct_period_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_acct_period_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
