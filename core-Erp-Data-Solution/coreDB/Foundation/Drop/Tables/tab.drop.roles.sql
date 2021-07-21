use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'roles')
	BEGIN
		DROP  Table roles
	END
GO
 