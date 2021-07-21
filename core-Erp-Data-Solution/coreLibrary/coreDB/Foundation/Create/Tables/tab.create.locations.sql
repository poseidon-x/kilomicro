use coreDB
go
 
CREATE TABLE locations
(
  location_id int identity(1,1) not null constraint pk_location primary key,
  location_name nvarchar(100) not null constraint uk_location_name unique constraint ck_location_name check(datalength(ltrim(rtrim(location_name)))>0),
  city_id int not null constraint ck_location_city_id check(city_id > 0),
  location_code nvarchar(5) null,
  creation_date datetime null default(getdate()) constraint ck_location_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_location_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_location_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_location_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
