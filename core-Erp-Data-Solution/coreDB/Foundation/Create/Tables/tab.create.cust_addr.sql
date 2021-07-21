use coreDB
go
 
CREATE TABLE cust_addr
(
  cust_addr_id int identity(1,1) not null constraint pk_cust_addr primary key,
  addr_type nchar(1) not null ,
  is_default bit not null ,
  cust_id int not null constraint ck_cust_addr check(cust_id > 0),
  addr_line_1 nvarchar(250) not null constraint ck_cust_addr_line_1 check(datalength(ltrim(rtrim(addr_line_1)))>0),
  addr_line_2 nvarchar(250) null 
	constraint ck_cust_addr_line_2 check(addr_line_2 is null or datalength(ltrim(rtrim(addr_line_2)))>0),
  city_id int null constraint ck_cust_addr_city check (city_id is null or city_id > 0),
  creation_date datetime null default(getdate()) constraint ck_cust_addr_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_cust_addr_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_cust_addr_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_cust_addr_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
alter table cust_addr add
	constraint uk_cust_addr unique (cust_id, addr_type)

