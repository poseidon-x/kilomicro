use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_ou_cat')
	BEGIN
		alter table ou DROP  constraint fk_ou_cat
	END
GO
 