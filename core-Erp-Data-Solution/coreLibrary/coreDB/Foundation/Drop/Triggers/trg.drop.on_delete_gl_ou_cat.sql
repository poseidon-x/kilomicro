use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'on_delete_gl_ou_cat')
	BEGIN
		DROP  Trigger on_delete_gl_ou_cat
	END
GO
 