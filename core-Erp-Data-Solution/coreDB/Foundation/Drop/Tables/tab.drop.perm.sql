use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'perms')
	BEGIN
		DROP  Table perms
	END
GO
 