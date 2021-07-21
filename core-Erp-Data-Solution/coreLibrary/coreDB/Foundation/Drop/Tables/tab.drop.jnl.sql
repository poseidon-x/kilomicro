use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'jnl')
	BEGIN
		DROP  Table jnl
	END
GO
 