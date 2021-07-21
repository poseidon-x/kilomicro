use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'sup_types')
	BEGIN
		DROP  Table sup_types
	END
GO
 