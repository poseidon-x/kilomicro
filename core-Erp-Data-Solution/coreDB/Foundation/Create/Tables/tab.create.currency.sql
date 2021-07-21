use coreDB
go
 
CREATE TABLE currencies
(
  currency_id int identity(1,1) not null constraint pk_currencies primary key,
  major_name nvarchar(50) not null constraint uk_currencies_major_name unique constraint ck_currencies_major_name check(datalength(ltrim(rtrim(major_name)))>0),
  minor_name nvarchar(50) not null  constraint ck_currencies_minor_name check(datalength(ltrim(rtrim(minor_name)))>0),
  major_symbol nvarchar(3) not null constraint uk_currencies_major_symbol unique constraint ck_currencies_major_symbol check(datalength(ltrim(rtrim(major_symbol)))>0),
  minor_symbol nvarchar(3) not null   constraint ck_currencies_minor_symbol check(minor_symbol is null or datalength(ltrim(rtrim(minor_symbol)))>0),
  current_buy_rate float not null constraint ck_currencies_current_buy_rate check(current_buy_rate > 0),
  current_sell_rate float not null constraint ck_currencies_current_sell_rate check(current_sell_rate > 0),
  creation_date datetime null default(getdate()) constraint ck_currencies_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_currencies_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_currencies_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_currencies_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
