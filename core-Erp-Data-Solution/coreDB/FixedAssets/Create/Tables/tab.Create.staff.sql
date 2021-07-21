use coreDB
go

create table fa.staff
(
	staffID int identity (1,1) primary key,
	staffNo nvarchar(50) not null unique,
	surName nvarchar(50) not null,
	otherNames nvarchar(50) not null,
	DOB datetime,
	maritalStatusID int,
	sex nchar(1),
	staffCategoryID int,
	jobTitleID int,
	branchID int,
	ouID int
)
go

create table fa.staffAddress
(
	staffAddressID int identity (1,1) primary key, 
	staffID int not null, 
	addressID int, 
	addressTypeID int
)
go

create table fa.staffPhone
(
	staffPhoneID int identity (1,1) primary key, 
	staffID int not null, 
	phoneID int, 
	phoneTypeID int
)
go

create table fa.staffEmail
(
	staffEmailID int identity (1,1) primary key, 
	staffID int not null, 
	emailID int, 
	emailTypeID int
)
go


create  table fa.staffImage
(
	staffImageID int identity(1,1) primary key,
	staffID int , 
	imageID int 
)
go


create table fa.staffDocument
(
	staffDocumentID int identity(1,1) primary key,
	documentID int not null,
	staffID int not null
)
go

alter table fa.staff add
	userName nvarchar(30) null
go

alter table fa.staff add
	employmentStatusID int
go


alter table fa.staff add
	employmentStartDate datetime,
	employmentStopDate datetime
go

alter table fa.staff add
	lastLeaveDaysAccummulationDate datetime 
go

alter table fa.staff add
	lastPromotionDate datetime null
go

alter table fa.staff add companyId int
go
