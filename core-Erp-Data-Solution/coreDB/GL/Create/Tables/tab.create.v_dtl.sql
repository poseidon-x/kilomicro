use coreDB
go
 
CREATE TABLE gl.v_dtl
(
  v_dtl_id int identity(1,1) not null constraint pk_v_dtl primary key,
  v_head_id int not null,
  acct_id int not null constraint ck_v_dtl_acct_id check(acct_id > 0),
  ref_no nvarchar(30) null ,
  tx_date datetime not null,
  description nvarchar(200) not null,
  gl_ou_id int null,
  amount float not null,
  is_imprest bit not null default(0),
  creation_date datetime null default(getdate()) constraint ck_v_dtl_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_v_dtl_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_v_dtl_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_v_dtl_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

alter table gl.v_dtl add
  vat_amt float not null constraint df_v_dtl_vat_amt default(0),
  nhil_amt float not null constraint df_v_dtl_nhil_amt default(0)
 
