use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'sup_phons')
	BEGIN
		DROP  Table sup_phons
	END
GO
 