use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_cust_addr_type')
	BEGIN
		alter table cust_addr DROP  constraint fk_cust_addr_type
	END
GO
 