﻿use coreDB
go
 
CREATE TABLE cust_phons
(
  cust_phon_id int identity(1,1) not null constraint pk_cust_phon primary key,
  cust_id int not null constraint ck_cust_phone check(cust_id > 0),
  phon_type nchar(1) not null ,
  is_default bit not null ,
  phon_num nvarchar(250) not null constraint ck_cust_phon_num check(datalength(ltrim(rtrim(phon_num)))>0),
  creation_date datetime null default(getdate()) constraint ck_cust_phon_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_cust_phon_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_cust_phon_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_cust_phon_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0) 
) 
GO
 
alter table cust_phons add
	constraint uk_cust_phon unique (cust_id, phon_type)
alter table cust_phons add
	constraint uk_cust_phon_num unique (phon_num)

