use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_accounts')
	BEGIN
		DROP  View vw_accounts
	END
GO
