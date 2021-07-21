use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'get_acc_bal')
	BEGIN
		DROP  procedure get_acc_bal
	END
GO