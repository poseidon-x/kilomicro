use coreDB
go
 
CREATE TABLE gl.pc_head
(
  pc_head_id int identity(1,1) not null constraint pk_pc_head primary key,
  pc_acct_id int not null constraint ck_pc_head_acct_id check(pc_acct_id > 0),
  batch_no nvarchar(30) not null,
  currency_id int not null,
  rate float not null,
  recipient nvarchar(50) null,
  recipient_id int null,
  posted bit not null constraint df_pc_head_posted default(0),
  creation_date datetime null default(getdate()) constraint ck_pc_head_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_pc_head_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_pc_head_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_pc_head_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
