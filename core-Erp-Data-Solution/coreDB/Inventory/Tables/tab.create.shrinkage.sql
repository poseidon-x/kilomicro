use coreDB
go

create table iv.shrinkageBatch
(
	shrinkageBatchId bigint not null identity(1,1) primary key,
	shrinkageDate datetime not null,
	locationId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
	approved bit not null default(0),
	approvedBy nvarchar(30),
	enteredBy nvarchar(30) not null,
	posted bit not null default(0),
	postedDate datetime ,
)
go

create table iv.shrinkage
(
	shrinkageBatchId bigint not null,
	shrinkageId bigint not null identity(1,1) primary key,
	inventoryItemId bigint not null,
	quantityShrunk float not null,
	unitOfMeasurementId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go

alter table iv.shrinkageBatch add
	postedBy nvarchar(30) null,
	postingComments nvarchar(100) null,
	approvalComments nvarchar(100) null,
	approvalDate datetime null
go


create table iv.shrinkageReason
(
	shrinkageReasonId int not null identity(1,1) primary key,
	shrinkageReasonCode nvarchar(10) not null constraint uk_shrinkageReason_Code unique,
	shrinkageReasonName nvarchar(100) not null constraint uk_shrinkageReason_Name unique
)
go

alter table iv.shrinkage add
	shrinkageReasonId int null
go

alter table iv.shrinkageBatch add
	memo nvarchar(400) null
go

alter table iv.shrinkage add
	batchNumber nvarchar(30) null
go
