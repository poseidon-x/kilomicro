USE [coreDB]
GO

INSERT INTO [msg].[messageFailureReason]
           ([messageFailureReasonID]
           ,[messageFailureReasonName])
     VALUES
           (1
           ,'GENERAL_HTTP_SMS_SENDING_ERROR')
GO

