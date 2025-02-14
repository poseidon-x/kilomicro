﻿use coreDB
go

create table ar.arInvoice
(
	arInvoiceId bigint identity (1,1) primary key,
	salesOrderId bigint null,
	jobCardId bigint null,
	customerId bigint null,
	invoiceNumber nvarchar(20) not null unique,
	customerName nvarchar(250) null, 
	invoiceDate datetime not null,
	totalAmount float not null,
	balance float not null, 
	paidDate datetime,
	paid bit not null default(0),
	isVat bit not null default(0),
	isNHIL bit not null default(0),
	vatRate float not null default(0),
	nhilRate float not null default(0),
	withRate float not null default(0),
	isWith bit not null default(0),
	paymentTermId int null,
	invoiceStatusId int not null,
	posted bit not null default(0),
	postedDate datetime,
	currencyId int not null,
	balanceLocal float not null,
	totalAmountLocal float not null, 
	buyRate float not null,
	sellRate float not null,
	accountId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table ar.arInvoiceLine
(
	arInvoiceLineId bigint identity (1,1) primary key, 
	arInvoiceId bigint not null,
	inventoryItemId bigint null,
	lineNumber int not null,
	description nvarchar(400) not null,
	acct_id int null,
	unitPrice float not null,
	quantity float not null,
	unitOfMeasurementId int not null,
	discountAmount float not null default(0),
	discountPercentage float not null default(0),
	totalAmount float not null,
	netAmount float not null,
	isVat bit not null default(0),
	isNHIL bit not null default(0),
	isWith bit not null default(0),
	unitPriceLocal float not null,
	discountAmountLocal float not null,
	totalAmountLocal float not null,
	netAmountLocal float not null,
	accountId int not null,
	discountAccountId int not null,
	vatAccountId int not null,
	nhilAccountId int not null,
	withAccountId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

