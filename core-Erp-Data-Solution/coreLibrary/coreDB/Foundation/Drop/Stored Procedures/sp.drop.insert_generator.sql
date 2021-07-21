use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'gen_insert')
	BEGIN
		DROP  Procedure  gen_insert
	END

GO
