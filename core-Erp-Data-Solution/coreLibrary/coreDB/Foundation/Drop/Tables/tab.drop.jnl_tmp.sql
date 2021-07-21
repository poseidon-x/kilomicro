use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'jnl_tmp')
	BEGIN
		DROP  Table jnl_tmp
	END
GO
 