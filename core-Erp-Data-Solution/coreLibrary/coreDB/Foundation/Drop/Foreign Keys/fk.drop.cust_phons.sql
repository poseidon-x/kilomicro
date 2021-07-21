use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_cust_phone_type')
	BEGIN
		alter table cust_phons DROP  constraint fk_cust_phone_type
	END
GO
 