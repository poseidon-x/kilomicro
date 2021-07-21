use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_gl_ou_cat')
	BEGIN
		alter table gl_ou DROP  constraint fk_gl_ou_cat
	END
GO
 