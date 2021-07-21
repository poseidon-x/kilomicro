use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'close_period')
	BEGIN
		DROP  Procedure  close_period
	END

GO
