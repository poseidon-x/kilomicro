use coreDB
go

create table iv.locationType
(
	locationTypeId int not null primary key,
	locationTypeName nvarchar(250) not null unique,
	parentLocationTypeId int null,
	locationTypeCode nvarchar(2) not null unique
)
go

insert into iv.locationType (locationTypeId, locationTypeName, parentLocationTypeId, locationTypeCode)
values (1, 'Warehouse', null, 'WH')
insert into iv.locationType (locationTypeId, locationTypeName, parentLocationTypeId, locationTypeCode)
values (2, 'Bin', 1, 'BN')
insert into iv.locationType (locationTypeId, locationTypeName, parentLocationTypeId, locationTypeCode)
values (3, 'Retail Shop', null, 'RS')
insert into iv.locationType (locationTypeId, locationTypeName, parentLocationTypeId, locationTypeCode)
values (4, 'Wholesale Shop', null, 'WS')
insert into iv.locationType (locationTypeId, locationTypeName, parentLocationTypeId, locationTypeCode)
values (5, 'Show Room', null, 'SR')
insert into iv.locationType (locationTypeId, locationTypeName, parentLocationTypeId, locationTypeCode)
values (6, 'Factory', null, 'FT')
go

create table iv.location
(
	locationId int not null identity(1,1) primary key,
	locationCode nvarchar(10) not null unique,
	locationName nvarchar(250) not null,
	locationTypeId int not null,
	physicalAddress nvarchar(250) null,
	cityId int null,
	isActive bit not null default(1),
	longitude float null,
	lattitude float null
)
go
