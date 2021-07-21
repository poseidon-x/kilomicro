use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'bank_branches')
	BEGIN
		DROP  Table bank_branches
	END
GO
 