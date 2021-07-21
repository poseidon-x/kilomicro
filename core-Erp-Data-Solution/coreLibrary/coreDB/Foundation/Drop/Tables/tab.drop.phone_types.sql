use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'phone_types')
	BEGIN
		DROP  Table phone_types
	END
GO
 