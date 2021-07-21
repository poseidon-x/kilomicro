use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'gl_ou_level')
	BEGIN
		DROP  function gl_ou_level
	END
GO