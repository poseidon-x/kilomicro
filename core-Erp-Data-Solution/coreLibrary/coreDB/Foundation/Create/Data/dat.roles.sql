use coreDB
go

insert dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) values('admin','Adminstrator',1,'Jul  5 2010  4:51PM',null,'Mar  7 2013 10:05AM',null)
insert dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) values('cashier','Cashier',1,'Jul  5 2010  4:51PM',null,'Jul  6 2010  1:14AM',null)
insert dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) values('fieldOfficer','Field Officer',1,'May  9 2013  4:01PM',null,null,null)
insert dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) values('loanAdmin','Loan Administrator',1,'May  9 2013  3:58PM',null,null,null)
insert dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) values('manager','Branch Manager',1,'May  9 2013  4:01PM',null,null,null)
insert dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) values('receptionist','Receptionist',1,'May  9 2013  3:59PM',null,null,null)
GO
update statistics dbo.roles
GO