use coreDB
go
 
CREATE TABLE sup_types
(
  sup_type_id int identity(1,1) not null constraint pk_sup_type primary key,
  sup_type_name nvarchar(100) not null constraint uk_sup_type_name unique constraint ck_sup_type_name check(datalength(ltrim(rtrim(sup_type_name)))>0),
  creation_date datetime null default(getdate()) constraint ck_sup_type_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_sup_type_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_sup_type_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_sup_type_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
