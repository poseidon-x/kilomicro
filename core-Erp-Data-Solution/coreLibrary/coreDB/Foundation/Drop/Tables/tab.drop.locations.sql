use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'locations')
	BEGIN
		DROP  Table locations
	END
GO
 