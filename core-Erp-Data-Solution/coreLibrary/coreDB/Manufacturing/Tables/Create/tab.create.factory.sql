use coreDB
go

create table man.factory
(
	factoryId smallint not null identity(1,1) primary key,
	factoryName nvarchar(40) not null,
	factoryTypeId smallint not null,
	locationId smallint not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)

create table man.factoryType
(
	factoryTypeId smallint not null identity(1,1) primary key,
	factoryTypeName int not null
)
