use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_city_locations')
	BEGIN
		alter table locations DROP  constraint fk_city_locations
	END
GO
 