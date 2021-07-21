use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'cust_addr')
	BEGIN
		DROP  Table cust_addr
	END
GO
 