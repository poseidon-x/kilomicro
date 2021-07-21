use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_bal')
	BEGIN
		DROP  function acc_bal
	END
GO