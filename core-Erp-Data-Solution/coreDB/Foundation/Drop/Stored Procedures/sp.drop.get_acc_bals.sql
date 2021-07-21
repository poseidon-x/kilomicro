use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'get_acc_bals')
	BEGIN
		DROP  procedure get_acc_bals
	END
GO