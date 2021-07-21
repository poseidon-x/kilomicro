use coreDB
go
 
CREATE TABLE jnl_stg
(
  jnl_id int identity(1,1) not null constraint pk_jnl_stg primary key,
  jnl_batch_id int not null constraint ck_jnl_stg_batch_id check(jnl_batch_id > 0), 
  ref_no nvarchar(20) null, 
  acct_id int not null constraint ck_jnl_stg_acct check(acct_id > 0),
  tx_date datetime not null,
  acct_period int null constraint ck_jnl_stg_acct_period check(acct_period is null or acct_period > 0),
  description nvarchar(250) not null constraint ck_jnl_batch_stg check(datalength(ltrim(rtrim(description)))>0),
  currency_id int not null constraint ck_jnl_stg_cur check(currency_id > 0),
  rate float not null constraint df_jnl_stg_rate default (1.0),
  dbt_amt float not null constraint df_jnl_stg_dbt default (0),
  crdt_amt float not null constraint df_jnl_stg_crdt default (0),
  frgn_dbt_amt float not null constraint df_jnl_stg_frgn_dbt default (0),
  frgn_crdt_amt float not null constraint df_jnl_stg_frgn_crdt default (0),
  recipient nvarchar(100) null,
  cost_center_id int null constraint ck_jnl_stg_cost_center_id check(cost_center_id is null or cost_center_id > 0),
  creation_date datetime null default(getdate()) constraint ck_jnl_stg_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_jnl_stg_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_jnl_stg_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_jnl_stg_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO


alter table jnl_stg add constraint ck_jnl_stg_deb_cred
	check ((dbt_amt>0 or crdt_amt>0) and (dbt_amt=0 or crdt_amt=0))
go

alter table jnl_stg add
	check_no nvarchar(50) null
go