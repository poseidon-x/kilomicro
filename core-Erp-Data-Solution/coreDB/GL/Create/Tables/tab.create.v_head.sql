use coreDB
go
 
CREATE TABLE gl.v_head
(
  v_head_id int identity(1,1) not null constraint pk_v_head primary key,
  bank_acct_id int not null constraint ck_v_bank_acct_id check(bank_acct_id > 0),
  with_acct_id int null constraint ck_v_with_acct_id check(with_acct_id is null or with_acct_id > 0),
  v_type nvarchar(1) not null constraint ck_v_head_type check( v_type in ('C','E','S'))
	constraint df_v_head_v_type default ('C'),
  batch_no nvarchar(30) not null,
  currency_id int not null,
  cust_id int null,
  sup_id int null,
  emp_id int null,
  rate float not null,
  with_rate float not null constraint df_v_head_with_rate default(0),
  vat_rate float not null constraint df_v_head_vat_rate default(0),
  nhil_rate float not null constraint df_v_head_nhil_rate default(0),
  with_amt float not null constraint df_v_head_with_amt default(0),
  vat_amt float not null constraint df_v_head_vat_amt default(0),
  nhil_amt float not null constraint df_v_head_nhil_amt default(0),
  recipient nvarchar(50) null,
  check_no nvarchar(30) not null constraint df_v_dtl_check_no default(''),
  invoice_no nvarchar(30) not null constraint df_v_dtl_invoice_no default(''),
  is_vat bit not null constraint df_v_head_vat default(0),
  is_nhil bit not null constraint df_v_head_nhil default(0),
  is_withheld bit not null constraint df_v_head_withheld default(0),
  posted bit not null constraint df_v_head_posted default(0),
  creation_date datetime null default(getdate()) constraint ck_v_head_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_v_head_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_v_head_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_v_head_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
