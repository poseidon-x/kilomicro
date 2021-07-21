use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_batch')
	BEGIN
		alter table jnl DROP  constraint fk_jnl_batch
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_acct')
	BEGIN
		alter table jnl DROP  constraint fk_jnl_acct
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_currency')
	BEGIN
		alter table jnl DROP  constraint fk_jnl_currency
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_cost_center')
	BEGIN
		alter table jnl DROP  constraint fk_jnl_cost_center
	END
GO
  