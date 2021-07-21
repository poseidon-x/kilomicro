use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 't_acc_bals')
	BEGIN
		DROP  Table t_acc_bals
	END
GO
 