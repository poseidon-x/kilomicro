use coreDB
go


-- =============================================
CREATE FUNCTION cur_earn
(
	@end_date datetime
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float, @rev float, @exp float
	
	select @rev = sum(
			case when cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join jnl_batch b
		on j.jnl_batch_id = b.jnl_batch_id inner join accts a
		on j.acct_id =a.acct_id inner join acct_heads h
		on a.acct_head_id = h.acct_head_id inner join acct_cats c
		on h.acct_cat_id = c.acct_cat_id
	where
		(
			(cat_code in (4,7))
			and (tx_date <= @end_date)
			and ((tx_date >=  dbo.fin_year_start(@end_date)))
		)
		
	select @exp = sum(
			case when cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join jnl_batch b
		on j.jnl_batch_id = b.jnl_batch_id inner join accts a
		on j.acct_id =a.acct_id inner join acct_heads h
		on a.acct_head_id = h.acct_head_id inner join acct_cats c
		on h.acct_cat_id = c.acct_cat_id
	where
		(
			(cat_code in (5,6,8))
			and (tx_date <= @end_date)
			and ((tx_date >=  dbo.fin_year_start(@end_date)))
		)
	
	select @bal = isnull(@rev,0)-isnull(@exp,0)
	
	return @bal
END
GO


