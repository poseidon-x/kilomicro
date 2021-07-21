use coreDB
go
 
CREATE TABLE gl.v_ftr
(
  v_ftr_id int identity(1,1) not null constraint pk_v_ftr primary key,
  v_head_id int not null,
  acct_id int not null constraint ck_v_ftr_acct_id check(acct_id > 0),
  is_perc bit not null constraint df_v_ftr_perc default(0),
  description nvarchar(200) not null,
  amount float not null,
  tot_amount float not null, 
  creation_date datetime null default(getdate()) constraint ck_v_ftr_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_v_ftr_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_v_ftr_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_v_ftr_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
