use coreDB
go

create table ln.clientActivityType
(
	clientActivityTypeID int identity(1,1) not null primary key,
	clientActivityTypeName nvarchar(100) not null unique
)
go

create table ln.clientActivityLog
(
	clientActivityLogID int identity(1,1) not null primary key,
	clientActivityTypeID int not null,
	activityDate datetime not null,
	activityNotes ntext not null,
	nextActionDate datetime null,
	nextAction ntext null,
	clientID int not null,
	loanID int null,
	responsibleStaffID int null,
	creator nvarchar(30),
	creationDate datetime
)
go

alter table ln.clientActivityLog add
	completed bit not null default(0)
go