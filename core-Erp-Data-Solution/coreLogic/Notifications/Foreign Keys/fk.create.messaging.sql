use coreDB
go

alter table msg.messageEvent add
	constraint fk_mssageEvent_messageEventCategory foreign key (messageEventCategoryID)
	references msg.messageEventCategory(messageEventCategoryID)
go

alter table msg.messageTemplate add
	constraint fk_messageTemplate_messageEventCategory foreign key (messageEventCategoryID)
	references msg.messageEventCategory(messageEventCategoryID)
go

alter table msg.messagesSent add
	constraint fk_messagesSent_messageEvent foreign key (messageEventID)
	references msg.messageEvent(messageEventID)
go

alter table msg.messagesFailed add
	constraint fk_messagesFailed_messageEvent foreign key (messageEventID)
	references msg.messageEvent(messageEventID)
go

alter table msg.messagesFailed add
	constraint fk_messagesFailed_messagesFailureReason foreign key (messagesFailureReasonID)
	references msg.messageFailureReason(messageFailureReasonID)
go
