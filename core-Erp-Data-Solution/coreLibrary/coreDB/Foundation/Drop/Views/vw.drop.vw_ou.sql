use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_ou')
	BEGIN
		DROP  View vw_ou
	END
GO
