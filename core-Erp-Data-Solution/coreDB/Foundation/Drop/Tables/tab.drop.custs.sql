use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'custs')
	BEGIN
		DROP  Table custs
	END
GO
 