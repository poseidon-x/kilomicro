use coreDB
go

create table ln.microBusinessCategory
(	
	microBusinessCategoryID int identity(1,1) primary key,
	clientID int not null, 
	lineOfBusiness nvarchar(500),
	businessOwner nvarchar(500),
	dateEstablished datetime,
	numberOfCloseCompetitors int
)
go

create table ln.smeCategory
(	
	smeCategoryID int identity(1,1) primary key,
	clientID int not null, 
	companyName nvarchar(500),
	regNo nvarchar(50),
	registeredAddressID int,
	physicalAddressID int,
	regDate datetime,
	incDate datetime
)
go

create table ln.smeDirector
(	
	smeDirectorID int identity(1,1) primary key,
	smeCategoryID int,
	imageID int not null,
	phoneID int,
	emailID int,
	surName nvarchar(50),
	otherNames nvarchar(50),
	idNoID int
)
go

create table ln.groupCategory
(	
	groupCategoryID int identity(1,1) primary key,
	clientID int not null, 
	groupID int, 
	membershipNo nvarchar(50),
	joinDate datetime 
)
go

create table ln.[group]
(	
	groupID int identity(1,1) primary key,
	groupName nvarchar(500),
	groupSize int,
	addressID int
)
go

create table ln.groupExec
(	
	groupExecID int identity(1,1) primary key,
	groupID int, 
	phoneID int,
	emailID int,
	surName nvarchar(50),
	otherNames nvarchar(50),
	addressID int
)
go

create table ln.employer
(	 
	employerID int identity(1,1) primary key, 
	employmentTypeID int,
	employerName nvarchar(500),
	employerAddressID int
)
go

alter table ln.employer add
	officeNumber nvarchar(50)
go

create table ln.employerDirector
(	
	employerDirectorID int identity(1,1) primary key,
	employerID int,
	signatureImageID int,
	phoneID int,
	emailID int,
	surName nvarchar(50),
	otherNames nvarchar(50),
	idNoID int
)
go

create table ln.employeeCategory
(	
	employeeCategoryID int identity(1,1) primary key,
	employerID int ,
	clientID int not null
)
go

alter table ln.smeDirector alter column
	imageID int null
go

create table ln.lineOfBusiness
(
	lineOfBusinessID int identity(1,1) primary key,
	lineOfBusinessName nvarchar(100) not null unique
)
go

alter table ln.microBusinessCategory add
	lineOfBusinessID int null
go

alter table ln.employeeCategory add
	employerDirectorID int, 
	employmentTypeID int
go


alter table ln.[group] add
	groupTypeID int
go

create table ln.nextOfKin
(	
	nextOfKinID int identity(1,1) primary key,
	clientID int not null, 
	phoneID int,
	emailID int,
	surName nvarchar(50),
	otherNames nvarchar(50),
	relationship nvarchar(50),
	idNoID int
)
go

alter table ln.nextOfKin add
	imageID int null
go

create table ln.employeeContractType
(
	employeeContractTypeID int primary key,
	employeeContractTypeName nvarchar(100)
)
go

create table ln.staffCategory
(
	staffCategoryID int identity(1,1) primary key,
	clientID int not null,
	employerID int null,
	ssn nvarchar(50),
	employeeNumber nvarchar(50),
	employmentStartDate datetime,
	lengthOfService float not null default(0),
	position nvarchar(100),
	employeeContractTypeID int,
	employerDepartmentID int
)
go

create table ln.clientBankAccount
(
	clientBankAccountID int not null primary key identity(1,1),
	accountTypeID int not null,
	accountNumber nvarchar(50) not null unique,
	accountName nvarchar(250) not null,
	branchID int  not null,
	clientID int not null,
	isPrimary bit not null default(1)
)
go

create table ln.staffCategoryDirector
(
	staffCategoryDirectorID int identity(1,1) primary key,
	staffCategoryID int not null,
	employerDirectorID int not null
)
go

create table ln.employerDepartment
(
	employerDepartmentID int identity(1,1) not null primary key,
	employerID int not null,
	departmentName nvarchar(250)
)
go

alter table ln.staffCategory add
	employeeNumberOld nvarchar(50)
go

create table ln.region
(
	regionID int not null identity(1,1) primary key,
	regionName nvarchar(100) not null unique
)

alter table ln.staffCategory add
	regionID int null
go

alter table ln.staffCategory add
	empAddress1 nvarchar(250) null,
	empAddress2 nvarchar(250) null,
	empAddressCity nvarchar(250) null,
	authOfficerName nvarchar(250) null,
	authOfficerPosition nvarchar(250) null,
	authOfficerPhone nvarchar(250) null
go
