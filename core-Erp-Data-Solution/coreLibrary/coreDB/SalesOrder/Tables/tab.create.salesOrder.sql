use coreDB
go

create table so.salesOrder
(
	salesOrderId bigint identity (1,1) primary key,
	customerId bigint null,
	customerName nvarchar(250) null, 
	orderNumber nvarchar(20) not null unique,
	salesDate datetime not null,
	totalAmount float not null,
	balance float not null,
	requiredDate datetime not null,
	shippedDate datetime,
	locationId int,
	salesTypeId int not null,
	currencyId int not null,
	buyRate float not null,
	sellRate float not null,
	totalAmountLocal float not null,
	balanceLocal float not null,
	accountId int null,
	paymentTermId int not null,
	isInvoiced bit null,
	invoicedDate datetime not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table so.salesOrderline
(
	salesOrderLineId bigint identity (1,1) primary key,
	salesOrderId bigint not null,
	inventoryItemId bigint null,
	lineNumber int not null,
	description nvarchar(400) not null,
	accountId int null,
	unitPrice float not null,
	quantity float not null,
	unitOfMeasurementId int not null,
	discountAmount float not null default(0),
	discountPercentage float not null default(0),
	totalAmount float not null,
	netAmount float not null,
	unitPriceLocal float not null,
	totalAmountLocal float not null,
	netAmountLocal float not null,
	discountAmountLocal float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table so.salesOrderShipping
(
	salesOrderShippingId bigint identity (1,1) primary key,
	salesOrderId bigint not null,
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

create table so.salesOrderBilling
(
	salesOrderBillingId bigint identity (1,1) primary key,
	salesOrderId bigint not null, 
	billTo nvarchar(400) not null,
	addressLine1 nvarchar(400) not null,
	addressLine2 nvarchar(400) not null,
	cityName nvarchar(100) not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go
