use coreDB
go

create table pref.roleMenu
(
	roleMenuID int not null identity(1,1) primary key,
	roleName nvarchar(100) not null unique,
	description nvarchar(255) not null,
	iconFile nvarchar(400) not null
)
go

create table pref.roleMenuItem
(
	roleMenuItemID int not null identity(1,1) primary key,
	roleMenuID int not null,
	itemName nvarchar(100) not null,
	description nvarchar(255) not null,
	iconFile nvarchar(400) not null,
	moduleID int not null
)
go

alter table pref.roleMenuItem add
	navigateUrl nvarchar(400) null
go

alter table pref.roleMenu add
	navigateUrl nvarchar(400) null
go
