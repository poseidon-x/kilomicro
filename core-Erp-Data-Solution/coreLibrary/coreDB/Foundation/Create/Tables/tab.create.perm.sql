use coreDB
go
 
CREATE TABLE perms
( 
  perm_code nchar(1) not null constraint uk_perm_code primary key
	 constraint ck_perm_code check(datalength(ltrim(rtrim(perm_code)))>0
		and perm_code in ('A', 'D', 'M', 'V', 'N')),
  perm_name nvarchar(30) not null constraint ck_perm_name check(datalength(ltrim(rtrim(perm_name)))>0), 
  creation_date datetime not null default(getdate()) constraint ck_perm_creation_date check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) null, 
  modification_date datetime null constraint ck_perm_modification_date check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null constraint ck_perm_last_modifier check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0), 
) 
GO
 
insert into perms (perm_code, perm_name)
values ('A', 'ALL')

insert into perms (perm_code, perm_name)
values ('D', 'DELETE')

insert into perms (perm_code, perm_name)
values ('M', 'MODIFY')

insert into perms (perm_code, perm_name)
values ('V', 'VIEW')

insert into perms (perm_code, perm_name)
values ('N', 'ADDNEW')
