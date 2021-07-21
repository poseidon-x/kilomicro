use coreDB
go

create table dbo.accessLevel
(
	accessLevelID tinyint not null primary key check(accessLevelID> 0 and accessLevelID < 60),
	accessLevelName nvarchar(100)not null unique,
	approvalLimit float not null default(0),
	withdrawalLimit float not null default(0),
	disbursementLimit float not null default(0)
)
go

insert into dbo.accessLevel(accessLevelID, accessLevelName, approvalLimit, withdrawalLimit, disbursementLimit)
values (10, 'Read Only', 0, 0, 0)
insert into dbo.accessLevel(accessLevelID, accessLevelName, approvalLimit, withdrawalLimit, disbursementLimit)
values (20, 'Entry Level', 0, 500, 10000)
insert into dbo.accessLevel(accessLevelID, accessLevelName, approvalLimit, withdrawalLimit, disbursementLimit)
values (30, 'Supervisor', 5000, 5000, 20000)
insert into dbo.accessLevel(accessLevelID, accessLevelName, approvalLimit, withdrawalLimit, disbursementLimit)
values (40, 'Manager', 10000, 10000, 30000)
insert into dbo.accessLevel(accessLevelID, accessLevelName, approvalLimit, withdrawalLimit, disbursementLimit)
values (50, 'Executive', 1000000000, 1000000000, 1000000000)

alter table dbo.users add
	accessLevelID tinyint not null default(10)
go

alter table dbo.users add
	constraint fk_users_accessLevel foreign key (accessLevelID)
	references dbo.accessLevel (accessLevelID)
go
