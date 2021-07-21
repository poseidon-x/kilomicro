use coreDB
go
 
CREATE TABLE gl_ou_cat
(
  ou_cat_id int identity(1,1) not null constraint pk_gl_ou_cat primary key,
  cat_name nvarchar(100) not null constraint uk_gl_ou_cat_name unique constraint ck_gl_ou_cat_name check(datalength(ltrim(rtrim(cat_name)))>0),
  parent_ou_cat_id int null constraint uk_gl_ou_cat_parent unique 
	constraint ck_gl_ou_cat_parent check(parent_ou_cat_id is null or parent_ou_cat_id > 0),
  creation_date datetime null default(getdate()) constraint ck_gl_ou_cat_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_gl_ou_cat_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_gl_ou_cat_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_gl_ou_cat_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
