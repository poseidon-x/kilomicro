use coreDB
go
 
CREATE TABLE custs
(
  cust_id int identity(1,1) not null constraint pk_cust primary key,
  cust_type_id int not null constraint ck_cust_type check(cust_type_id > 0),
  currency_id int not null constraint ck_cust_currency check(currency_id > 0),
  rep_emp_id int null constraint ck_cust_rep check(rep_emp_id is null or rep_emp_id > 0),
  ar_acct_id int null constraint ck_cust_ar_acct_id check(ar_acct_id is null or ar_acct_id > 0),
  vat_acct_id int null constraint ck_cust_vat_acct_id check(vat_acct_id is null or vat_acct_id > 0),
  acc_num nvarchar(20) not null constraint uk_cust_acc_num unique
	constraint ck_cust_acc_num check(datalength(ltrim(rtrim(acc_num)))>0),
  cust_name nvarchar(250) not null constraint ck_cust_name check(datalength(ltrim(rtrim(cust_name)))>0),
  contact_person nvarchar(250) null constraint ck_cust_contact_person check(contact_person is null or datalength(ltrim(rtrim(contact_person)))>0),
  credit_terms ntext null,
  creation_date datetime null default(getdate()) constraint ck_cust_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_cust_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_cust_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_cust_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
alter table custs add 
	constraint uk_cust_name unique (cust_type_id, cust_name)