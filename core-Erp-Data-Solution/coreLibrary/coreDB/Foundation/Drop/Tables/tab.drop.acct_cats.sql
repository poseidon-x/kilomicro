use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'acct_cats')
	BEGIN
		DROP  Table acct_cats
	END
GO
 