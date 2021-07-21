use coreDB
go
 
CREATE TABLE modules
( 
  module_id int identity(1,1) not null constraint pk_module primary key,
  module_name nvarchar(250) not null constraint uk_module_name unique
	 constraint ck_module_name check(datalength(ltrim(rtrim(module_name)))>0),
  url nvarchar(250) not null
	 constraint df_module_url default(''),
  parent_module_id int null constraint ck_module_parent check(parent_module_id is null or parent_module_id>0),
  creation_date datetime not null default(getdate()) constraint ck_module_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_module_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_module_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0),
  sort_value tinyint not null constraint df_module_sort_value default(255),
  visible bit not null constraint df_module_visible default(1)
) 
GO

alter table modules add
	module_code nvarchar(10) not null default('')
go

alter table modules alter column
	module_name nvarchar(250) not null 
go

alter table modules drop constraint uk_module_name 
go

alter table modules drop constraint ck_module_last_modifier
go