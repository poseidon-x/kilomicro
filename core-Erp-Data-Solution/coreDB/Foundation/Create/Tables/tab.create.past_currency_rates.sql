use coreDB
go
 
CREATE TABLE past_currency_rates
(
  currency_rate_id int identity(1,1) not null constraint pk_past_currency_rates primary key,
  currency_id int not null ,
  buy_rate float not null constraint ck_past_currency_rates_buy_rate check(buy_rate > 0),
  sell_rate float not null constraint ck_past_currency_rates_sell_rate check(sell_rate > 0),
  tran_datetime datetime not null constraint ck_past_currency_rates_tran_datetime check(tran_datetime  <= dateadd(day,1,getdate())),
  creation_date datetime not null default(getdate()) constraint ck_past_currency_rates_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_past_currency_rates_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_past_currency_rates_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_past_currency_rates_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
