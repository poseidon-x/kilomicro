use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'open_period')
	BEGIN
		DROP  Procedure  open_period
	END

GO
