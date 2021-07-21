use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'jnl_batch_stg')
	BEGIN
		DROP  Table jnl_batch_stg
	END
GO
 