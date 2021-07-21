use coreDB
go
 
CREATE TABLE sups
(
  sup_id int identity(1,1) not null constraint pk_sup primary key,
  sup_type_id int not null constraint ck_sup_type check(sup_type_id > 0),
  currency_id int not null constraint ck_sup_currency check(currency_id > 0),
  rep_emp_id int null constraint ck_sup_rep check(rep_emp_id is null or rep_emp_id > 0),
  ap_acct_id int null constraint ck_sup_ap_acct_id check(ap_acct_id is null or ap_acct_id > 0),
  vat_acct_id int null constraint ck_sup_vat_acct_id check(vat_acct_id is null or vat_acct_id > 0),
  acc_num nvarchar(20) not null constraint uk_sup_acc_num unique
	constraint ck_sup_acc_num check(datalength(ltrim(rtrim(acc_num)))>0),
  sup_name nvarchar(250) not null constraint ck_sup_name check(datalength(ltrim(rtrim(sup_name)))>0),
  contact_person nvarchar(250) null constraint ck_sup_contact_person check(contact_person is null or datalength(ltrim(rtrim(contact_person)))>0),
  debit_terms ntext null,
  creation_date datetime null default(getdate()) constraint ck_sup_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_sup_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_sup_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_sup_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
alter table sups add 
	constraint uk_sup_name unique (sup_type_id, sup_name)