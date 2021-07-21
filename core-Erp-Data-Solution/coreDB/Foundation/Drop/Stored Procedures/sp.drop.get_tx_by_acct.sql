use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'get_tx_by_acct')
	BEGIN
		DROP  procedure get_tx_by_acct
	END
GO