use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'jnl_batch')
	BEGIN
		DROP  Table jnl_batch
	END
GO
 