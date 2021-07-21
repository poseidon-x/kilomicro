use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_past_currency_rates')
	BEGIN
		alter table past_currency_rates DROP  constraint fk_past_currency_rates
	END
GO
 