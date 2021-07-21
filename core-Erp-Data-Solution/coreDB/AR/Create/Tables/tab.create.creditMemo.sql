use coreDB
go

create table ar.creditMemo
(
	creditMemoId bigint not null identity(1,1) primary key,
	memoNumber nvarchar(12) not null,
	arInvoiceId bigint null,
	arPaymentId bigint null,
	customerId bigint not null,
	memoDate datetime not null,
	totalAmountReturned float not null,
	totalBalanceLeft float not null default(0),
	approved bit not null default(0),
	approvedBy nvarchar(30) null,
	posted bit not null default(0),
	postedDate datetime,
	creditMemoReasonId int not null,
	currencyId int not null,
	totalAmountReturnedLocal float not null,
	buyRate float not null,
	sellRate float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table ar.creditMemoLine
(
	creditMemoLineId bigint not null identity(1,1) primary key,
	creditMemoId bigint not null,
	arinvoiceLineId bigint null,
	amountReturned float not null,
	quantityReturned float not null,
	balanceLeft float not null default(0),
	amountReturnedLocal float not null,
	balanceLeftLocal float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table ar.creditMemoReason
(
	creditMemoReasonId int identity (1,1) not null primary key,
	creditMemoReasonName nvarchar(255) not null unique	
)
go
