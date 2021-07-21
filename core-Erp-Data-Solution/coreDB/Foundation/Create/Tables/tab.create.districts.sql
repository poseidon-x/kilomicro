use coreDB
go
 
CREATE TABLE districts
(
  district_id int identity(1,1) not null constraint pk_district primary key,
  district_name nvarchar(100) not null constraint uk_district_name unique constraint ck_district_name check(datalength(ltrim(rtrim(district_name)))>0),
  region_id int not null constraint ck_district_region_id check(region_id > 0),
  creation_date datetime null default(getdate()) constraint ck_district_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_district_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_district_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_district_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
