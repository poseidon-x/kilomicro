use coreDB
go
 
CREATE TABLE user_gl_ou_gl_ou
( 
  user_gl_ou_id int identity(1,1) not null constraint pk_user_gl_ou_id primary key,
  user_name nvarchar(30) not null constraint ck_user_gl_ou_user_name check(datalength(ltrim(rtrim(user_name)))>0),
  cost_center_id int not null constraint ck_user_gl_ou_cost_center_id check(cost_center_id > 0),
  allow bit not null constraint df_user_gl_ou_allow default(1),
  creation_date datetime not null default(getdate()) constraint ck_user_gl_ou_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null, 
  modification_date datetime null constraint ck_user_gl_ou_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_user_gl_ou_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

 
