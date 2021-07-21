use coreDB
go

create table ln.savingType
(
	savingTypeID int identity(1,1) primary key,
	savingTypeName nvarchar(100) not null unique,
	interestRate float not null,
	defaultPeriod int not null,
	allowsInterestWithdrawal bit not null default(0),
	allowsPrincipalWithdrawal bit not null default(0)
)
go

create table ln.saving
(
	savingID int identity(1,1) primary key,
	clientID int not null,
	savingTypeID int not null,
	amountInvested float not null,
	interestAccumulated float not null default(0),
	interestBalance float not null default(0),
	principalBalance float not null default(0),
	interestRate float not null,
	firstSavingDate datetime not null default(getdate()),
	period int not null,
	maturityDate datetime,
	autoRollover bit not null default(0),
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
)
go

create table ln.savingInterest
(
	savingInterestID int identity (1,1) primary key,
	savingID int not null,
	principal float not null,
	interestAmount float not null,
	interestRate float not null,
	fromDate datetime,
	toDate Datetime,
	interestDate datetime not null default(getdate()),
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
)
go


create table ln.savingWithdrawal
(
	savingWithdrawalID int identity(1,1) primary key,
	savingID int not null,
	withdrawalDate datetime not null default(getdate()),
	interestWithdrawal float not null,
	principalWithdrawal float not null,
	principalBalance float not null,
	interestBalance float not null,
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0),
	bankID int,
	checkNo nvarchar(30),
	modeOfPaymentID int not null
)
go


create table ln.savingAdditional
(
	savingAdditionalID int identity(1,1) primary key,
	savingID int not null,
	savingDate datetime not null default(getdate()), 
	savingAmount float not null,
	principalBalance float not null,
	interestBalance float not null,
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0),
	bankID int,
	checkNo nvarchar(30),
	modeOfPaymentID int not null
)
go

alter table ln.saving add
	savingNo nvarchar(30) not null unique
go


alter table ln.saving add
	lastInterestDate datetime
go

alter  table ln.savingType add
	vaultAccountID int,
	accountsPayableAccountID int,
	interestExpenseAccountID int
go

alter table ln.saving add
	interestMethod bit not null default(0),
	interestRepaymentModeID int not null default(-1),
	principalRepaymentModeID int not null default(-1)
go

create table ln.savingSchedule
(
	savingScheduleID int identity(1,1) primary key,
	savingID int not null,
	interestPayment float not null default(0),
	principalPayment float not null default(0),
	repaymentDate datetime not null,
	authorized bit not null default(0)
)
go

alter table ln.savingInterest  add
	proposedAmount float not null default(0)
go

alter table ln.saving add
	principalAuthorized float not null default(0),
	interestAuthorized float not null default(0)
go


alter table ln.savingSchedule add
	expensed bit not null default(0)
go

alter table ln.savingSchedule add
	temp bit not null default(1)
go

update ln.savingSchedule set temp=0
go

create table ln.savingRollOver
(
	savingRollOverID int identity(1,1) primary key,
	savingID int not null,
	interestPayment float not null default(0),
	principalPayment float not null default(0),
	rollOverDate datetime not null 
)
go

alter table ln.saving add
	interestCalculationScheduleID int not null default(-1),
	fxRate float not null default(1),
	currencyID int not null default(1),
	localAmount float not null default(0)
go

alter table ln.savingAdditional add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.savingWithdrawal add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.savingType add
	fxUnrealizedGainLossAccountID int not null default(0),
	fxRealizedGainLossAccountID int not null default(0),
	interestCalculationScheduleID int not null default(-1)
go

alter table ln.savingInterest add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.saving add
	lastPrincipalFxGainLoss float not null default(0),
	lastInterestFxGainLoss float not null default(0)
go

alter table ln.savingType add
	interestPayableAccountID int not null default(0)
go

alter table ln.savingInterest add
	interestBalance float not null default(0)
go

alter table ln.savingAdditional add
	lastPrincipalFxGainLoss float not null default(0)
go

alter table ln.savingInterest add
	lastInterestFxGainLoss float not null default(0)
go

create table ln.savingSignatory
(
	savingSignatoryID int identity (1,1) not null primary key,
	savingID int not null,
	fullName nvarchar(100) not null,
	signatureImageID int null
)
go

alter table ln.saving add
	interestExpected float not null default(0)
go


alter table ln.savingAdditional add
	posted bit not null constraint df_savingAdditional_posted default(1)
go

alter table ln.savingAdditional 
	drop constraint df_savingAdditional_posted 
go


alter table ln.savingWithdrawal add
	posted bit not null constraint df_savingWithdrawal_posted default(1)
go

alter table ln.savingWithdrawal 
	drop constraint df_savingWithdrawal_posted 
go

create table ln.savingCharge
(
	savingChargeID int identity(1,1) not null primary key,
	savingID int not null,
	chargeTypeID int not null,
	chargeDate datetime not null,
	amount float not null,
	approvedBy nvarchar(30),
	creationDate datetime not null default(getdate()),
	memo ntext not null default('')
)
go

alter table ln.savingAdditional add
	naration nvarchar(400) not null default ('Deposit Made')
go

alter table ln.savingWithdrawal add
	naration nvarchar(400) not null default ('Amount withdrawn from savings account')
go

alter table ln.saving add
	savingPlanID int not null default(0),
	savingPlanAmount float not null default(0)
go

create table ln.savingPlan
(
	savingPlanID int identity(1,1) not null primary key,
	savingID int not null,
	plannedDate datetime not null,
	plannedAmount float not null,
	deposited bit not null default(0),
	creator nvarchar(30) not null,
	creationDate datetime not null,
	modifier nvarchar(30) null,
	modificationDate datetime null
)
go

alter table ln.savingPlan add
	amountDeposited float not null  default(0)
go

alter table ln.savingType add 
	chargesIncomeAccountID int null
go


create table ln.savingDailyInterest
(
	savingDailyInterestID int identity (1,1) not null primary key,
	savingID int not null,
	interestDate datetime not null,
	applied bit not null default(0),
	balanceAsAt float not null default(0),
	interestAmount float not null default(0)
)
go

create table ln.savingConfig
(
	savingConfigID int identity(1,1) not null primary key,
	savingTypeID int not null,
	interestDecimalPlaces tinyint not null default(6),
	accrueInterestToPrincipal bit not null default(0),
	principalBalanceLatency int not null default(0),
	interestBalanceLatency int not null default(0),
	calculateInterest bit not null default(0)
)
go

alter table ln.saving add
	availablePrincipalBalance float not null default(0),
	clearedPrincipalBalance float not null default(0),
	availableInterestBalance float not null default(0),
	clearedInterestBalance float not null default(0)
go

alter table ln.saving add
	staffID int null
go

create table ln.staffSaving
(
	staffSavingID int identity(1,1) not null primary key,
	staffID int not null unique,
	savingID int not null 
)
go

alter table ln.savingType add
	minPlanAmount float null,
	maxPlanAmount float null,
	planID tinyint null
go

create table ln.savingPlanFlag
(
	savingPlanFlagID bigint not null identity(1,1) primary key,
	savingID int not null,
	flagDate datetime not null,
	currentPlanId tinyint not null,
	proposedPlanId  tinyint not null,
	approved bit null,
	approvedBy nvarchar(30) null,
	approvedDate datetime null,
	applied bit null,
	appliedDate datetime null
)
go

alter table ln.savingType add
	earlyWithdrawalChargeRate float null
go

alter table ln.savingAdditional add
	closed bit not null default (0)
go

alter table ln.savingWithdrawal add
	closed bit not null default (0)
go

update ln.savingAdditional set closed=posted
go

update ln.savingWithdrawal set closed=posted
go

alter table ln.savingType add
	minDaysBeforeInterest float not null default(0)
go


alter table ln.savingAdditional add
	posted bit not null default(0)
go


alter table ln.savingWithdrawal add
	posted bit not null default(0)
go

update ln.savingAdditional set posted=1 where creation_date<'2014-10-28'
update ln.savingWithdrawal set posted=1 where creation_date<'2014-10-28'

alter table ln.saving add
	agentId int null
go

create table ln.savingNextOfKin
(
	savingNextOfKinId int identity(1,1) primary key,
	savingId int not null,
	dob datetime null,
	otherNames nvarchar(50) not null,
	surName nvarchar(50) not null,
	relationshipType nvarchar(50) not null,
	idTypeId int not null,
	idNumber nvarchar(20) not null,
	percentageAllocated float not null,
	picture image null
)
go

alter table ln.savingNextOfKin add
	phoneNumber nvarchar(20) null
go

alter table ln.saving
add reservedAmount float not null default(0)
