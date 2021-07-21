use coreDB
go
 
CREATE TABLE acct_heads
(
  acct_head_id int identity(1,1) not null constraint pk_acct_heads primary key,
  acct_cat_id int not null constraint ck_acct_head_cat check (acct_cat_id >0),
  head_name nvarchar(100) not null constraint ck_acct_head_name check(datalength(ltrim(rtrim(head_name)))>0),
  parent_acct_head_id int null constraint ck_acct_head_parent check(parent_acct_head_id is null
	or parent_acct_head_id>0),
  max_acct_num nvarchar(20) not null constraint ck_acct_head_max_acct_num check(datalength(ltrim(rtrim(max_acct_num)))>0),
  min_acct_num nvarchar(20) not null constraint ck_acct_head_min_acct_num check(datalength(ltrim(rtrim(min_acct_num)))>0),
  creation_date datetime null default(getdate()) constraint ck_acct_head_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_acct_head_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_acct_head_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_acct_head_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

alter table acct_heads add
	constraint ck_acct_head_max_min check(max_acct_num>min_acct_num)
GO

alter table acct_heads add
	constraint uk_acct_head_max_acct_num unique (parent_acct_head_id, max_acct_num)
GO

alter table acct_heads add
	constraint uk_acct_head_min_acct_num unique (parent_acct_head_id, min_acct_num)
GO
	
alter table acct_heads add
	constraint uk_acct_head_name unique (parent_acct_head_id, head_name)
GO
 
