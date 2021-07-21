use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'cust_phons')
	BEGIN
		DROP  Table cust_phons
	END
GO
 