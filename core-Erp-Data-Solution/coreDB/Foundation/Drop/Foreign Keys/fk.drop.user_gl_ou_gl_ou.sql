use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_user_gl_ou_gl_ou')
	BEGIN
		alter table acct_heads DROP  constraint fk_user_gl_ou_gl_ou
	END
GO
 