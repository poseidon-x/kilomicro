use coreDB
go

create table man.assemblyLine
(
	assemblyLineId int not null identity(1,1) primary key,
	assemblyLineName nvarchar(30) not null unique,
	assemblyLineTypeId smallint not null,
	endProductId int not null,
	factoryId smallint not null,
	supervisorStaffId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)

create table man.assemblyLineType
(
	assemblyLineTypeId smallint not null identity(1,1) primary key,
	assemblyLineTypeName nvarchar(30) not null
)

create table man.workStageType
(
	workStageTypeId int not null identity(1,1) primary key,
	workStageTypeName nvarchar(50) not null
)

create table man.assemblyWorkStage
(
	assemblyWorkStageId int not null identity(1,1) primary key,
	assemblyLineId int not null,
	workStageTypeId int not null,
	workStageName nvarchar(20) not null,
	workStageCode nvarchar(10) not null,
	activityDescription nvarchar(100) not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)

create table man.assemblyLineStaff
(
	assemblyLineStaffId int not null identity(1,1) primary key,
	assemblyLineId int not null,
	employeeStaffId int not null,
	assemblyWorkStageId int not null,
	startDate datetime not null,
	endDate int null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)


insert into man.assemblyLineType
values ('Single Model Line')
insert into man.assemblyLineType
values ('Multi Model Line')
insert into man.assemblyLineType
values ('Mixed Model Line')

insert into man.workStageType
values ('Set-Up Raw Material')
insert into man.workStageType
values ('Work In Progress')
insert into man.workStageType
values ('Packing')
