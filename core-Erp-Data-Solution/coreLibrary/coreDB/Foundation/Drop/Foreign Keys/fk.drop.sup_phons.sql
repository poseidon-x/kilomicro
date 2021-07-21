use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_sup_phone_type')
	BEGIN
		alter table sup_phons DROP  constraint fk_sup_phone_type
	END
GO
 