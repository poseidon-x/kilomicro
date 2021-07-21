use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'gl_ou_cat')
	BEGIN
		DROP  Table gl_ou_cat
	END
GO
 