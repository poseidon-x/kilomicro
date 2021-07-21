use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_sup_addr_type')
	BEGIN
		alter table sup_addr DROP  constraint fk_sup_addr_type
	END
GO
 