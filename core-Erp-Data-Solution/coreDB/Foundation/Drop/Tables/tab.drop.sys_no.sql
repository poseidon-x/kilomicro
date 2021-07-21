use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'sys_no')
	BEGIN
		DROP  Table sys_no
	END
GO
 