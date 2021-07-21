use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'on_update_currencies')
	BEGIN
		DROP  Trigger on_update_currencies
	END
GO
 