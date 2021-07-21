use coreDB
go

create table man.durationType 
(
	durationTypeId int identity(1,1) primary key not null,
	durationTypeName nvarchar(50) not null,
	durationTypeCode nvarchar(10) not null unique,
	detailDurationTypeId int not null
)

create table man.manufacturingCalender 
(
	manufacturingCalenderId int not null identity primary key ,
	assemblyLineId int not null,
	productId int not null,
	duration float not null,
	durationTypeId int not null,
	expectedQuantity int not null,
	unitOfMeasurementId int not null,
	startDate datetime not null,
	endDate datetime not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)

create table man.actualPerfomance 
(
	actualPerfomanceId int identity primary key not null,
	manufacturingCalenderId int not null,
	duration nvarchar(50) not null,
	durationTypeId int not null,
	actualQuantity float not null,
	unitOfMeasurementId int not null,
	startDate datetime not null,
	endDate datetime not null,
	batchNumber nvarchar(20) not null,
	manufacturingDate datetime not null,
	expiryDate datetime not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)
	
create table man.manufacturingScrap 
(
	manufacturingScrapId int identity primary key not null,
	actualPerfomanceId int not null,
	quantity float not null,
	unitOfMeasurementId int not null,
	scrapReasonId int not null,
	scrapDate datetime not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)

create table man.scrapReason 
(
	scrapReasonId int identity primary key not null,
	scrapReasonName nvarchar(50) not null
)
	
create table man.manufacturingCalenderStaff 
(
	manufacturingCalenderStaffId int identity primary key not null,
	manufacturingCalenderId int not null,
	employeeStaffId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)

	
create table man.billOfMaterial 
(
	billOfMaterialId int identity primary key not null,
	productId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)
	
create table man.billOfMaterialDetail 
(
	billOfMaterialDetailId int identity primary key not null,
	billOfMaterialId int null,
	productId int not null,
	unitOfMeasurementId int not null,
	quantity float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)


