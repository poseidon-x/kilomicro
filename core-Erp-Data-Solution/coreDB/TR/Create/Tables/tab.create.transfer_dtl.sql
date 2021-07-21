use coreDB
go
 
CREATE TABLE tr.transfer_dtl
(
  transfer_dtl_id int identity(1,1) not null constraint pk_transfer_dtl primary key, 
  district_id int not null,
  district_acct_id int not null,
  num_of_bags int not null,
  commission_rate float not null,
  price_per_bag float not null,
  pprice float not null,
  commission_amt float not null,
  creation_date datetime null default(getdate()) constraint ck_transfer_dtl_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_transfer_dtl_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_transfer_dtl_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_transfer_dtl_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
 alter table tr.transfer_dtl add
  transfer_id int not null

  alter table tr.transfer_dtl add
  total_amt as pprice + commission_amt 
  