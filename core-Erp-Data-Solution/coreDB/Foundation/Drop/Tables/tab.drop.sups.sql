use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'sups')
	BEGIN
		DROP  Table sups
	END
GO
 