use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'user_login_attempts')
	BEGIN
		DROP  Table user_login_attempts
	END
GO
 