use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'cities')
	BEGIN
		DROP  Table cities
	END
GO
 