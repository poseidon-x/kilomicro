use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'users')
	BEGIN
		DROP  Table users
	END
GO
 