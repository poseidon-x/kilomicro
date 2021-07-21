use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'banks')
	BEGIN
		DROP  Table banks
	END
GO
 