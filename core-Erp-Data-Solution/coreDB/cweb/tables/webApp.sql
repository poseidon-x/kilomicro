use coreDB
go

create table webApp
(
	webAppID int identity (1,1) not null primary key,
	surName nvarchar(50) not null,
	otherNames nvarchar(50) not null,
	dob datetime,
	maritalStatusID int,
	sex nchar(1),
	categoryID int not null,
	clientTypeID int not null,
	branchID int,
	approved bit not null default(0),
	denied bit not null default(0)
)
go

create table webAppID
(
	webAppID int not null primary key,
	primaryIDTypeID int,
	primaryIDNo nvarchar(50),
	primaryIDExpiryDate datetime,
	secondaryIDTypeID int,
	secondaryIDNo nvarchar(50),
	secondaryIDExpiryDate datetime,
)
go

create table webAppPhoto
(
	webAppID int not null primary key,
	photo image
)
go

create table webAppPhyAddr
(
	webAppID int not null primary key,
	addrLine1 nvarchar(250)	,
	addrLine2 nvarchar(250)	,
	city nvarchar(50)
)
go

create table webAppMailAddr
(
	webAppID int not null primary key,
	addrLine1 nvarchar(250)	,
	addrLine2 nvarchar(250)	,
	city nvarchar(50)
)
go

create table webAppContact
(
	webAppID int not null primary key,
	mobilePhone nvarchar(50),
	workPhone nvarchar(50),
	homePhone nvarchar(50),
	officeEmail nvarchar(250),
	personalEmail nvarchar(250)
)
go

create table webAppEmp
(
	webAppID int not null primary key,
	employerID int,
	departmentID int,
	authOfficerName nvarchar(50),
	authOfficerPosition nvarchar(100),
	authOfficerPhone nvarchar(50),
	contractTypeID int
)
go

create table webAppEmp2
(
	webAppID int not null primary key,
	employeeNumber nvarchar(50),
	oldEmployeeNumber nvarchar(50),
	socialSecurityNo nvarchar(50),
	position nvarchar(100),
	employmentStartDate datetime,
	empAddr1 nvarchar(250),
	empAddr2 nvarchar(250),
	empAddrCity nvarchar(50)
)
go

create table webAppBank
(
	webAppID int not null primary key,
	bankAccountTypeID int,
	bankAccountName nvarchar(100),
	bankAccountNo nvarchar(50),
	bankID int,
	bankBranchID int
)
go

create table webAppSalary
(
	webAppID int not null primary key,
	basicSalary float,
	socialSecurityWelfare float,
	tax float,
	thirdPartyDeductions float,
	deductionsNotOnPayslip float,
	totalAllowances float
)
go

alter table webApp add
	user_name nvarchar(30) not null
go