use coreDB
go
 
CREATE TABLE phon_types
(
  phon_type nchar(1) not null constraint pk_phon_type primary key,
  phon_type_name nvarchar(100) not null constraint uk_phon_type_name unique constraint ck_phon_type_name check(datalength(ltrim(rtrim(phon_type_name)))>0),
  creation_date datetime null default(getdate()) constraint ck_phon_type_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_phon_type_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_phon_type_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_phon_type_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
