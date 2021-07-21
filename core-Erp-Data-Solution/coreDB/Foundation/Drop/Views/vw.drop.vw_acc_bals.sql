use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'vw_acc_bals')
	BEGIN
		DROP  View vw_acc_bals
	END
GO
