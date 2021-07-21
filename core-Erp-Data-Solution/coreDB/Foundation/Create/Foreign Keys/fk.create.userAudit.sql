use coreDB
go

alter table dbo.userAudit add constraint fk_userAudit_users foreign key (userName)
	references dbo.users (user_name)
go

alter table dbo.userAudit add constraint fk_userAudit_modules foreign key (moduleid)
	references dbo.modules (module_id)
go

