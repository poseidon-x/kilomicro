use coreDB
go

create table momo.momoProvider
(
	providerID int identity(1,1) not null primary key,
	providerFullName nvarchar(100) not null unique,
	providerShortName nvarchar(10 ) not null unique,
	momoProductName nvarchar(100) not null unique
)
go

create table momo.momoWallet
(
	walletID int identity (1,1) not null primary key,
	cashiersTillID int not null,
	providerID int not null,
	accountNumber nvarchar(30) not null unique,
	walletAccountID int not null,
	chargesIncomeAccountID int not null,
	chargesExpenseAccountID int not null,
	enteredBy nvarchar(50) not null,
	entryDate datetime not null,
	balance float not null,
	[version] rowversion not null
)
go

create table momo.walletLoading
(
	walletLoadingID bigint not null identity(1,1) primary key,
	walletID int not null,
	modeOfPaymentID int not null,
	bankID int null,
	loadingDate datetime not null,
	amountLoaded float not null,
	balanceAfter float not null,
	enteredBy nvarchar(30) not null,
	entryDate datetime not null,
	posted bit not null,
	postedBy nvarchar(30) null,
	postingDate datetime null,
	[version] rowversion not null 
)
go

create table momo.walletCashup
(
	walletCashupID bigint identity(1,1) not null primary key,
	walletID int not null,
	modeOfPaymentID int not null,
	bankID int null,
	cashupDate datetime not null,
	amount float not null,
	chargesAmount float not null,
	balanceAfter float not null,
	destAccountID int not null,
	enteredBy nvarchar(30) not null,
	entryDate datetime not null,
	posted bit not null,
	postedBy nvarchar(30) null,
	postingDate datetime null,
	[version] rowversion not null
)
go

create table momo.momoService
(
	serviceID int identity(1,1) not null primary key,
	providerID int not null,
	serviceName nvarchar(100) not null
)
go

create table momo.momoServiceCharge
(
	serviceChargeID int identity(1,1) not null primary key,
	serviceID int not null,
	minTranAmount float not null,
	maxTranAmount float not null,
	chargesValue float not null,
	isPercent bit not null
)
go

create table momo.momoTransaction
(
	transactionID bigint identity(1,1) not null primary key,
	walletID int not null,
	serviceID int not null,
	momoClientID bigint not null,
	modeOfPaymentID int not null,
	bankID int null,
	clientPhoneNumber nvarchar(15) not null,
	clientName nvarchar(100) not null,
	tranAmount float not null,
	chargesAmount  float not null,
	transactionDate datetime not null,
	enteredBy nvarchar(30) not null,
	entryDate datetime not null,
	posted bit not null,
	postedBy nvarchar(30) null,
	postingDate datetime null,
	[version] rowversion not null
)
go

create table momo.momoClient
(
	momoClientID bigint not null identity(1,1) primary key,
	clientName nvarchar(100) not null,
	clientPhoneNumber nvarchar(15) not null unique
)
go
