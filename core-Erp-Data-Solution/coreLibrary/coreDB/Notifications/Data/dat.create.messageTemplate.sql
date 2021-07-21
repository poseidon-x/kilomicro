USE [coreDB]
GO

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
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(3
,'Dear $$FIRST_NAME$$,
Transaction Advice
Acc. Num:$$ACCOUNT_NUMBER$$
Tran Type:Credit
Amount: $$AMOUNT$$ 
Date: $$DATE$$
CLR Bal:$$BALANCE$$
Naration:$$NARATION$$
'
,3)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(4
,'Dear $$FIRST_NAME$$,
Transaction Advice
Acc. Num:$$ACCOUNT_NUMBER$$
Tran Type:Credit
Amount: $$AMOUNT$$ 
Date: $$DATE$$
CLR Bal:$$BALANCE$$
Naration:$$NARATION$$
'
,4)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(5
,'Dear $$FIRST_NAME$$,
Transaction Advice
Acc. Num:$$ACCOUNT_NUMBER$$
Tran Type:Debit
Amount: $$AMOUNT$$ 
Date: $$DATE$$
CLR Bal:$$BALANCE$$
Naration:$$NARATION$$
'
,5)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(6
,'Dear $$FIRST_NAME$$,
Transaction Advice
Acc. Num:$$ACCOUNT_NUMBER$$
Tran Type:Debit
Amount: $$AMOUNT$$ 
Date: $$DATE$$
CLR Bal:$$BALANCE$$
Naration:$$NARATION$$
'
,6)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(7
,'Dear $$FIRST_NAME$$,
Please be reminded of your $$STATUS$$ loan repayment
Acc. Num:$$ACCOUNT_NUMBER$$ 
Amount: $$AMOUNT$$ 
Due Date: $$DATE$$ 
'
,7)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(8
,'Dear $$FIRST_NAME$$,
JIREH MICROFINANCE WISHES YOU A HAPPY BIRTHDAY AND MANY HAPPY RETURNS 
'
,8)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(9
,'Dear $$FIRST_NAME$$,
Welcome to the Jireh Family.
Please call 0302-500958 for any assistance.'
,9)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(10
,'Dear $$FIRST_NAME$$,
Please be informed your investment matures in the next three days
----0302-500958.'
,10)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(11
,'Dear $$FIRST_NAME$$,
Please be informed your investment interest payment is due in the next three days
----0302-500958.'
,11)
INSERT INTO [msg].[messageTemplate]
           ([messageTemplateID]
           ,[messageBodyTemplate]
           ,[messageEventCategoryID])
VALUES
(12
,'Dear $$FIRST_NAME$$,
Please be informed your investment interest has been rolled over
----0302-500958.'
,12)
GO

