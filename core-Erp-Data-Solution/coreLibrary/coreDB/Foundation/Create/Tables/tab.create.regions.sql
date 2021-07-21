use coreDB
go
 
CREATE TABLE regions
(
  region_id int identity(1,1) not null constraint pk_region primary key,
  region_name nvarchar(100) not null constraint uk_region_name unique constraint ck_region_name check(datalength(ltrim(rtrim(region_name)))>0),
  country_id int not null constraint ck_region_country_id check(country_id > 0),
  abbrev nvarchar(5) null ,
  creation_date datetime null default(getdate()) constraint ck_region_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_region_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_region_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_region_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
