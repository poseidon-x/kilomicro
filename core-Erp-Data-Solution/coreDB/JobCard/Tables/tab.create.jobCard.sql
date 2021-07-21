use coreDB
go


create table jc.jobCard
(
	jobCardId bigint identity (1,1) primary key,
	jobNumber nvarchar(20) not null,
	revisionNumber bigint null,
	jobDate datetime not null,
	orderStartingDate datetime not null,
	customerId bigint not null,
	workOrderNumber nvarchar(20) not null,
	standardMarkUpRate float not null,
	standardHourlyBillingRate float not null,
	totalLabour float not null,
	totalMaterial bigint not null,
	vat float not null,
	nhil float not null,
	approved bit not null,
	approvedBy nvarchar(50) null,
	approvelDate datetime null,
	invoiced bit not null,
	invoicedBy nvarchar(50) null,
	invoiceDate datetime null,
	signed bit not null,
	signedBy nvarchar(50) null,
	signDate datetime null,
	fulfilled bit not null,
	fulfilledBy nvarchar(50) null,
	fulfillmentDate datetime null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,
)

create table jc.jobCardMaterialDetail
(
	jobCardMaterialDetailId int identity (1,1) primary key,
	jobCardId bigint not null,
	serialNumber nvarchar(40) not null,
	materialDescription nvarchar(50) not null,
	inventoryItemId bigint not null,
	partNumber nvarchar(50) not null,
	quantity bigint not null,
	unitOfMeasurementId int not null,
	unitCost float not null,
	materialCost float not null,
	markup float not null,
	materialCharge float not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null,	
)

create table jc.jobCardLabourDetail
(
	jobCardLabourDetailId bigint identity (1,1) primary key,
	jobCardId bigint not null,
	labourDate datetime not null,
	productionLineDescription nvarchar(100) not null,
	starTime datetime not null,
	endTime datetime not null,
	activityCode nvarchar(10) not null,
	totalHours smallint not null,
	billable bit not null,
	billableHours smallint null,
	billingRate float null,
	billingValue float null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)



