use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'role_perms')
	BEGIN
		DROP  Table role_perms
	END
GO
 