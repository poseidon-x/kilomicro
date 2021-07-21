use coreDB
go

create table dbo.userError
(
	userErrorID int identity(1,1) primary key not null,
	userName nvarchar(30),
	url nvarchar(400),
	errorDate datetime not null default(getdate()),
	exceptionMessage nvarchar(400),
	exceptionStackTrace ntext
)
go