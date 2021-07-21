use coreDB
go

create table so.shippingMethod
(
	shippingMethodID int identity (1,1) not null primary key,
	shippingMethodName nvarchar(250) not null unique
)
go
