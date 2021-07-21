use coreDB
go

create  table crm.customer
(
	customerId bigint not null identity(1,1) primary key,
	customerNumber nvarchar(20) not null unique,
	customerName nvarchar(255) not null,
	paymentTermID int not null,
	currencyId int not null,
	balance float not null,
	balanceLocal float not null,
	contactPersonName nvarchar(100) not null,
	glAccountId int not null,
	locationId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table crm.customerShippingAddress
(
	customerShippingAddressId bigint identity (1,1) primary key,
	customerId bigint not null,
	shippingMethodId int not null,
	shipTo nvarchar(400) not null,
	addressLine1 nvarchar(400) not null,
	addressLine2 nvarchar(400) not null,
	cityName nvarchar(100) not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
	countryName nvarchar(100) not null,
)
go

create table crm.customerBusinessAddress
(
	customerBusinessAddressId bigint not null identity(1,1) primary key,
	addressTypeId int not null,
	customerId bigint not null,
	addressLine nvarchar(400) not null,
	landmark nvarchar(400) null,
	cityName nvarchar(100) not null,
	countryName nvarchar(100) not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table crm.customerPhone
(
	customerPhoneId bigint not null identity(1,1) primary key, 
	customerId bigint not null,
	mobilePhoneNumber nvarchar(200) not null, 
	landlinePhoneNumber nvarchar(200) not null, 
	faxNumber nvarchar(200) not null, 
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

drop table crm.customerEmail
go
create table crm.customerEmail
(
	customerEmailId bigint not null identity(1,1) primary key, 
	customerId bigint not null,
	emailAddress1 nvarchar(200) not null, 
	emailAddress2 nvarchar(200) not null, 
	emailAddress3 nvarchar(200) not null, 
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table crm.customerContactPerson
(
	customerContactPersonId bigint not null identity(1,1) primary key, 
	customerId bigint not null,
	contactPersonName nvarchar(100) not null,
	jobTitle nvarchar(100) not null,
	mobilePhoneNumber nvarchar(20) not null, 
	landlinePhoneNumber nvarchar(20) not null, 
	officeExtension nvarchar(10) not null,
	emailAddress nvarchar(20) not null, 
	skypeId nvarchar(100) not null, 
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go


alter table crm.customerContactPerson alter column emailAddress nvarchar(200) not null
go
