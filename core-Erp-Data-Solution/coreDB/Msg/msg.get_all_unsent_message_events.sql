

GO
DROP PROCEDURE IF EXISTS msg.get_all_unsent_message_events;

GO
CREATE PROCEDURE msg.get_all_unsent_message_events
AS
BEGIN;

	 SELECT TOP 100
			me.messageEventID,
			me.messageEventCategoryID,
			me.clientID,
			me.accountID,
			me.eventID,
			me.phoneNumber,
			me.messageBody,
			me.sender,
			me.eventDate,
			me.finished
	FROM msg.messageEvent me
	WHERE me.finished=0 AND me.eventDate < CURRENT_TIMESTAMP;

END;




GO
DROP PROCEDURE IF EXISTS  msg.get_disbursement_sms_template;

GO
CREATE PROCEDURE msg.get_disbursement_sms_template
AS
BEGIN;
	SELECT mt.messageBodyTemplate 
	FROM msg.messageTemplate mt	
	WHERE mt.messageEventCategoryID=9;
END;



GO
DROP PROCEDURE IF EXISTS  msg.mark_sms_as_sent;

GO
CREATE PROCEDURE msg.mark_sms_as_sent
(
@MessageEventId INT
)
AS
BEGIN;

	UPDATE msg.messageEvent
	SET finished=1
	WHERE messageEventID=@MessageEventId;

	INSERT INTO msg.messagesSent (messageEventID,sentDate)
	VALUES (@MessageEventId,CURRENT_TIMESTAMP);

	SELECT @MessageEventId;
END;



GO
DROP PROCEDURE IF EXISTS  msg.get_sms_config;

GO
CREATE PROCEDURE msg.get_sms_config
(
@MessageConfigId INT
)
AS
BEGIN;

	SELECT 
			mc.messagingConfigID,
			mc.httpMessagingUrl,
			mc.httpMessagingUserName,
			mc.httpMessagingPassword,
			mc.messagingSender,
			mc.maxMessageLength,
			mc.maxNarationLength,
			mc.loanRepaymentNotificationCycle,
			mc.numberOfDaysBeforeLoanOverdue
	FROM msg.messagingConfig mc
	WHERE mc.messagingConfigID =@MessageConfigId;

END;
