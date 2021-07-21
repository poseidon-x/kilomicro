use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'def_acct_names')
	BEGIN
		DROP  Table def_acct_names
	END
GO
 