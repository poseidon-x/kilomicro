use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_custs_type')
	BEGIN
		alter table custs DROP  constraint fk_custs_type
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_custs_currency')
	BEGIN
		alter table custs DROP  constraint fk_custs_currency
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_custs_ar_acct')
	BEGIN
		alter table custs DROP  constraint fk_custs_ar_acct
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_custs_vat_acct')
	BEGIN
		alter table custs DROP  constraint fk_custs_vat_acct
	END
GO
 