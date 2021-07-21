use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'acct_heads')
	BEGIN
		DROP  Table acct_heads
	END
GO
 