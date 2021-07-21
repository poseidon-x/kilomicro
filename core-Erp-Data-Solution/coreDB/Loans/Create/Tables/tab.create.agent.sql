use coreDB
go

create table ln.agent
(
	agentID int identity (1,1) primary key,
	agentNo nvarchar(50) not null unique,
	surName nvarchar(50) not null,
	otherNames nvarchar(50) not null,
	DOB datetime, 
	sex nchar(1),  
	accountTypeID int null,
	accountNumber nvarchar(50) null ,
	accountName nvarchar(250) null,
	bankBranchID int  null,
	branchID int 
)
go

create table ln.agentAddress
(
	agentAddressID int identity (1,1) primary key, 
	agentID int not null, 
	addressID int, 
	addressTypeID int
)
go

create table ln.agentPhone
(
	agentPhoneID int identity (1,1) primary key, 
	agentID int not null, 
	phoneID int, 
	phoneTypeID int
)
go

create table ln.agentEmail
(
	agentEmailID int identity (1,1) primary key, 
	agentID int not null, 
	emailID int, 
	emailTypeID int
)
go


create  table ln.agentImage
(
	agentImageID int identity(1,1) primary key,
	agentID int , 
	imageID int 
)
go


create table ln.agentDocument
(
	agentDocumentID int identity(1,1) primary key,
	documentID int not null,
	agentID int not null
)
go
 
create table ln.agentNextOfKin
(	
	nextOfKinID int identity(1,1) primary key,
	agentID int not null, 
	phoneID int,
	emailID int,
	surName nvarchar(50),
	otherNames nvarchar(50),
	relationship nvarchar(50),
	idNoID int
)
go

alter table ln.agentNextOfKin add
	imageID int null
go
