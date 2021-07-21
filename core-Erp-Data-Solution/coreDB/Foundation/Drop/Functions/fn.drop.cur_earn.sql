use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'cur_earn')
	BEGIN
		DROP  function cur_earn
	END
GO