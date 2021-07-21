use coreDB
go

create table dbo.userAudit
(
	userAuditID int identity(1,1) primary key,
	userName nvarchar(30),
	moduleID int,
	actionDateTime datetime not null default(getDate())
)

go

alter table dbo.userAudit add
	allowed bit not null default (0)
go

alter table dbo.userAudit add
	url nvarchar(400)
go