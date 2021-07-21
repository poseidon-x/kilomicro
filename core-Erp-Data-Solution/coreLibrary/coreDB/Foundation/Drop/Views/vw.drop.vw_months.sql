use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_months')
	BEGIN
		DROP  View vw_months
	END
GO
