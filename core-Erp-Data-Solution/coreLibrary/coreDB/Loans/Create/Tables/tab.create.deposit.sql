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

create table ln.depositTypeAllowedTenure
(
	depositTypeAllowedTenureId int identity(1,1) primary key,
	depositTypeId int not null,
	tenureTypeId int not null,
	minTenure int not null,
	maxTenure int not null	 
)

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


alter table ln.depositSchedule add
	expensed bit not null default(0)
go

alter table ln.depositSchedule add
	temp bit not null default(1)
go

alter table ln.deposit add
	interestCalculationScheduleID int not null default(-1),
	fxRate float not null default(1),
	currencyID int not null default(1),
	localAmount float not null default(0)
go

alter table ln.depositAdditional add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.depositWithdrawal add
	fxRate float not null default(1),
	localAmount float not null default(0)
go

alter table ln.depositType add
	fxUnrealizedGainLossAccountID int not null default(0),
	fxRealizedGainLossAccountID int not null default(0),
	interestCalculationScheduleID int not null default(-1)

go

alter table ln.depositInterest add
	fxRate float not null default(1),
	localAmount float not null default(0)
go
 
 alter table ln.deposit add
	lastPrincipalFxGainLoss float not null default(0),
	lastInterestFxGainLoss float not null default(0)
go

alter table ln.deposit add
	interestPayableAccountID int not null default(0)
go

alter table ln.depositInterest add
	interestBalance float not null default(0)
go

alter table ln.depositAdditional add
	lastPrincipalFxGainLoss float not null default(0)
go

alter table ln.depositInterest add
	lastInterestFxGainLoss float not null default(0)
go

create table ln.depositSignatory
(
	depositSignatoryID int identity (1,1) not null primary key,
	depositID int not null,
	fullName nvarchar(100) not null,
	signatureImageID int null
)
go

alter table ln.deposit add
	interestExpected float not null default(0)
go


alter table ln.depositAdditional add
	posted bit not null constraint df_depositAdditional_posted default(1)
go

alter table ln.depositAdditional 
	drop constraint df_depositAdditional_posted 
go


alter table ln.depositWithdrawal add
	posted bit not null constraint df_depositWithdrawal_posted default(1)
go

alter table ln.depositWithdrawal 
	drop constraint df_depositWithdrawal_posted 
go

create table ln.chargeType
(
	chargeTypeID int identity(1,1) primary key not null,
	chargeTypeName nvarchar(100) not null constraint uk_chargeTpe_chargeTypeName unique
)
go

create table ln.depositCharge
(
	depositChargeID int identity(1,1) not null primary key,
	depositID int not null,
	chargeTypeID int not null,
	chargeDate datetime not null,
	amount float not null,
	approvedBy nvarchar(30),
	creationDate datetime not null default(getdate()),
	memo ntext not null default('')
)
go

alter table ln.depositAdditional add
	naration nvarchar(400) not null default ('Deposit Made')
go

alter table ln.depositWithdrawal add
	naration nvarchar(400) not null default ('Amount withdrawn from deposit account')
go

alter table ln.depositType add 
	chargesIncomeAccountID int null
go

alter table ln.depositType add 
	interestPayableAccountID int null,
	fixedRate bit not null default(0)
go

alter table ln.deposit add
	staffID int null
go

alter table ln.deposit add
	modern bit not null default(0)
go


alter table ln.depositAdditional add
	posted bit not null default(0)
go


alter table ln.depositWithdrawal add
	posted bit not null default(0)
go

update ln.depositAdditional set posted=1 where creation_date<'2014-10-28'
update ln.depositWithdrawal set posted=1 where creation_date<'2014-10-28'

alter table ln.deposit add
	agentId int null
go





create table ln.clientCheck
(
	clientCheckId int not null identity(1,1) primary key,
	clientId int not null unique,
	creator nvarchar(30) not null,
	created datetime not null
)
go

create table ln.clientCheckDetail
(
	clientCheckDetailId int not null identity(1,1) primary key,
	clientCheckId int not null,
	checkNumber nvarchar(30) not null,
	bankId int not null,
	sourceBankAccountId int null,
	checkAmount float not null,
	checkDate datetime not null,
	cashed bit not null default (0),
	cashedDate datetime null,
	balance float not null,
	clearingAttempts tinyint default(0),
	creator nvarchar(30) not null,
	created datetime not null,
	modifier nvarchar(30) null,
	modified datetime null
)
go

create table ln.checkApply
(
	checkApplyId int not null identity(1,1) primary key,
	clientCheckDetailId int not null,
	depositId int not null,
	amountApplied float not null,
	appliedDate datetime not null,
	posted bit not null default(0),
	postedBy nvarchar(30) null,
	creator nvarchar(30) not null,
	created datetime not null,
	modifier nvarchar(30) null,
	modified datetime null
)
go






--add by Manager 19-OCT-2015
create table ln.clientInvestmentReceipt
(	
	clientInvestmentReceiptId int not null identity(1,1) primary key,
	clientId int not null unique,
)

create table ln.clientInvestmentReceiptDetail
(
	clientInvestmentReceiptDetailId int not null identity(1,1) primary key,
	clientInvestmentReceiptId int not null,
	amountReceived float not null,
	receivedBy nvarchar(50) not null,
	receiptDate datetime not null,
	invested bit not null,
	investedBy nvarchar(50) null,
	investedDate datetime null,
	created datetime not null,
	modifier nvarchar(50) null,
	modified datetime null
)

--add by Manager 23-OCT-2015
create table ln.depositConfig
(
	depositConfigId int identity not null primary key,
	interestWithDrawBySav bit not null default(0)
)

insert into ln.depositConfig (interestWithDrawBySav) 
			values(0)

create table ln.depositRepaymentMode
(
	depositRepaymentModeId int identity not null primary key,
	repaymentModeName nvarchar(60) not null,
	repaymentModeDays smallint not null
)

insert into ln.depositRepaymentMode (repaymentModeName,repaymentModeDays) 
			values('Monthly',30),
				  ('Quarterly',90),
				  ('Half-Yearly',180),
				  ('At Maturity',-1) 


