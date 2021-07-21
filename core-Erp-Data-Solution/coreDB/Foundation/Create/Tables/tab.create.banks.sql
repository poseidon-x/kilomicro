use coreDB
go
 
CREATE TABLE banks
(
  bank_id int identity(1,1) not null constraint pk_bank primary key,
  bank_name nvarchar(100) not null constraint uk_bank_name unique constraint ck_bank_name check(datalength(ltrim(rtrim(bank_name)))>0),
  creation_date datetime null default(getdate()) constraint ck_bank_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_bank_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_bank_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_bank_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
 alter table banks add 
  commission_rate float not null default(0)
go
  
 alter table banks add 
  full_name nvarchar(250) not null default('')
 go
  
 alter table banks add 
  institution_type nvarchar(250) not null default('')
 go