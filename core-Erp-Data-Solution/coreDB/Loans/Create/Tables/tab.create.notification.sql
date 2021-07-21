use coreDB
go

create table ln.[notification]
(
	notificationID int identity(1,1) not null primary key,
	notificationCode nvarchar(10) not null unique,
	notificationName nvarchar(100) not null unique,
	[description] ntext not null default(''),
	isActive bit not null default(0)
)
go

create table ln.[notificationSchedule]
(
	notificationScheduleID int identity(1,1) not null primary key,
	notificationID int not null,
	dayOfWeekID int not null,
	frequencyID int  not null,
	startTime time,
	endTime time
)
go

create table ln.notificationRecipient
(
	notificationRecipientID int identity(1,1) not null primary key,
	notificationID int not null,
	staffID int not null
)
go

alter table ln.notificationRecipient add
	email nvarchar(250) not null default('')
go