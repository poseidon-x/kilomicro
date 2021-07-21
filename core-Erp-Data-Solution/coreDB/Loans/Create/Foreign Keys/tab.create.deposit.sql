use coreDB
go

create table ln.depositType
(
	depositTypeID int identity(1,1) primary key,
	depositTypeName nvarchar(100) not null unique,
	interestRate float not null,
	defaultPeriod int not null,
	allowsInterestWithdrawal bit not null default(0),
	allowsPrincipalWithdrawal bit not null default(0)
)
go

create table ln.deposit
(
	depositID int identity(1,1) primary key,
	clientID int not null,
	depositTypeID int not null,
	amountInvested float not null,
	interestAccumulated float not null default(0),
	interestBalance float not null default(0),
	principalBalance float not null default(0),
	interestRate float not null,
	firstDepositDate datetime not null default(getdate()),
	period int not null,
	maturityDate datetime,
	autoRollover bit not null default(0),
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
)
go

create table ln.depositInterest
(
	depositInterestID int identity (1,1) primary key,
	depositID int not null,
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


create table ln.depositWithdrawal
(
	depositWithdrawalID int identity(1,1) primary key,
	depositID int not null,
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


create table ln.depositAdditional
(
	depositAdditionalID int identity(1,1) primary key,
	depositID int not null,
	depositDate datetime not null default(getdate()), 
	depositAmount float not null,
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

alter table ln.deposit add
	depositNo nvarchar(30) not null unique
go


alter table ln.deposit add
	lastInterestDate datetime
go

alter  table ln.depositType add
	vaultAccountID int,
	accountsPayableAccountID int,
	interestExpenseAccountID int
go

alter table ln.deposit add
	interestMethod bit not null default(0),
	interestRepaymentModeID int not null default(-1),
	principalRepaymentModeID int not null default(-1)
go

create table ln.depositSchedule
(
	depositScheduleID int identity(1,1) primary key,
	depositID int not null,
	interestPayment float not null default(0),
	principalPayment float not null default(0),
	repaymentDate datetime not null,
	authorized bit not null default(0)
)
go

alter table ln.depositInterest  add
	proposedAmount float not null default(0)
go

alter table ln.deposit add
	principalAuthorized float not null default(0),
	interestAuthorized float not null default(0)
go




