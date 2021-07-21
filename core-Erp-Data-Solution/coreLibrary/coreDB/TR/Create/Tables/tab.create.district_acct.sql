use coreDB
go
 
CREATE TABLE tr.district_acct
(
  district_acct_id int identity(1,1) not null constraint pk_district_acct primary key,
  district_id int not null, 
  bank_id int not null, 
  branch_id int not null, 
  acct_num nvarchar(50) not null constraint ck_district_acct_num check(datalength(ltrim(rtrim(acct_num))) > 0), 
  currency_id int null,  
  creation_date datetime null default(getdate()) constraint ck_district_acct_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_district_acct_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_district_acct_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_district_acct_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
