use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'acc_bal_frgn')
	BEGIN
		DROP  function acc_bal_frgn
	END
GO