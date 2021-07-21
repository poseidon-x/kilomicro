use coreDB
go

create table jc.workOrder
(
	workOrderId bigint identity (1,1) primary key,
	customerId bigint not null,
	workOrderNumber nvarchar(20) not null,
	workOrderDate datetime not null,
	[status] bit not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)

create table jc.workOrderActivity
(
	workOrderActivityId bigint identity (1,1) primary key,
	workOrderId bigint not null,
	activityCode nvarchar(10) not null,
	specialityId tinyInt not null,
	hourlyBillingRate float not null,
	hourlyCost float not null,
	markup float not null,
	billable bit not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null
)