use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_op_stmt')
	BEGIN
		DROP  View vw_op_stmt
	END
GO
