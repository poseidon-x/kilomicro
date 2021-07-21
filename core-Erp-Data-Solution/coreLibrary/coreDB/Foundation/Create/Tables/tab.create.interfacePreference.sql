use coreDB
go

create table dbo.interfacePreference
(
	interfacePreferenceID int identity(1,1) not null primary key,
	userName nvarchar(30) not null unique,
	skinName nvarchar(30) not null default('Metro')
)
go

alter table dbo.interfacePreference add
	backgroundColor nvarchar(30) not null default('ivory'),
	foreColor nvarchar(30) not null default('black')
go
