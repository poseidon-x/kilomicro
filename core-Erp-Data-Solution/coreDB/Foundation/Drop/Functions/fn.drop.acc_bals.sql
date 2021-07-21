use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_bals')
	BEGIN
		DROP  function acc_bals
	END
GO