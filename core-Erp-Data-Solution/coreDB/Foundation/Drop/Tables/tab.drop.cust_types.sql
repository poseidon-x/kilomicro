use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'cust_types')
	BEGIN
		DROP  Table cust_types
	END
GO
 