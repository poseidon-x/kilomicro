use coreDB
go
 
CREATE TABLE jnl_batch_stg
(
  jnl_batch_id int identity(1,1) not null constraint pk_jnl_batch_stg primary key,
  batch_no nvarchar(30) not null constraint ck_jnl_batch_stg_batch_no check(datalength(ltrim(rtrim(batch_no)))>0),
  source nvarchar(20) not null constraint ck_jnl_batch_stg_source check(datalength(ltrim(rtrim(source)))>0),
  posted bit not null constraint df_jnl_batch_stg_posted default (0),
  multi_currency bit not null constraint df_jnl_batch_stg_multi_currency default (0),
  is_adj bit not null constraint df_jnl_batch_stg_is_adj default (0),
  creation_date datetime null default(getdate()) constraint ck_jnl_batch_stg_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_jnl_batch_stg_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_jnl_batch_stg_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_jnl_batch_stg_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 