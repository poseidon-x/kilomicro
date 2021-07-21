use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_tmp_batch')
	BEGIN
		alter table jnl_tmp DROP  constraint fk_jnl_tmp_batch
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_tmp_acct')
	BEGIN
		alter table jnl_tmp DROP  constraint fk_jnl_tmp_acct
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_tmp_currency')
	BEGIN
		alter table jnl_tmp DROP  constraint fk_jnl_tmp_currency
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_jnl_tmp_cost_center')
	BEGIN
		alter table jnl_tmp DROP  constraint fk_jnl_tmp_cost_center
	END
GO
  