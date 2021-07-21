use coreDB
go

create table hc.staffQualification
(
	staffQualificationID int identity(1,1) not null primary key,
	staffID int not null,
	qualificationTypeID int not null,
	qualificationSubjectID int not null,
	startDate datetime,
	endDate datetime,
	expiryDate datetime,
	institutionName nvarchar(250)
)
go

create table hc.staffRelation
(
	staffRelationID int identity(1,1) not null primary key,
	staffID int not null,
	relationTypeID int not null,
	surName nvarchar(50) not null,
	otherNames nvarchar(50) not null,
	dob datetime
)
go

create table hc.staffAllowance
(
	staffAllowanceID int identity(1,1) not null primary key,
	staffID int not null,
	allowanceTypeID int not null,
	amount float not null ,
	percentValue float not null  ,
	isEnabled bit not null default(1)
)
go

create table hc.staffDeduction
(
	staffDeductionID int identity(1,1) not null primary key,
	staffID int not null,
	deductionTypeID int not null,
	amount float not null ,
	percentValue float not null ,
	isEnabled bit not null default(1)
)
go

create table hc.staffManager
(
	staffManagerID int identity(1,1) not null primary key,
	staffID int not null,
	managerStaffID int null,
	levelID int not null,
	levelNotchID int null
)
go

create table hc.staffPension
(
	staffPensionID int identity(1,1) not null primary key,
	staffID int not null,
	pensionTypeID int not null,
	employeeAmount float not null ,
	employerAmount float not null default(0),
	isPercent bit not null default(1),
	isEnabled bit not null default(1)
)
go

create table hc.staffBenefit
(
	staffBenefitID int identity (1,1) not null primary key,
	staffID int not null unique,
	basicSalary float not null default(0),
	ssn nvarchar(50) null,
	bankAccountNo nvarchar(50) null,
	bankName nvarchar(100) null,
	bankBranchName nvarchar(100)
)
go

create table hc.staffOneTimeDeduction
(
	staffOneTimeDeductionID int identity(1,1) not null primary key,
	staffID int not null,
	oneTimeDeductionTypeID int not null,
	payCalendarID int not null,
	amount float not null ,
	percentValue float not null  ,
	isEnabled bit not null default(1)
)
go

create table hc.staffBenefitsInKind
(
	staffBenefitsInKindID int identity(1,1) not null primary key,
	staffID int not null,
	benefitsInKindID int not null,
	amount float not null ,
	percentValue float not null ,
	isEnabled bit not null default(1)
)
go

create table hc.staffTaxRelief
(
	staffTaxReliefID int identity(1,1) not null primary key,
	staffID int not null,
	taxReliefTypeID int not null,
	amount float not null ,
	isEnabled bit not null default(1)
)
go

alter table hc.staffManager add
	employmentStartDate datetime,
	employmentStopDate datetime
go
