use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_district_cities')
	BEGIN
		alter table cities DROP  constraint fk_district_cities
	END
GO
 