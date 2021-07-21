use coreDB
go

create index idx_messageEvent_Category on msg.messageEvent
(
	messageEventCategoryID
)
go

create index idx_messageEvent_owner on msg.messageEvent
(
	clientID,
	eventID,
	accountID
)
go

create index idx_messagesSent_date on msg.messagesSent
(
	sentDate
)
go

create index idx_messagesFailed_date on msg.messagesFailed
(
	attemptDate,
	messagesFailureReasonID
)
go
