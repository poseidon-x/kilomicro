use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'modules')
	BEGIN
		DROP  Table modules
	END
GO
 