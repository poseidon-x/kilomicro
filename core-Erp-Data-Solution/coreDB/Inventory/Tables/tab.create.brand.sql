use coreDB
go

create table iv.brand
(
	brandId int not null identity(1,1) primary key,
	brandCode nvarchar(10) not null unique,
	brandName nvarchar(240) not null unique
)