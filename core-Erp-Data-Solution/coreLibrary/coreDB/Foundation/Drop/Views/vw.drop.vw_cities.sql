use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_cities')
	BEGIN
		DROP  View vw_cities
	END
GO
