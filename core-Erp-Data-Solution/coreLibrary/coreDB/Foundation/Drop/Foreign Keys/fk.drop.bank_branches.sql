use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_bank_branches')
	BEGIN
		alter table bank_branches DROP  constraint fk_bank_branches
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_bank_branch_loc')
	BEGIN
		alter table bank_branches DROP  constraint fk_bank_branch_loc
	END
GO
 