use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_country_currency')
	BEGIN
		alter table countries DROP  constraint fk_country_currency
	END
GO
 