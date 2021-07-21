use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'countries')
	BEGIN
		DROP  Table countries
	END
GO
 