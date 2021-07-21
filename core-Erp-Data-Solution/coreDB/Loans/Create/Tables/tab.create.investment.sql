use coreDB
go

create table ln.investmentType
(
	investmentTypeID int identity(1,1) primary key,
	investmentTypeName nvarchar(100) not null unique,
	interestRate float not null,
	defaultPeriod int not null,
	allowsInterestWithdrawal bit not null default(0),
	allowsPrincipalWithdrawal bit not null default(0)
)
go

create table ln.investment
(
	investmentID int identity(1,1) primary key,
	clientID int not null,
	investmentTypeID int not null,
	amountInvested float not null,
	interestAccumulated float not null default(0),
	interestBalance float not null default(0),
	principalBalance float not null default(0),
	interestRate float not null,
	firstInvestmentDate datetime not null default(getdate()),
	period int not null,
	maturityDate datetime,
	autoRollover bit not null default(0),
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
)
go

create table ln.investmentInterest
(
	investmentInterestID int identity (1,1) primary key,
	investmentID int not null,
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


create table ln.investmentWithdrawal
(
	investmentWithdrawalID int identity(1,1) primary key,
	investmentID int not null,
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


create table ln.investmentAdditional
(
	investmentAdditionalID int identity(1,1) primary key,
	investmentID int not null,
	investmentDate datetime not null default(getdate()), 
	investmentAmount float not null,
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

alter table ln.investment add
	investmentNo nvarchar(30) not null unique
go


alter table ln.investment add
	lastInterestDate datetime
go

alter  table ln.investmentType add
	vaultAccountID int,
	accountsPayableAccountID int,
	interestExpenseAccountID int
go

alter table ln.investment add
	interestMethod bit not null default(0),
	interestRepaymentModeID int not null default(-1),
	principalRepaymentModeID int not null default(-1)
go

create table ln.investmentSchedule
(
	investmentScheduleID int identity(1,1) primary key,
	investmentID int not null,
	interestPayment float not null default(0),
	principalPayment float not null default(0),
	repaymentDate datetime not null,
	authorized bit not null default(0)
)
go

alter table ln.investmentInterest  add
	proposedAmount float not null default(0)
go

alter table ln.investment add
	principalAuthorized float not null default(0),
	interestAuthorized float not null default(0)
go


alter table ln.investmentSchedule add
	expensed bit not null default(0)
go

alter table ln.investmentSchedule add
	temp bit not null default(1)
go

alter table ln.investment add
	interestCalculationScheduleID int not null default(-1),
	fxRate float not null default(1),
	currencyID int not null default(1),
	localAmount float not null default(0)
go

alter table ln.investmentAdditional add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.investmentWithdrawal add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.investmentType add
	fxUnrealizedGainLossAccountID int not null default(0),
	fxRealizedGainLossAccountID int not null default(0),
	interestCalculationScheduleID int not null default(-1)
go

alter table ln.investmentInterest add
	fxRate float not null default(1),
	localAmount float not null default(0)
go
 
 alter table ln.investment add
	lastPrincipalFxGainLoss float not null default(0),
	lastInterestFxGainLoss float not null default(0)
go

alter table ln.investment add
	interestPayableAccountID int not null default(0)
go

alter table ln.investmentInterest add
	interestBalance float not null default(0)
go

alter table ln.investmentAdditional add
	lastPrincipalFxGainLoss float not null default(0)
go

alter table ln.investmentInterest add
	lastInterestFxGainLoss float not null default(0)
go

create table ln.investmentSignatory
(
	investmentSignatoryID int identity (1,1) not null primary key,
	investmentID int not null,
	fullName nvarchar(100) not null,
	signatureImageID int null
)
go

alter table ln.investment add
	interestExpected float not null default(0)
go


alter table ln.investmentAdditional add
	posted bit not null constraint df_investmentAdditional_posted default(1)
go

alter table ln.investmentAdditional 
	drop constraint df_investmentAdditional_posted 
go


alter table ln.investmentWithdrawal add
	posted bit not null constraint df_investmentWithdrawal_posted default(1)
go

alter table ln.investmentWithdrawal 
	drop constraint df_investmentWithdrawal_posted 
go
 
create table ln.investmentCharge
(
	investmentChargeID int identity(1,1) not null primary key,
	investmentID int not null,
	chargeTypeID int not null,
	chargeDate datetime not null,
	amount float not null,
	approvedBy nvarchar(30),
	creationDate datetime not null default(getdate()),
	memo ntext not null default('')
)
go

alter table ln.investmentAdditional add
	naration nvarchar(400) not null default ('Investment Made')
go

alter table ln.investmentWithdrawal add
	naration nvarchar(400) not null default ('Amount withdrawn from investment account')
go

alter table ln.investmentType add 
	chargesIncomeAccountID int null
go

alter table ln.investmentType add 
	interestReceivableAccountID int null
go

alter table ln.investment add
	staffID int null
go

alter table ln.investment add
	modern bit not null default(0)
go
 