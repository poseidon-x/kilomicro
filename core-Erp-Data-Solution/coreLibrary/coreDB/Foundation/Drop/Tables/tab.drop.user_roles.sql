use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'user_roles')
	BEGIN
		DROP  Table user_roles
	END
GO
 