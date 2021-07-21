use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'get_op_stmt')
	BEGIN
		DROP  procedure get_op_stmt
	END
GO