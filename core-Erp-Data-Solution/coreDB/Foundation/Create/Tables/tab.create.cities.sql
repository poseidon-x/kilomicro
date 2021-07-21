use coreDB
go
 
CREATE TABLE cities
(
  city_id int identity(1,1) not null constraint pk_city primary key,
  city_name nvarchar(100) not null constraint uk_city_name unique constraint ck_city_name check(datalength(ltrim(rtrim(city_name)))>0),
  district_id int not null constraint ck_city_district_id check(district_id > 0),
  creation_date datetime null default(getdate()) constraint ck_city_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_city_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_city_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_city_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
