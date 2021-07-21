use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ou')
	BEGIN
		DROP  Table ou
	END
GO
 