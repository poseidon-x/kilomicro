use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_tx_by_acct')
	BEGIN
		DROP  View vw_tx_by_acct
	END
GO
