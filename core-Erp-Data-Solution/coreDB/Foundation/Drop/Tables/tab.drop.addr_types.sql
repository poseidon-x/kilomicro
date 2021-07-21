use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'addr_types')
	BEGIN
		DROP  Table addr_types
	END
GO
 