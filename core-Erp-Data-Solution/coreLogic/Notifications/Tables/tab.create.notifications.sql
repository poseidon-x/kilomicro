use coreDB
go

create table noti.notificationType
(
	notificationTypeID tinyint not null primary key,
	notificationTypeName nvarchar(100) not null unique
)
go

create table noti.notificationPrivilege
(
	notificationPrivilegeID int not null identity(1,1) primary key,
	notificationTypeID tinyint not null,
	allowAll bit not null default(0),
	userName nvarchar(30) not null default(''),
	roleName nvarchar(30) not null default('')
)
go

