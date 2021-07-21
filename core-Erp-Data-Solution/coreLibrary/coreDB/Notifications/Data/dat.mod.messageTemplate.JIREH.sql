use coreDB
go

update msg.messageEventCategory set
	isEnabled=0
where messageEventCategoryID in (1,2)
go

update msg.messageTemplate set
	[messageBodyTemplate]='Dear $$FIRST_NAME$$
Txn:$$TRANSACTION_TYPE$$
Amt:$$AMOUNT$$
Acct:$$ACCOUNT_NUMBER$$
Cur Bal:$$BALANCE$$
Date:$$DATE$$
By:$$NARATION$$'
where messageTemplateID in (3,4,5,6)	
go
  
update msg.messageTemplate set
	[messageBodyTemplate]='Dear $$FIRST_NAME$$
Jireh team appreciates your patronage and wish you a Happy Birthday.'
where messageTemplateID in (8)	
go

update msg.messageTemplate set
	[messageBodyTemplate]='Dear $$FIRST_NAME$$
Please be reminded of your $$STATUS$$loan payment.'
where messageTemplateID in (7)	
go

update msg.messageTemplate set
	[messageBodyTemplate]='Dear $$FIRST_NAME$$
Welcome to the Jireh Family.
Please call 0302-500958 for any assistance.'
where messageTemplateID in (9)	
go

update msg.messageTemplate set
	[messageBodyTemplate]='Dear $$FIRST_NAME$$
Please be informed your investment matures in the next three days
----0302-500958'
where messageTemplateID in (10)	
go


