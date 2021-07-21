use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'ou_walk')
	BEGIN
		DROP  function ou_walk
	END
GO