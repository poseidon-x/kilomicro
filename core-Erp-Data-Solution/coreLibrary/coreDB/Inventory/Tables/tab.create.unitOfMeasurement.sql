use coreDB
go

create table iv.unitOfMeasurement
(
	unitOfMeasurementId int identity(1,1) not null primary key,
	unitOfMeasurementName nvarchar(100) not null unique,
	complexDetailUnitOfMeasurementId int,
	numberOfUnits float not null default(0),
	createdBy nvarchar(30) not null,
	creationDate datetime not null default(getDate()),
	modifiedBy nvarchar(30),
	modifiedDate datetime
)
go