use coreDB
go

create table iv.openningBalanceBatch
(
	openningBalanceBatchId bigint not null identity(1,1) primary key,
	balanceDate datetime not null,
	locationId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
	enteredBy nvarchar(30) not null,
	posted bit not null default(0),
	postedDate datetime,
	approved bit not null,
	approvedBy nvarchar(50) not null,
)
go

create table iv.openningBalance
(
	openningBalanceId bigint not null identity(1,1) primary key,
	openningBalanceBatchId bigint not null,
	inventoryItemId bigint not null,
	quantityOnHand float not null,
	productId int not null,
	brandId int not null,
	unitOfMeasurementId int not null,
	drAccountId int,
	crAccountId int,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

alter table iv.openningBalanceBatch add
	postedBy nvarchar(30) null,
	postingComments nvarchar(100) null,
	approvalComments nvarchar(100) null,
	approvalDate datetime null
go

alter table iv.openningBalance add
	batchNumber nvarchar(30) null
go
