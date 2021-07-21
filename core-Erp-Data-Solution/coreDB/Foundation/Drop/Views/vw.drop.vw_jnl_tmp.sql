use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_jnl_tmp')
	BEGIN
		DROP  View vw_jnl_tmp
	END
GO
