use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_ytd_amt')
	BEGIN
		DROP  function acc_ytd_amt
	END
GO