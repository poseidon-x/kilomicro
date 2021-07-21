use coreDB
go

create table hc.performanceContractStatus
(
	performanceContractStatusID int not null primary key,
	performanceContractStatusName nvarchar(100) not null unique
)
go

insert into hc.performanceContractStatus (performanceContractStatusID, performanceContractStatusName)
values (1, 'Contracting and Agreement')
insert into hc.performanceContractStatus (performanceContractStatusID, performanceContractStatusName)
values (2, 'Mid-Year Review')
insert into hc.performanceContractStatus (performanceContractStatusID, performanceContractStatusName)
values (3, 'Final Review')
insert into hc.performanceContractStatus (performanceContractStatusID, performanceContractStatusName)
values (4, 'Closed')
insert into hc.performanceContractStatus (performanceContractStatusID, performanceContractStatusName)
values (-1, 'Cancelled')
go

create table hc.performanceAppraisalType
(
	performanceAppraisalTypeID int not null primary key,
	performanceAppraisalTypeName nvarchar(100) not null unique
)
go

insert into hc.performanceAppraisalType (performanceAppraisalTypeID, performanceAppraisalTypeName)
values (1, 'Monthly Review')
insert into hc.performanceAppraisalType (performanceAppraisalTypeID, performanceAppraisalTypeName)
values (2, 'Mid-Year Review')
insert into hc.performanceAppraisalType (performanceAppraisalTypeID, performanceAppraisalTypeName)
values (3, 'Final Review')


create table hc.performanceScore
(
	performanceScoreID int not null primary key,
	performanceScoreName nvarchar(100) not null unique,
	scoreValue float not null unique
)
go

insert into hc.performanceScore (performanceScoreID, performanceScoreName, scoreValue)
values (0, 'Not Achieved', 0)
insert into hc.performanceScore (performanceScoreID, performanceScoreName, scoreValue)
values (1, 'Partly Achieved', 1)
insert into hc.performanceScore (performanceScoreID, performanceScoreName, scoreValue)
values (3, 'Over Achieved', 3)
insert into hc.performanceScore (performanceScoreID, performanceScoreName, scoreValue)
values (4, 'Exceeds Expectations', 4)
insert into hc.performanceScore (performanceScoreID, performanceScoreName, scoreValue)
values (2, 'Achieved On Target', 2)

create table hc.performanceArea
(
	performanceAreaID int identity(1,1) not null primary key,
	performanceAreaName nvarchar(100) not null unique,
	createdBy nvarchar(30) not null,
	creationDate datetime not null default(getdate()),
	modifiedBy nvarchar(30) null,
	modifiedDate datetime null
)
go

create table hc.performanceContract
(
	performanceContractID int identity(1,1) not null primary key,
	staffID int not null,
	startDate datetime not null,
	endDate datetime not null,
	performanceContractStatusID int not null,
	enteredBy nvarchar(30) not null,
	creationDate datetime not null default(getdate())
)
go

create table hc.performanceContractItem
(
	performanceContractItemID int identity(1,1) not null primary key,
	performanceContractID int not null, 
	performanceAreaID int not null,
	itemDescription nvarchar(400) not null,
	[weight] float not null
)
go

create table hc.performanceContractTarget
(
	performanceContractTargetID int identity(1,1) not null primary key,
	performanceContractItemID int not null,
	performanceScoreID int not null,  
	targetCreteria nvarchar(400) not null
)
go

create table hc.performanceAppraisal
(
	performanceAppraisalID int identity(1,1) not null primary key,
	performanceContractID int not null, 
	performanceAppraisalTypeID int not null,
	staffComments nvarchar(400) not null default(''),
	managerComments nvarchar(400) not null default(''),
	hrComments nvarchar(400) not null default(''),
	managerStaffID int,
	appraisalDate datetime not null default(getdate())
)
go

create table hc.performanceAppraisalScore
(
	performanceAppraisalScoreID int identity(1,1) not null primary key,
	performanceAppraisalID int not null,
	performanceScoreID int not null,  
	comments nvarchar(400) not null default(''),  
	managerComments nvarchar(400) not null default('')
)
go

alter table hc.performanceAppraisalScore add
	performanceContractItemID int not null
go