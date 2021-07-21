use coreDB
go
 
CREATE TABLE gl.pc_dtl
(
  pc_dtl_id int identity(1,1) not null constraint pk_pc_dtl primary key,
  pc_head_id int not null,
  acct_id int not null constraint ck_pc_dtl_acct_id check(acct_id > 0),
  ref_no nvarchar(30) null ,
  tx_date datetime not null,
  description nvarchar(200) not null,
  gl_ou_id int null,
  check_no nvarchar(30) not null constraint df_pc_dtl_check_no default(''),
  amount float not null,
  creation_date datetime null default(getdate()) constraint ck_pc_dtl_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_pc_dtl_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_pc_dtl_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_pc_dtl_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
