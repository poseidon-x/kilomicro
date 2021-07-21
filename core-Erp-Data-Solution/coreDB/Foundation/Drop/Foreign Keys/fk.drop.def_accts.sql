use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_def_accts')
	BEGIN
		alter table def_accts DROP  constraint fk_def_accts
	END
GO
 