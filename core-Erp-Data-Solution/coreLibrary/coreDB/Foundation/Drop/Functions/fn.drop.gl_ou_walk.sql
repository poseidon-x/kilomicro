use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'gl_ou_walk')
	BEGIN
		DROP  function gl_ou_walk
	END
GO