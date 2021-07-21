use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ou_cat')
	BEGIN
		DROP  Table ou_cat
	END
GO
 