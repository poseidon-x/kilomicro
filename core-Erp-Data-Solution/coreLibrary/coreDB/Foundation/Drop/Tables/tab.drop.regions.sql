use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'regions')
	BEGIN
		DROP  Table regions
	END
GO
 