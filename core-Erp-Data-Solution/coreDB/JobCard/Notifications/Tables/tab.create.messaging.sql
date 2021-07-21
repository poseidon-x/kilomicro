use coreDB
go

create table msg.messageEventCategory
(
	messageEventCategoryID int not null primary key,
	messageEventCategoryName nvarchar(50) not null unique,
	isEnabled bit not null default(0)
)
go

create table msg.messageEvent
(
	messageEventID int identity(1,1) not null primary key,
	messageEventCategoryID int not null,
	clientID int not null,
	accountID int not null,
	eventID int not null,
	phoneNumber nvarchar(30) not null,
	messageBody nvarchar(400) not null,
	sender nvarchar(10) not null,
	eventDate datetime not null default(getdate()),
	finished bit not null default(0)
)
go

create table msg.messageTemplate
(
	messageTemplateID int not null primary key,
	messageBodyTemplate nvarchar(400) not null,
	messageEventCategoryID int not null
)
go

create table msg.messagesSent
(
	messagesSentID int identity(1,1) not null primary key,
	messageEventID int not null,
	sentDate datetime not null
)
go

create table msg.messageFailureReason
(
	messageFailureReasonID int not null primary key,
	messageFailureReasonName nvarchar(400) unique
)
go

create table msg.messagesFailed
(
	messagesFailedID int identity(1,1) not null primary key,
	messageEventID int not null,
	attemptDate datetime not null,
	messagesFailureReasonID int not null
)
go

create table msg.messagingConfig
(
	messagingConfigID int not null primary key check(messagingConfigID=1),
	httpMessagingUrl nvarchar(400) not null,
	httpMessagingUserName nvarchar(20) not null,
	httpMessagingPassword nvarchar(30) not null,
	messagingSender nvarchar(10) not null
)
go

