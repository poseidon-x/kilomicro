use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE  name = 'ou_level')
	BEGIN
		DROP  function ou_level
	END
GO