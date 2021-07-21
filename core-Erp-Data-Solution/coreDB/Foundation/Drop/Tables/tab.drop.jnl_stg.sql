use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'jnl_stg')
	BEGIN
		DROP  Table jnl_stg
	END
GO
 