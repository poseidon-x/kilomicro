use coreDB
go
 
CREATE TABLE comp_prof
(
  comp_prof_id tinyint not null constraint pk_comp_prof primary key,
  comp_name nvarchar(100) not null constraint uk_comp_prof_comp_name unique constraint ck_comp_prof_comp_name check(datalength(ltrim(rtrim(comp_name)))>0),
  addr_line_1 nvarchar(250) not null constraint ck_comp_prof_addr_line_1 check(datalength(ltrim(rtrim(addr_line_1)))>0),
  addr_line_2 nvarchar(250) null 
	constraint ck_comp_prof_addr_line_2 check(addr_line_2 is null or datalength(ltrim(rtrim(addr_line_2)))>0),
  phon_num nvarchar(250) null constraint ck_comp_prof_phon_num check(phon_num is null or datalength(ltrim(rtrim(phon_num)))>0),
  web nvarchar(250) null constraint ck_comp_prof_web check(web is null or datalength(ltrim(rtrim(web)))>0),
  city_id int null constraint ck_comp_prof_city_id check(city_id is null or city_id>0),
  country_id int null constraint ck_comp_prof_country_id check(country_id is null or country_id>0),
  email nvarchar(250) null constraint ck_comp_prof_email check(email is null or datalength(ltrim(rtrim(email)))>0),
  vat_num nvarchar(20) null constraint ck_comp_prof_vat_num check(vat_num is null or datalength(ltrim(rtrim(vat_num)))>0),
  vat_rate float not null constraint df_comp_prof_vat_rate default(0),
  withh_rate float not null constraint df_comp_prof_withh_rate default(0),
  nhil_rate float not null constraint df_comp_prof_nhil_rate default(0),
  vat_flat_rate float not null constraint df_comp_prof_vat_flat_rate default(0),
  employee_rate float not null constraint df_comp_prof_employee_rate default(0),
  employer_rate float not null constraint df_comp_prof_employer_rate default(0),  
  petty_cash_ceil float not null constraint df_comp_prof_petty_cash_ceil default(0),
  currency_id int null constraint ck_comp_prof_currency_id check(currency_id is null or currency_id>0),
  fax nvarchar(250) null constraint ck_comp_prof_fax check(fax is null or datalength(ltrim(rtrim(fax)))>0),
  fmoy tinyint not null constraint df_comp_prof_fmoy default(1),
  num_b4_name bit not null constraint df_comp_prof_num_b4_name default(1),
  edit_posted_jnl bit not null constraint df_comp_prof_edit_posted_jnl default(1),
  enf_ou_usg bit not null constraint df_comp_prof_enf_ou_usg default(0),
  enf_ou_sec bit not null constraint df_comp_prof_enf_ou_sec default(0),
  creation_date datetime null default(getdate()) constraint ck_comp_prof_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_comp_prof_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_comp_prof_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_comp_prof_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO

alter table comp_prof add
	constraint ck_comp_prof_max_min check(comp_prof_id=1)
GO

alter table comp_prof add
	price_per_bag float not null default (0)
go

alter table comp_prof add
	logo image null 
go
 
alter table comp_prof add
	deductInsurance bit not null default(0),
	deductProcFee bit not null default(0) 
go

alter table comp_prof add
	traditionalLoanNo bit not null default (0)
go
 
alter table comp_prof add
	disburseLoansToSavingsAccount bit not null default (0)
go
 
alter table comp_prof add
	defaultInterestTypeID int not null default(1)
go

alter table comp_prof add
	penaltyMode int not null default(1)
go

create table id_prof
(
	comp_prof_id int identity (1,1) not null primary key,
	/*
		1=Original (Link Exchange)
		2=Traditional (Jireh, Eclipse)
		3=Branch Based
		4=Branch Based+Traditional
	*/
	client_account_no_gen_scheme tinyint not null default(1) check(client_account_no_gen_scheme in (1,2,3,4)),
	product_account_no_gen_scheme tinyint not null default(1) check(product_account_no_gen_scheme in (1,2,3,4)),
	staff_no_gen_scheme tinyint not null default(1) check(staff_no_gen_scheme in (1,2,3,4))
)
go


alter table id_prof add
	id_size tinyint not null default(9)
go

alter table id_prof add
	fragment_separator nvarchar(2) not null default('')
go

alter table comp_prof add
	ssnitNo nvarchar(50) not null default(1)
go

