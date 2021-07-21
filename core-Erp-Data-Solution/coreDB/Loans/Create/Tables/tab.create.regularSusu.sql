use coreDB
go

create table ln.regularSusuAccount
(
	regularSusuAccountID int identity (1,1) not null primary key,
	clientID int not null,
	applicationDate datetime not null default(getDate()),
	startDate datetime,
	contributionRate float not null default(0),
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

create table ln.regularSusuContributionSchedule
(
	regularSusuContributionScheduleID int identity (1,1) not null primary key,
	regularSusuAccountID int not null,
	plannedContributionDate datetime not null,
	actualContributionDate datetime,
	balance float not null default(0),
	amount float not null default(0) 
)
go

create table ln.regularSusuContribution
(
	regularSusuContributionID int identity (1,1) not null primary key,
	regularSusuAccountID int not null,
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


alter table ln.regularSusuAccount add
	contributionAmount float not null default(0),
	disbursedBy nvarchar(30)
go
 
alter table ln.regularSusuAccount add
	entitledDate datetime
go

alter table ln.SusuConfig add
	regularSusuDaysInPeriod int not null default(31)
go

alter table ln.SusuConfig add
	regularSusuPeriodsInCycle int not null default(1)
go

alter table ln.regularSusuAccount add
	regularSusuAccountNo nvarchar(30) not null unique default(''),
	regularSusuGroupID int
go

alter table ln.regularSusuAccount add
	netAmountEntitled float not null default(0),
	interestAmount float not null default(0)
go

alter table ln.regularSusuAccount add
	modeOfPaymentID int,
	checkNo nvarchar(30),
	bankID int
go

alter table ln.regularSusuAccount add
	posted bit not null default(0)
go

alter table ln.regularSusuContribution add 
	checkNo nvarchar(30),
	bankID int
go

alter table ln.regularSusuContribution add 
	posted bit not null default(0)
go

alter table ln.regularSusuAccount add 
	authorized bit not null default(0)
go

alter table ln.SusuConfig add
	regularSusuContributionsPayableAccountID int
go

alter table ln.SusuConfig add
	regularSusuDaysDeductedPerPeriod int not null default(1)
go

alter table ln.regularSusuAccount add
	regularSusCommissionAmount float not null default(0)
go

alter table ln.regularSusuAccount add
	exited bit not null default(0),
	exitDate datetime null
go

alter table ln.regularSusuAccount add 
	exitApprovedBy nvarchar(30) not null default('') 
go

alter table ln.regularSusuAccount add
	commissionPaid float not null default(0),
	interestPaid float not null default(0),
	principalPaid float not null default(0)
go
  
create table ln.regularSusuWithdrawal
(
	regularSusuWithdrawalId int identity (1,1) not null primary key,
	regularSusuAccountId int not null,
	withdrawalDate datetime not null, 
	balance float not null default(0),
	amount float not null default(0),
	narration nvarchar(400) not null, 
)
go
