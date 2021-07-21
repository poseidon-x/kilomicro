use coreDB
go

create table iv.inventoryTransfer
(
	inventoryTransferId int not null identity (1,1) primary key,
	fromLocationId int not null,
	toLocationId int not null,
	requisitionDate datetime,
	approved bit not null default(0),
	approvedDate datetime,
	approver nvarchar(30),
	enteredBy nvarchar(30) not null,
	delivered bit not null default(0),
	deliveredDate datetime,
	receivedBy nvarchar(30),
	posted bit not null default(0),
	postedDate datetime,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

create table iv.inventoryTransferDetail
(
	inventoryTransferDetailId int not null identity(1,1) primary key,
	inventoryTransferId int not null,
	inventoryItemId bigint not null,
	quantityTransferred float not null,
	fromAccountId int not null,
	toAccountId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,
)
go

create table iv.inventoryTransferDetailLine
(
	inventoryTransferDetailLineId int not null identity(1,1) primary key,
	inventoryTransferDetailId int not null, 
	quantityTransferred float not null,
	batchNumber nvarchar(30),
	mfgDate datetime,
	expiryDate datetime,
	unitCost float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,
)
go

alter table iv.inventoryTransfer add
	postedBy nvarchar(30) null,
	postingComments nvarchar(100) null,
	approvalComments nvarchar(100) null
go

alter table iv.inventoryTransfer add
	deliveredBy nvarchar(30) null,
	deliveryCosmments nvarchar(100) null 
go

alter table iv.inventoryTransfer drop column 
	deliveryCosmments 
go

alter table iv.inventoryTransfer add 
	deliveryComments nvarchar(100) null ,
	receiptComments nvarchar(100) null ,
	receiptDate datetime null
go
