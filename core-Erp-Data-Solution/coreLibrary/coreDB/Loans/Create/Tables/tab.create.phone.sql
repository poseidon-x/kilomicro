use coreDB
go

create table ln.phoneType
(
	phoneTypeID int primary key, 
	phoneTypeName nvarchar(30) not null
)
go
create table ln.phone
(
	phoneID int identity (1,1) primary key, 
	phoneTypeID int, 
	phoneNo nvarchar(30)
)
go
