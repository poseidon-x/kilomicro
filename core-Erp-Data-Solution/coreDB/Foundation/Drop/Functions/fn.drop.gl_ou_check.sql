use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'gl_ou_check')
	BEGIN
		DROP  function gl_ou_check
	END
GO