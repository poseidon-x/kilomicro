use coreDBtest
go

create table ln.client
(
	clientID int identity (1,1) primary key,
	accountNumber nvarchar(50) not null unique,
	surName nvarchar(50) not null,
	otherNames nvarchar(50) not null,
	DOB datetime,
	maritalStatusID int,
	sex nchar(1),
	industryID int,
	sectorID int,
	branchID int,
	categoryID int,
	idNoID int
)
go

create table ln.clientAddress
(
	clientAddressID int identity (1,1) primary key, 
	clientID int not null, 
	addressID int, 
	addressTypeID int
)
go

create table ln.clientPhone
(
	clientPhoneID int identity (1,1) primary key, 
	clientID int not null, 
	phoneID int, 
	phoneTypeID int
)
go

create table ln.clientEmail
(
	clientEmailID int identity (1,1) primary key, 
	clientID int not null, 
	emailID int, 
	emailTypeID int
)
go

create table ln.clientBusinessActivity
(
	clientBusActID int identity (1,1) primary key, 
	clientID int not null, 
	revenue float, 
	businessTypeID int,
	businessName nvarchar(100),
	profitMargin float
)
go

create table ln.clientLiability
(
	clientLiabilityID int identity (1,1) primary key, 
	clientID int not null, 
	amountBorrowed float, 
	institutionID int,
	businessName nvarchar(100),
	outstandingBalance float,
	repaymentDate datetime
)
go

create table ln.clientImage
(
	clientImageID int identity(1,1) primary key,
	clientID int , 
	imageID int 
)
go

alter table ln.client add
	clientTypeID int not null default(0)
go

alter table ln.client add
	idNoID2 int null
go

alter table ln.client add
	companyName nvarchar(250) not null default(''),
	isCompany bit not null default(0)
go

create table ln.clientCompany
(
	clientID int not null primary key,
	contactSurname nvarchar(50),
	contactOtherNames nvarchar(50),
	companyName nvarchar(250),
	businessAddressID int,
	emailID int,
	phoneID int
)
go








create table ln.clientType
(
	clientTypeId int not null primary key,
	clientTypeName nvarchar(100) not null unique,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) null,
	modified datetime null
)
go

insert into ln.clientType(clientTypeId,clientTypeName,creator,created)
values
(0,'Individual Loan Client', 'coreadmin', '2015-09-14 00:00:00'),
(1,'Individual Investor Client', 'coreadmin', '2015-09-14 00:00:00'),
(2,'Individual Loan + Investor Client', 'coreadmin', '2015-09-14 00:00:00'),
(3,'Investment Taking Client', 'coreadmin', '2015-09-14 00:00:00'),
(4,'Corporate Loan Client', 'coreadmin', '2015-09-14 00:00:00'),
(5,'Corporate Investor Client', 'coreadmin', '2015-09-14 00:00:00'),
(6,'Joint Loan + Investor Account', 'coreadmin', '2015-09-14 00:00:00'),
(7,'Individual + Group Loan Client', 'coreadmin', '2015-09-14 00:00:00')

go


