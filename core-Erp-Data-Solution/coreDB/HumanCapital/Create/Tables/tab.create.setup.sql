use coreDB
go

create table hc.allowanceType
(
	allowanceTypeID int identity(1,1) primary key,
	alllowanceTypeName nvarchar(250) not null unique,
	isPercent bit not null default(0),
	amount float not null ,
	isTaxable bit not null default(0)
)
go

create table hc.deductionType
(
	deductionTypeID int identity(1,1) primary key,
	deductionTypeName nvarchar(250) not null unique,
	isPercent bit not null default(0),
	amount float not null ,
	isBeforeTax bit not null default(0),
	isBeforePension bit not null default(0)
)
go

create table hc.qualificationType
(
	qualificationTypeID int identity(1,1) primary key,
	qualificationTypeName nvarchar(250) not null unique,
	sortOrder int not null 
)
go

create table hc.qualificationSubject
(
	qualificationSubjectID int identity(1,1) primary key,
	qualificationSubjectName nvarchar(250) not null unique 
)
go

create table hc.relationType
(
	relationTypeID int identity(1,1) primary key,
	relationTypeName nvarchar(250) not null unique
)
go

create table hc.bonusCalculationType
(
	bonusCalculationTypeID int identity(1,1) primary key,
	bonusCalculationTypeName nvarchar(250) not null unique
)
go

create table hc.bonusType
(
	bonusTypeID int identity(1,1) primary key,
	bonusTypeName nvarchar(250) not null unique,
	bonusCalculationTypeID int not null default(0),
	amount float not null,
	isTaxable bit not null default(0)
)
go

create table hc.oneTimeDeductionType
(
	oneTimeDeductionTypeID int identity(1,1) primary key,
	oneTimeDeductionTypeName nvarchar(250) not null unique,
	isPercent bit not null default(0),
	amount float not null ,
	isBeforeTax bit not null default(0),
	isBeforePension bit not null default(0)
)
go

create table hc.pensionType
(
	pensionTypeID int identity(1,1) primary key,
	pensionTypeName nvarchar(250) not null unique,
	isPercent bit not null default(0),
	employeeAmount float not null ,
	employerAmount float not null default(0),
	isBeforeTax bit not null default(0) 
)
go

create table hc.[level]
(
	levelID int identity(1,1) primary key,
	levelName nvarchar(250) not null unique,
	sortOrder int not null default(0)
)
go

create table hc.[levelNotch]
(
	levelNotchID int identity(1,1) primary key,
	levelID int not null,
	notchName nvarchar(250) not null unique,
	sortOrder int not null default(0)
)
go

create table hc.[levelAllowance]
(
	levelAllowanceID int identity(1,1) primary key,
	levelID int not null,
	allowanceTypeID int not null 
)
go

create table hc.[levelDeduction]
(
	levelDeductionID int identity(1,1) primary key,
	levelID int not null,
	deductionTypeID int not null 
)
go

create table hc.benefitsInKind
(
	benefitsInKindID int identity(1,1) primary key,
	benefitsInKindName nvarchar(250) not null unique,
	isPercent bit not null default(0),
	amount float not null,
	isTaxable bit not null default(0)
)
go

create table hc.[levelBenefitsInKind]
(
	levelBenefitsInKindID int identity(1,1) primary key,
	levelID int not null,
	benefitsInKindID int not null 
)
go

create table hc.leaveType
(
	leaveTypeID int identity(1,1) not null primary key,
	leaveTypeName nvarchar(250) not null unique
)
go

create table hc.levelLeave
(
	levelLeaveID int identity(1,1) not null primary key,
	leaveTypeID int not null,
	levelID int not null,
	DaysPerAnnum int not null default(0)
)
go


create table hc.employmentStatus
(
	employmentStatusID int not null primary key,
	employmentStatusName nvarchar(100) not null unique
)
go

insert into hc.employmentStatus (employmentStatusID, employmentStatusName) values(1,'Active')
insert into hc.employmentStatus (employmentStatusID, employmentStatusName) values(2,'Retired')
insert into hc.employmentStatus (employmentStatusID, employmentStatusName) values(3,'Resigned')
insert into hc.employmentStatus (employmentStatusID, employmentStatusName) values(4,'Terminated')
go

create table hc.taxReliefType
(
	taxReliefTypeID int identity(1,1) primary key,
	taxReliefTypeName nvarchar(250) not null unique ,
	amount float not null 
)
go

alter table hc.allowanceType add
	taxPercent float not null default (0)
go

alter table hc.allowanceType add
	addToBasicAndTax bit not null default (0)
go