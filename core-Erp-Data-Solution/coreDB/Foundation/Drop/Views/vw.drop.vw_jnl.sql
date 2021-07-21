use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_jnl')
	BEGIN
		DROP  View vw_jnl
	END
GO
