use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'get_unb_tx')
	BEGIN
		DROP  procedure get_unb_tx
	END
GO