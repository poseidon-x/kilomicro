use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'get_bal_sht')
	BEGIN
		DROP  procedure get_bal_sht
	END
GO