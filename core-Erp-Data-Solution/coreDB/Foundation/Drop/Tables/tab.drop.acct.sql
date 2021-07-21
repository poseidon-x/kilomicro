use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'accts')
	BEGIN
		DROP  Table accts
	END
GO
 