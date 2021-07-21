use coreDB
go


-- =============================================
CREATE FUNCTION acc_bal_frgn
(
	@acct_id int,
	@cat_code tinyint,
	@start_date datetime,
	@end_date datetime
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float
	declare @closedBal float 
	declare @fin_year_start datetime
	
	select @fin_year_start = dbo.fin_year_start(@end_date)
	
	select @closedBal = SUM(frgn_bal)
	from acct_bals
	where acct_id = @acct_id

	select @bal = sum(
			case when @cat_code in (1,5,6,8) then frgn_dbt_amt-frgn_crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join jnl_batch b
		on j.jnl_batch_id = b.jnl_batch_id
	where
		(
			(acct_id = @acct_id)
			and 
			(
			 (@cat_code <4 )
			 or
			 (acct_period is null or tx_date >= @fin_year_start)
			)
			and (tx_date <= @end_date)
			and (tx_date >= @start_date)
		)
		
	select @bal = ISNULL(@bal, 0) + ISNULL(@closedBal, 0)
	return @bal
END
GO


