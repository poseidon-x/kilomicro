use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_bank_accts')
	BEGIN
		alter table bank_accts DROP  constraint fk_bank_accts
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_bank_branch_loc')
	BEGIN
		alter table bank_accts DROP  constraint fk_bank_branch_loc
	END
GO
 