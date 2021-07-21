use coreDB
go

create table ln.emailType
(
	emailTypeID int primary key, 
	emailTypeName nvarchar(30) not null
)
go
create table ln.email
(
	emailID int identity (1,1) primary key, 
	emailTypeID int, 
	emailAddress nvarchar(300)
)
go
