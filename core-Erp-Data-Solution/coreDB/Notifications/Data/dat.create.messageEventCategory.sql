USE [coreDB]
GO

INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (1
           ,'Loans Repayments'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (2
           ,'Loans Approval'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (3
           ,'Savings Deposit'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (4
           ,'Investment Deposit'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (5
           ,'Savings Withdrawal'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (6
           ,'Investment Withdrawal'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (7
           ,'Upcoming Loan Repayment'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (13
           ,'Client Birthday'
           ,1)
INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (9
           ,'Client Welcome'
           ,1)
	INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (10
           ,'Investment maturity'
           ,1)
	INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (11
           ,'Investment payment due'
           ,1)
	INSERT INTO [msg].[messageEventCategory]
           ([messageEventCategoryID]
           ,[messageEventCategoryName]
           ,[isEnabled])
     VALUES
           (12
           ,'Investment rollover'
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