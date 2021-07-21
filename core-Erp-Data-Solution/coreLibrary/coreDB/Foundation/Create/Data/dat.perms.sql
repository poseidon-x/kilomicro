use coreDB
go

DELETE from dbo.perms
GO
insert dbo.perms(perm_code,perm_name,creation_date,creator,modification_date,last_modifier) values('A','ALL','Jul  5 2010 11:00AM',null,null,null)
insert dbo.perms(perm_code,perm_name,creation_date,creator,modification_date,last_modifier) values('D','DELETE','Jul  5 2010 11:00AM',null,null,null)
insert dbo.perms(perm_code,perm_name,creation_date,creator,modification_date,last_modifier) values('M','MODIFY','Jul  5 2010 11:00AM',null,null,null)
insert dbo.perms(perm_code,perm_name,creation_date,creator,modification_date,last_modifier) values('N','ADDNEW','Jul  5 2010 11:00AM',null,null,null)
insert dbo.perms(perm_code,perm_name,creation_date,creator,modification_date,last_modifier) values('V','VIEW','Jul  5 2010 11:00AM',null,null,null)
GO
update statistics dbo.perms
GO