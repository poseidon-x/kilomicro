use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'user_gl_ou_gl_ou')
	BEGIN
		DROP  Table user_gl_ou_gl_ou
	END
GO
 