use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'change_acc_parent2')
	BEGIN
		DROP  Procedure  change_acc_parent2
	END

GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'change_acc_parent')
	BEGIN
		DROP  Procedure  change_acc_parent
	END

GO
