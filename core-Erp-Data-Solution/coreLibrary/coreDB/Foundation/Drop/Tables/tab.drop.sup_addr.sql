use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'sup_addr')
	BEGIN
		DROP  Table sup_addr
	END
GO
 