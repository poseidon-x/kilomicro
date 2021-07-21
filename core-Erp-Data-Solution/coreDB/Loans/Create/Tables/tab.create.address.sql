use coreDB
go

create table ln.addressType
(
	addressTypeID int primary key, 
	addressTypeName nvarchar(50) not null
)
go

create table ln.[address]
(
	addressID int identity(1,1) primary key,
	addressLine1 nvarchar(500),
	addressLine2 nvarchar(500),
	cityTown nvarchar(100)
)
go

create table ln.addressImage
(
	addressImageID int identity(1,1) primary key,
	addressID int , 
	imageID int 
)
go

create table ln.ownerShipType
(
	ownerShipTypeID int primary key,
	ownershipTypeName nvarchar(50) not null
)
go

alter table ln.address add
	ownerShipTypeID int null
go
