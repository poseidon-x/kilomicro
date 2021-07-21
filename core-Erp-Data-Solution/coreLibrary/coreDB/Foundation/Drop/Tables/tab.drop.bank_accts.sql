use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'bank_accts')
	BEGIN
		DROP  Table bank_accts
	END
GO
 