use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'ret_earn')
	BEGIN
		DROP  function ret_earn
	END
GO