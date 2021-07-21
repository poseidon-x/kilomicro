use coreDB
go

create table ln.cashiersTill
(
	cashiersTillID int identity(1,1) primary key,
	userName nvarchar(30) not null unique,
	accountID int not null
)
go

create table ln.cashiersTillDay
(
	cashiersTillDayID int identity(1,1) primary key,
	cashiersTillID int not null,
	[open] bit not null default(0),
	tillDay datetime not null default(getdate()),
	creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
	creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
	modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
	last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
)
go

create table ln.cashierDisbursement
(
	cashierDisbursementID int identity(1,1) primary key,
	cashierTillID int not null,
	txDate datetime not null,
	amount float not null,
	clientID int not null,
	loanID int not null,
	posted bit not null default(0),
	bankID int null,
	paymentModeID int not null,
	checkNo nvarchar(50) null
)
go

create table ln.cashierReceipt
(
	cashierReceiptID int identity(1,1) primary key,
	cashierTillID int not null,
	txDate datetime not null,
	amount float not null,
	clientID int not null,
	loanID int not null,
	posted bit not null default(0),
	bankID int null,
	paymentModeID int not null,
	checkNo nvarchar(50) null,
	repaymentTypeID int not null
)
go


alter table ln.cashierReceipt add
	interestAmount float not null default(0),
	feeAmount float not null default(0),
	addInterestAmount float not null default(0)
go

alter table ln.cashierReceipt add
	principalAmount float not null default(0),
	remainderAmount float not null default(0)
go

alter table ln.cashierDisbursement add
	addFees bit not null default(0) 
go

alter table ln.cashierReceipt add
	writeOff bit not null default(0) 
go

alter table ln.cashierReceipt add
	tillAccountID int,
	batchNo1 nvarchar(50),
	batchNo2 nvarchar(50),
	batchNo3 nvarchar(50),
	batchNo4 nvarchar(50),
	batchNo5 nvarchar(50),
	closed bit not null default(0)
go

create table ln.multiPayment
(
	multipaymentID int identity(1,1) primary key,
	cashierReceiptID int not null,
	description nvarchar(500),
	amountPaid float not null,
	disbursementDate datetime not null,
	interestOutstanding float not null,
	principalOutstanding float not null,
	processingFee float not null,
	invoiceNo nvarchar(50) null,
	loanID int not null,
	loanNo nvarchar(50) not null
)
go


create table ln.multiPaymentClient
(
	multiPaymentClientID int identity(1,1) primary key,
	cashierReceiptID int not null,
	clientName nvarchar(400) not null,
	amount float not null,
	invoiceDate datetime not null
)
GO

alter table ln.multiPayment add
	multiPaymentClientID int null
go

alter table ln.cashierDisbursement add
	postToSavingsAccount bit not null default(0)
go

alter table ln.multiPaymentClient add
	approved bit not null default(1),
	refunded bit not null default(1),
	balance float not null default(0)
go

alter table ln.multiPaymentClient add
	checkAmount float not null default(0)
go

alter table ln.multiPaymentClient add
	posted bit not null default(0)
go
