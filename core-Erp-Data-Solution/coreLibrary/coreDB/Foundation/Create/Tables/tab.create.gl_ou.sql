use coreDB
go
 
CREATE TABLE gl_ou
(
  ou_id int identity(1,1) not null constraint pk_gl_ou primary key,
  ou_name nvarchar(200) not null constraint uk_gl_ou_name unique(ou_name, parent_ou_id) constraint ck_gl_ou_name check(datalength(ltrim(rtrim(ou_name)))>0),
  parent_ou_id int null constraint ck_gl_ou_parent check(parent_ou_id is null or parent_ou_id > 0),
  creation_date datetime null default(getdate()) constraint ck_gl_ou_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  ou_cat_id int not null constraint ck_gl_ou_cat_id check(ou_cat_id > 0),
  ou_manager_id int null constraint ck_gl_ou_manager_id check(ou_manager_id is null or ou_manager_id > 0),
  creator nvarchar(50) not null constraint ck_gl_ou_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_gl_ou_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_gl_ou_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
