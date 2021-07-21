use coreDB
go

create table ar.arPayment
(
	arPaymentId bigint not null identity(1,1) primary key,
	customerId bigint null not null,
	paymentNumber nvarchar(12) null,
	paymentDate datetime not null,
	totalAmountPaid float not null,
	paymentCreditBalance bigint  not null,
	paymentMethodId int not null,
	checkNumber nvarchar(40) not null default(''),
	creditCardNumber nvarchar(400) not null default(''),--Please encrypt this columns
	mobileMoneyNumber nvarchar(30) not  null default(''),
	posted bit not null default(0),
	postedDate datetime ,
	currencyId int not null,
	totalAmountPaidLocal float not null,
	buyRate float not null,
	sellRate float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
	creditMemoId bigint null,
)
go

create table ar.arPaymentLine
(
	arPaymentLineId bigint not null identity(1,1) primary key,
	arPaymentId bigint not null,
	arinvoiceId bigint not null,
	amountPaid float not null,
	balanceLeft float not null default(0),
	amountLPaidLocal float not null,
	balanceLeftLocal float not null,
	accountId int not null, 
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
	creditMemoLineId bigint null,
)
go
