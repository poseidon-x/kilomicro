use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_unb_tx')
	BEGIN
		DROP  View vw_unb_tx
	END
GO
