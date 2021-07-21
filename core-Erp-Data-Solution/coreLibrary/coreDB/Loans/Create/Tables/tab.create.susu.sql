use coreDB
go

create table ln.susuAccount
(
	susuAccountID int identity (1,1) not null primary key,
	clientID int not null,
	applicationDate datetime not null default(getDate()),
	startDate datetime,
	susuPositionID int not null,
	susuGradeID int not null,
	dueDate datetime,
	disbursementDate datetime,
	amountEntitled float not null default(0),
	amountTaken float not null default(0),
	balance float not null default(0),
	approvalDate datetime,
	enteredBy nvarchar(30),
	verifiedBy nvarchar(30),
	approvedBy nvarchar(30),
	staffID int,
	agentID int,
	loanID int
)
go

create table ln.susuContributionSchedule
(
	susuContributionScheduleID int identity (1,1) not null primary key,
	susuAccountID int not null,
	plannedContributionDate datetime not null,
	actualContributionDate datetime,
	balance float not null default(0),
	amount float not null default(0) 
)
go

create table ln.susuContribution
(
	susuContributionID int identity (1,1) not null primary key,
	susuAccountID int not null,
	contributionDate datetime not null,
	modeOfPaymentID int not null,
	amount float not null default(0),
	receiverType int not null default(10),
	agentID int,
	staffID int,
	posted bit not null default(0),
	cashierUsername nvarchar(30),
	appliedToLoan bit not null default(0)
)
go

create table ln.susuGrade
(
	susuGradeID int identity (1,1) not null primary key,
	susuGradeNo int not null unique,
	susuGradeName nvarchar(30) not null unique,
	contributionAmount float not null default(0)
)
go

create table ln.susuPosition
(
	susuPositionID int identity(1,1) not null primary key,
	susuPositionNo int not null unique,
	susuPositionName nvarchar(30) not null unique,
	noOfWaitingPeriods int not null,
	percentageInterest float not null default(0),
	maxDefaultDays int not null
)
go

create table ln.susuGradePosition
(
	susuGradePositionID int identity (1,1) not null primary key,
	susuGradeID int not null,
	susuPositionID int not null,
	amountEntitled float not null default(0)
)
go

create table ln.susuConfig
(
	susuConfigID int not null default(1) check (susuConfigID = 1) primary key,
	susuSchemeID int not null,
	defaultActionID int not null,
	excludeSundays bit not null default(1),
	excludeSaturdays bit not null default(0),
	regularContributionsPayableAccountID int null
)

create table ln.defaultAction
(
	defaultActionID int not null primary key,
	defaultActionName nvarchar(100) not null unique
)
go

create table ln.susuScheme
(
	susuSchemeID int not null primary key,
	susuSchemeName nvarchar(100) not null unique
)
go

insert into ln.susuScheme (susuSchemeID, susuSchemeName) values (1, 'Position & Grade-Based Contribution')
go
insert into ln.susuScheme (susuSchemeID, susuSchemeName) values (2, 'Fixed Payout Grade Based Contribution')
go
insert into ln.defaultAction (defaultActionID, defaultActionName) values (1, 'Move to Last Position')
go
insert into ln.susuConfig (susuConfigID, susuSchemeID, defaultActionID) values(1, 1, 1)
go

alter table ln.susuAccount add
	contributionAmount float not null default(0),
	disbursedBy nvarchar(30)
go
 
alter table ln.susuAccount add
	entitledDate datetime
go

alter table ln.susuConfig add
	daysInPeriod int not null default(31)
go

alter table ln.susuConfig add
	periodsInCycle int not null default(6)
go

create table ln.susuGroup
(
	susuGroupID int identity (1,1) not null primary key,
	susuGroupNo int not null,
	susuGroupName nvarchar(100)
)
go

create table ln.susuGroupHistory
(
	susuGroupHistoryID int identity (1,1) not null primary key,
	susuGroupID int not null,
	susuAccountID int not null
)
go

alter table ln.susuAccount add
	susuAccountNo nvarchar(30) not null unique default(''),
	susuGroupID int
go

alter table ln.susuAccount add
	netAmountEntitled float not null default(0),
	interestAmount float not null default(0)
go

alter table ln.susuAccount add
	modeOfPaymentID int,
	checkNo nvarchar(30),
	bankID int
go

alter table ln.susuAccount add
	posted bit not null default(0)
go

alter table ln.susuContribution add 
	checkNo nvarchar(30),
	bankID int
go

alter table ln.susuContribution add 
	posted bit not null default(0)
go

alter table ln.susuAccount add 
	authorized bit not null default(0)
go

alter table ln.susuConfig add
	contributionsPayableAccountID int
go

alter table ln.susuConfig add
	daysDeductedPerPeriod int not null default(1)
go

alter table ln.susuAccount add
	commissionAmount float not null default(0)
go

alter table ln.susuConfig drop constraint DF__susuConfi__commi__117712A1
alter table ln.susuConfig drop column commissionAmount
go

alter table ln.susuAccount add
	exited bit not null default(0),
	exitDate datetime null
go

alter table ln.susuAccount add 
	exitApprovedBy nvarchar(30) not null default('') 
go

alter table ln.susuAccount add
	regularSusCommissionAmount float not null default(0)
go
  
alter table ln.susuAccount add
	commissionPaid float not null default(0),
	interestPaid float not null default(0),
	principalPaid float not null default(0)
go
  
alter table ln.susuPosition add
	interestDeductedPerMonth float not null default(0)
go
 
alter table ln.susuGrade add
	interestDeductedPerMonth float not null default(0)
go

alter table ln.susuContribution add 
	narration nvarchar(255) not null default ('Group Susu Contribution')
go

alter table ln.regularSusuContribution add 
	narration nvarchar(255) not null default ('Group Susu Contribution')
go

alter table ln.susuConfig add
	regularContributionsPayableAccountID int
go
