use coreDB
go
 
CREATE TABLE countries
(
  country_id int identity(1,1) not null constraint pk_country primary key,
  country_name nvarchar(100) not null constraint uk_country_name unique constraint ck_country_name check(datalength(ltrim(rtrim(country_name)))>0),
  nationality nvarchar(100) not null constraint uk_nationality unique constraint ck_nationality check(datalength(ltrim(rtrim(nationality)))>0),
  currency_id int null constraint ck_country_currency_id check(currency_id is null or currency_id > 0),
  country_code nvarchar(5) not null constraint uk_country_code unique constraint ck_country_code check(datalength(ltrim(rtrim(country_code)))>0),
  abbrev nvarchar(5) null ,
  creation_date datetime null default(getdate()) constraint ck_country_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_country_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_country_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_country_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
