use coreDB
go

create table ln.industry
(
	industryID int identity(1,1) primary key,
	industryName nvarchar(100) not null unique
)
go


create table ln.institution
(
	institutionID int identity(1,1) primary key,
	institutionName nvarchar(100) not null unique
)
go

create table ln.sector
(
	sectorID int identity(1,1) primary key,
	sectorName nvarchar(100) not null unique
)
go


create table ln.branch
(
	branchID int identity(1,1) primary key,
	branchName nvarchar(100) not null unique
)
go


create table ln.category
(
	categoryID int primary key,
	categoryName nvarchar(100) not null unique
)
go


create table ln.maritalStatus
(
	maritalStatusID int primary key,
	maritalStatusName nvarchar(100) not null unique
)
go


create table ln.businessType
(
	businessTypeID int identity(1,1) primary key,
	businessTypeName nvarchar(100) not null unique
)
go

create table ln.employmentType
(
	employmentTypeID int primary key,
	employmentTypeName nvarchar(100) not null unique
)
go

alter table ln.branch add 
	vaultAccountID int not null default(0),
	bankAccountID int not null default(0),
	cashierTillAccountID int not null default(0),
	unearnedInterestAccountID int not null default(0),
	interestIncomeAccountID int not null default(0),
	unpaidCommissionAccountID int not null default(0),
	commissionAndFeesAccountID int not null default(0),
	accountsReceivableAccountID int not null default(0),
	unearnedExtraChargesAccountID int not null default(0)
go

alter table ln.branch add 
	gl_ou_id int null
go

alter table ln.branch add
	branchCode nvarchar(10) not null default ('100')
go

create table ln.productCode
(
	productCodeID int not null default(1) primary key check(productCodeID = 1),
	savingsAccountCode nvarchar(10) not null default('100'),
	loanAccountCode nvarchar(10) not null default('200'),
	depositAccountCode nvarchar(10) not null default('300'),
	groupSusuAccountCode nvarchar(10) not null default('400'),
	normalSusuAccountCode nvarchar(10) not null default('500'),
	investmentAccountCode nvarchar(10) not null default('600')
)
go
