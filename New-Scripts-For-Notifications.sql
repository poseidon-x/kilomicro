insert into dbo.roles(role_name,description,is_active,creation_date,creator,modification_date,last_modifier) 
values ('Owner','System Owner',1,'2019-12-23',NULL,NULL,NULL);

insert into dbo.user_roles(user_name,role_name,creation_date) values ('coreAdmin','Owner','2019-12-23');

insert into dbo.user_roles(user_name,role_name,creation_date) values ('chris','Owner','2019-12-23');



--NOTIFICATION TABLES--

create schema [msg]


CREATE TABLE [msg].[messageEventCategory](
	[messageEventCategoryID] [int] NOT NULL,
	[messageEventCategoryName] [nvarchar](50) NOT NULL,
	[isEnabled] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[messageEventCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[messageEventCategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [msg].[messageEventCategory] ADD  DEFAULT ((0)) FOR [isEnabled]
GO




CREATE TABLE [msg].[messageEvent](
	[messageEventID] [int] IDENTITY(1,1) NOT NULL,
	[messageEventCategoryID] [int] NOT NULL,
	[clientID] [int] NOT NULL,
	[accountID] [int] NOT NULL,
	[eventID] [int] NOT NULL,
	[phoneNumber] [nvarchar](30) NOT NULL,
	[messageBody] [nvarchar](400) NOT NULL,
	[sender] [nvarchar](10) NOT NULL,
	[eventDate] [datetime] NOT NULL,
	[finished] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[messageEventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [msg].[messageEvent] ADD  DEFAULT (getdate()) FOR [eventDate]
GO

ALTER TABLE [msg].[messageEvent] ADD  DEFAULT ((0)) FOR [finished]
GO

ALTER TABLE [msg].[messageEvent]  WITH CHECK ADD  CONSTRAINT [fk_mssageEvent_messageEventCategory] FOREIGN KEY([messageEventCategoryID])
REFERENCES [msg].[messageEventCategory] ([messageEventCategoryID])
GO

ALTER TABLE [msg].[messageEvent] CHECK CONSTRAINT [fk_mssageEvent_messageEventCategory]
GO




CREATE TABLE [msg].[messageTemplate](
	[messageTemplateID] [int] NOT NULL,
	[messageBodyTemplate] [nvarchar](400) NOT NULL,
	[messageEventCategoryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[messageTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [msg].[messageTemplate]  WITH CHECK ADD  CONSTRAINT [fk_messageTemplate_messageEventCategory] FOREIGN KEY([messageEventCategoryID])
REFERENCES [msg].[messageEventCategory] ([messageEventCategoryID])
GO

ALTER TABLE [msg].[messageTemplate] CHECK CONSTRAINT [fk_messageTemplate_messageEventCategory]
GO



CREATE TABLE [msg].[messagingConfig](
	[messagingConfigID] [int] NOT NULL,
	[httpMessagingUrl] [nvarchar](400) NOT NULL,
	[httpMessagingUserName] [nvarchar](200) NOT NULL,
	[httpMessagingPassword] [nvarchar](200) NOT NULL,
	[messagingSender] [nvarchar](10) NOT NULL,
	[maxMessageLength] [smallint] NOT NULL,
	[maxNarationLength] [tinyint] NOT NULL,
	[loanRepaymentNotificationCycle] [smallint] NOT NULL,
	[numberOfDaysBeforeLoanOverdue] [smallint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[messagingConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [msg].[messagingConfig]  WITH CHECK ADD CHECK  (([loanRepaymentNotificationCycle]>(0) AND [loanRepaymentNotificationCycle]<(60)))
GO

ALTER TABLE [msg].[messagingConfig]  WITH CHECK ADD CHECK  (([maxMessageLength]>(0) AND [maxMessageLength]<(400)))
GO

ALTER TABLE [msg].[messagingConfig]  WITH CHECK ADD CHECK  (([maxNarationLength]>(0) AND [maxNarationLength]<(80)))
GO

--ALTER TABLE [msg].[messagingConfig]  WITH CHECK ADD CHECK  (([messagingConfigID]=(1)))
--GO

ALTER TABLE [msg].[messagingConfig]  WITH CHECK ADD CHECK  (([numberOfDaysBeforeLoanOverdue]>(0) AND [numberOfDaysBeforeLoanOverdue]<(60)))
GO




CREATE TABLE [msg].[messagesSent](
	[messagesSentID] [int] IDENTITY(1,1) NOT NULL,
	[messageEventID] [int] NOT NULL,
	[sentDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[messagesSentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [msg].[messagesSent]  WITH CHECK ADD  CONSTRAINT [fk_messagesSent_messageEvent] FOREIGN KEY([messageEventID])
REFERENCES [msg].[messageEvent] ([messageEventID])
GO

ALTER TABLE [msg].[messagesSent] CHECK CONSTRAINT [fk_messagesSent_messageEvent]
GO




CREATE TABLE [msg].[messageFailureReason](
	[messageFailureReasonID] [int] NOT NULL,
	[messageFailureReasonName] [nvarchar](400) NULL,
PRIMARY KEY CLUSTERED 
(
	[messageFailureReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[messageFailureReasonName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO





CREATE TABLE [msg].[messagesFailed](
	[messagesFailedID] [int] IDENTITY(1,1) NOT NULL,
	[messageEventID] [int] NOT NULL,
	[attemptDate] [datetime] NOT NULL,
	[messagesFailureReasonID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[messagesFailedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [msg].[messagesFailed]  WITH CHECK ADD  CONSTRAINT [fk_messagesFailed_messageEvent] FOREIGN KEY([messageEventID])
REFERENCES [msg].[messageEvent] ([messageEventID])
GO

ALTER TABLE [msg].[messagesFailed] CHECK CONSTRAINT [fk_messagesFailed_messageEvent]
GO

ALTER TABLE [msg].[messagesFailed]  WITH CHECK ADD  CONSTRAINT [fk_messagesFailed_messagesFailureReason] FOREIGN KEY([messagesFailureReasonID])
REFERENCES [msg].[messageFailureReason] ([messageFailureReasonID])
GO

ALTER TABLE [msg].[messagesFailed] CHECK CONSTRAINT [fk_messagesFailed_messagesFailureReason]
GO

--INSERTS--


INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (1
           ,'Loans Repayments'
           ,1)
GO

INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (2
           ,'Loans Approval'
           ,1)
GO

INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (3
           ,'Savings Deposit'
           ,1)
GO



INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (4
           ,'Investment Deposit'
           ,1)
GO

INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (5
           ,'Savings Withdrawal'
           ,1)
GO

INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (7
           ,'Upcoming Loan Repayment'
           ,1)
GO


  INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (8
           ,'Mini Statement'
           ,0)
GO

  update msg.messageEventCategory set isEnabled=1 where messageEventCategoryID=8;
--Templates--



INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (1
           ,'Dear $$FIRST_NAME$$,
We are delighted to have received your $$PAYMENT_TYPE$$ payment of $$AMOUNT$$ on your loan account:$$ACCOUNT_NUMBER$$.
We hope to continue to prosper together.
'
           ,1)
GO


INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (2
           ,'Dear $$FIRST_NAME$$,
We are delighted to inform you that an amount of $$AMOUNT$$ has been approved for your loan account:$$ACCOUNT_NUMBER$$.
We hope to continue to prosper together.
'
           ,2)
GO

INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (3
           ,'Dear $$FIRST_NAME$$
Txn:$$TRANSACTION_TYPE$$
Amt:$$AMOUNT$$
Acct:$$ACCOUNT_NUMBER$$
Cur Bal:$$BALANCE$$
Date:$$DATE$$
By:$$NARATION$$'
           ,3)
GO

INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (4
           ,'Dear $$FIRST_NAME$$
Txn:$$TRANSACTION_TYPE$$
Amt:$$AMOUNT$$
Acct:$$ACCOUNT_NUMBER$$
Cur Bal:$$BALANCE$$
Date:$$DATE$$
By:$$NARATION$$'
           ,4)
GO

INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (5
           ,'Dear $$FIRST_NAME$$
Txn:$$TRANSACTION_TYPE$$
Amt:$$AMOUNT$$
Acct:$$ACCOUNT_NUMBER$$
Cur Bal:$$BALANCE$$
Date:$$DATE$$
By:$$NARATION$$'
           ,5)
GO

INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (7
           ,'Dear $$FIRST_NAME$$
Please be reminded of your $$STATUS$$loan payment.'
           ,7)
GO


INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
     VALUES
           (8
           ,'Dear {ClientName},\nLoan Bal: GH₵{LoanBalance:n2},\nSecurity Bal: GH₵{SecurityBalance:n2},\nAcct: {AccountNumber},\nInstallments: GH₵{TotalPaidOff},\nDate: {MessageDate}'
           ,8)
GO


--NOTIFICATION CONFIG--

INSERT INTO [msg].[messagingConfig]
           ([messagingConfigID]
           ,[httpMessagingUrl]
           ,[httpMessagingUserName]
           ,[httpMessagingPassword]
           ,[messagingSender]
           ,[maxMessageLength]
           ,[maxNarationLength]
           ,[loanRepaymentNotificationCycle]
           ,[numberOfDaysBeforeLoanOverdue])
     VALUES
           (1
           ,'https://api.smsgh.com/v3/messages/send?From=$$SENDER$$&To=$$PHONE_NUMBER$$&Content=$$MESSAGE_BODY$$&ClientId=eqdcvyxo&ClientSecret=ulmfmzzm&RegisteredDelivery=true'
           ,'na1-eamparbeng'
           ,'2bon2bti'
           ,'ACS-TEST'
           ,157
           ,25
           ,30
           ,30)
GO

INSERT INTO [msg].[messagingConfig]
           ([messagingConfigID]
           ,[httpMessagingUrl]
           ,[httpMessagingUserName]
           ,[httpMessagingPassword]
           ,[messagingSender]
           ,[maxMessageLength]
           ,[maxNarationLength]
           ,[loanRepaymentNotificationCycle]
           ,[numberOfDaysBeforeLoanOverdue])
     VALUES
           (2
           ,'https://app.helliomessaging.com/api/v2/sms'
           ,'5df8d07c183dc'
           ,'3cdf7715926211c3611f68fcc0f92b66'
           ,'LENDZEE'
           ,158
           ,26
           ,30
           ,30)
GO

INSERT INTO [msg].[messagingConfig]
           ([messagingConfigID]
           ,[httpMessagingUrl]
           ,[httpMessagingUserName]
           ,[httpMessagingPassword]
           ,[messagingSender]
           ,[maxMessageLength]
           ,[maxNarationLength]
           ,[loanRepaymentNotificationCycle]
           ,[numberOfDaysBeforeLoanOverdue])
     VALUES
           (3
           ,'https://app.helliomessaging.com/api/sms?username=$$USER$$&password=$$PASSWORD$$&senderId=$$SENDER$$&msisdn=$$MSISDN$$&message=$$MESSAGE$$'
           ,'AdapativeCS'
           ,'B3@ut1fu1'
           ,'Lendzee'
           ,158
           ,26
           ,30
           ,30)
GO

  INSERT INTO [msg].[messagingConfig]
           ([messagingConfigID]
           ,[httpMessagingUrl]
           ,[httpMessagingUserName]
           ,[httpMessagingPassword]
           ,[messagingSender]
           ,[maxMessageLength]
           ,[maxNarationLength]
           ,[loanRepaymentNotificationCycle]
           ,[numberOfDaysBeforeLoanOverdue])
     VALUES
           (4
           ,'https://app.helliomessaging.com/api/sms?username=$$USER$$&password=$$PASSWORD$$&senderId=$$SENDER$$&msisdn=$$MSISDN$$&message=$$MESSAGE$$'
           ,'wendolin'
           ,'wEN.0247218146'
           ,'Lendzee'
           ,158
           ,26
           ,30
           ,30)
GO

