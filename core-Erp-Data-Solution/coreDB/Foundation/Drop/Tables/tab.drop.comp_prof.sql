use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'comp_prof')
	BEGIN
		DROP  Table comp_prof
	END
GO
 