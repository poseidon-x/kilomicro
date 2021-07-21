use coreDB
go

create table iv.inventoryItem
(
	inventoryItemId bigint not null identity(1,1) primary key,
	productId int not null,
	locationId int not null,
	brandId int not null,
	itemNumber nvarchar(20) not null unique,
	inventoryItemName nvarchar(250),
	unitPrice float not null,
	safetyStockLevel float not null,
	reorderPoint float not null,
	accountId int not null,
	shrinkageAccountId int not null,
	inventoryMethodId int not null,
	currencyId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,
)
go

create table iv.inventoryItemDetail
(
	inventoryItemDetailId bigint not null identity(1,1) primary key,
	inventoryItemId bigint not null,
	batchNumber nvarchar(30),
	mfgDate datetime,
	expiryDate datetime,
	unitCost float not null,
	quantityOnHand float not null,
	reservedQuantity float not null default(0),
	startSerialNumber nvarchar(30),
	endSerialNumber nvarchar(30),
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,
)
go
