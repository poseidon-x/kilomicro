use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_head_level')
	BEGIN
		DROP  function acc_head_level
	END
GO