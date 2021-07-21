use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'districts')
	BEGIN
		DROP  Table districts
	END
GO
 