use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'gl_ou')
	BEGIN
		DROP  Table gl_ou
	END
GO
 