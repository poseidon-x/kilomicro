use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_region_district')
	BEGIN
		alter table districts DROP  constraint fk_region_district
	END
GO
 