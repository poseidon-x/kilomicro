use coreDB
go
 
CREATE TABLE bank_branches
(
  branch_id int identity(1,1) not null constraint pk_bank_branch primary key,
  bank_id int not null constraint ck_bank_branch check(bank_id >0),
  branch_name nvarchar(100) not null constraint ck_branch_name check(datalength(ltrim(rtrim(branch_name)))>0),
  location_id int null constraint ck_branch_branch_location check(location_id is null or location_id >0),
  creation_date datetime null default(getdate()) constraint ck_branch_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null constraint ck_branch_creator check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null constraint ck_branch_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_branch_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
alter table bank_branches add  
	constraint uk_branch_name unique (bank_id, branch_name) 
go
 
alter table bank_branches add  
	branch_code nvarchar(50) not null default('') 
go

create table bankAccountType
(
	accountTypeID int not null primary key,
	accountTypeName nvarchar(50) not null unique
)
go

insert into bankAccountType(accountTypeID, accountTypeName) values (1, 'Savings')
insert into bankAccountType(accountTypeID, accountTypeName) values (2, 'Current')
insert into bankAccountType(accountTypeID, accountTypeName) values (3, 'Investment')
go
