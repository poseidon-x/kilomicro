use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'jnl_batch_tmp')
	BEGIN
		DROP  Table jnl_batch_tmp
	END
GO
 