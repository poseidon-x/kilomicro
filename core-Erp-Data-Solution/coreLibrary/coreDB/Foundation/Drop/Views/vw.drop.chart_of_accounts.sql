use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'chart_of_accounts')
	BEGIN
		DROP  View chart_of_accounts
	END
GO
