use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_acct__acct_head')
	BEGIN
		alter table accts DROP  constraint fk_acct__acct_head
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_acct_cur')
	BEGIN
		alter table accts DROP  constraint fk_acct_cur
	END
GO
 