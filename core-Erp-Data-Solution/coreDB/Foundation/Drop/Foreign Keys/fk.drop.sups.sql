use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_sups_type')
	BEGIN
		alter table sups DROP  constraint fk_sups_type
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_sups_currency')
	BEGIN
		alter table sups DROP  constraint fk_sups_currency
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_sups_ap_acct')
	BEGIN
		alter table sups DROP  constraint fk_sups_ap_acct
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_sups_vat_acct')
	BEGIN
		alter table sups DROP  constraint fk_sups_vat_acct
	END
GO
 