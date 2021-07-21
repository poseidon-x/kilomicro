USE [coreDB]
GO

INSERT INTO [msg].[messagingConfig]
           ([messagingConfigID]
           ,[httpMessagingUrl]
           ,[httpMessagingUserName]
           ,[httpMessagingPassword]
           ,[messagingSender])
     VALUES
           (1
           ,'http://216.224.161.207/api/sender/?username=$$USERNAME$$&password=$$PASSWORD$$&type=0&dlr=1&destination=$$PHONE_NUMBER$$&source=$$SENDER$$&message=$$MESSAGE_BODY$$'
           ,'na1-eamparbeng'
           ,'2bon2bti'
           ,'JIREHMFL')
GO

