use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_amt')
	BEGIN
		DROP  function acc_amt
	END
GO