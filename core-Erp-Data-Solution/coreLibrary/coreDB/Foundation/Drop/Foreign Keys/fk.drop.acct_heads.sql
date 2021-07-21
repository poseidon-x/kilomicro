use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_acct_heads')
	BEGIN
		alter table acct_heads DROP  constraint fk_acct_heads
	END
GO
 