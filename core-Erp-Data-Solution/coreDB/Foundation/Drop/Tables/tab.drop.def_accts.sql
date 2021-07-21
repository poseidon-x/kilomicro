use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'def_accts')
	BEGIN
		DROP  Table def_accts
	END
GO
 