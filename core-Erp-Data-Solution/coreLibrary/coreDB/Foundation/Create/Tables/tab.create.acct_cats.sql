use coreDB
go
 
CREATE TABLE acct_cats
(
  acct_cat_id int identity(1,1) not null constraint pk_acct_cats primary key,
  cat_name nvarchar(100) not null constraint uk_acct_cat_name unique constraint ck_acct_cat_name check(datalength(ltrim(rtrim(cat_name)))>0),
  cat_code tinyint not null constraint uk_acct_cat_code unique 
	constraint ck_acct_cat_code check(cat_code between 1 and 8),
  max_acct_num nvarchar(20) not null constraint uk_acct_cat_max_acct_num unique,
  min_acct_num nvarchar(20) not null constraint uk_acct_cat_min_acct_num unique,
  creation_date datetime null default(getdate()) constraint ck_acct_cat_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_acct_cat_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_acct_cat_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_acct_cat_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

alter table acct_cats add
	constraint ck_acct_cat_max_min check(max_acct_num>min_acct_num)
GO

	
 
 
CREATE TABLE dbo.acct_bals
(
	acct_bal_id	int	identity(1,1) not null constraint pk_acct_bals primary key,
	acct_id	int		not null,
	acct_period	int		not null,
	buy_rate	float		not null,
	sell_rate	float		not null,
	loc_bal	float		not null,
	frgn_bal	float		not null,
	currency_id	int		not null,
	creation_date	datetime	null,
	creator	nvarchar(50)		not null,
	modification_date	datetime	null,
	last_modifier	nvarchar(50)	null
)
 