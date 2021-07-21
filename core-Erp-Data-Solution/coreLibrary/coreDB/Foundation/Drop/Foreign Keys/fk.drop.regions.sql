use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_country_region')
	BEGIN
		alter table regions DROP  constraint fk_country_region
	END
GO
 