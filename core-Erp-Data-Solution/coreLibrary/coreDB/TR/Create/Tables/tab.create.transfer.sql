use coreDB
go
 
CREATE TABLE tr.transfer
(
  transfer_id int identity(1,1) not null constraint pk_transfer primary key, 
  season_id int not null,
  transfer_name nvarchar(50) not null constraint ck_transfer_name check(datalength(ltrim(rtrim(transfer_name))) > 0), 
  transfer_date datetime not null default(getdate()) constraint ck_transfer_date check(transfer_date  <= dateadd(day,1,getdate())),
  transfer_num int not null,
  creation_date datetime null default(getdate()) constraint ck_transfer_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_transfer_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_transfer_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_transfer_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
 
 alter table tr.transfer add
  approved_by nvarchar(50) null constraint ck_transfer_dtl_approved_by check(approved_by is null or datalength(ltrim(rtrim(approved_by))) > 0), 
  approved_date datetime null constraint ck_transfer_dtl_approved_date check(approved_date is null or approved_date  <= dateadd(day,1,getdate()))