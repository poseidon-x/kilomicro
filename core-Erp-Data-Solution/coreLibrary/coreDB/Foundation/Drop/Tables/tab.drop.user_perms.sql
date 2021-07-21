use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'user_perms')
	BEGIN
		DROP  Table user_perms
	END
GO
 