use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fin_year_start')
	BEGIN
		DROP  function fin_year_start
	END
GO