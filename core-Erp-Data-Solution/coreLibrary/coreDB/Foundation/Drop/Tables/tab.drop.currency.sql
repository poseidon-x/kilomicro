use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'currencies')
	BEGIN
		DROP  Table currencies
	END
GO
 