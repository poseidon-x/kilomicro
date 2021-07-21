use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'past_currency_rates')
	BEGIN
		DROP  Table past_currency_rates
	END
GO
 