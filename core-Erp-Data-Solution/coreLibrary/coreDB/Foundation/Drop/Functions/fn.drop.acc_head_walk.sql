use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_head_walk')
	BEGIN
		DROP  function acc_head_walk
	END
GO