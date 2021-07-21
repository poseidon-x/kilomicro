

/** CREATE A STORED PROCEDURE TO QUEUE THE MESSAGE EVENTS**/

CREATE PROCEDURE msg.queue_message_event
(
@MessageCategoryId INT,
@ClientId INT,
@AccId INT,
@EventId INT,
@PhoneNumber NVARCHAR(30),
@MessageBody NVARCHAR(400),
@SenderName NVARCHAR(10),
@MessageDate DATETIME
)
AS
BEGIN;

	DECLARE @MessageId INT;

	INSERT INTO msg.messageEvent (messageEventCategoryID,clientID,accountID,eventID,phoneNumber,messageBody,sender,eventDate,finished)
	VALUES (@MessageCategoryId,@ClientId,@AccId,@EventId,@PhoneNumber,@MessageBody,@SenderName,@MessageDate,0);

	SELECT @MessageId=SCOPE_IDENTITY();

	SELECT @MessageId;

END;

/*
EXEC msg.queue_message_event
@MessageCategoryId=8,
@ClientId=1234,
@AccId=2345,
@EventId=2343,
@PhoneNumber='233247218146',
@MessageBody='Hello world',
@SenderName='KILO',
@MessageDate='2020-11-13 19:37'

*/

  --INSERT INTO msg.messageEventCategory (messageEventCategoryID,messageEventCategoryName,isEnabled) VALUES (9,'Load Disbursement',1);

  --INSERT INTO msg.messageTemplate (messageTemplateID,messageBodyTemplate,messageEventCategoryID) VALUES (9,'Dear $$FirstName$$,\nYour loan amount of Ghc $$AmountDisbursed$$ has been disbursed on $$DisbursementDate$$.\nContact 0247640868 for more information.',9);
