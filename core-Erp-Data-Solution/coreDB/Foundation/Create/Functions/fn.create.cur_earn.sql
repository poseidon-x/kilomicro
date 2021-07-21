
-- =============================================
alter FUNCTION cur_earn
(
	@end_date datetime, 
	@cost_center_id int =  null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float, @rev float, @exp float 
	declare @closedDate datetime, @ed datetime
	
	select @ed=DATEADD(s, -1, DATEADD(d, 1, DATEADD(d, DATEDIFF(d, 0, @end_date), 0)))
	
	select @closedDate = max(close_date) from acct_period where close_date<=@end_date
	if(@closedDate is null)
		select @closedDate = '1900-01-01'
		  
	select @rev = sum(
			case when cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j 
		inner join accts a
		on j.acct_id =a.acct_id inner join acct_heads h
		on a.acct_head_id = h.acct_head_id inner join acct_cats c
		on h.acct_cat_id = c.acct_cat_id
	where
		(
			(cat_code in (4,7))
			and (tx_date <= @ed)
			--and tx_date> @closedDate 
			--and (tx_date >=  dbo.fin_year_start(@ed))
			and acct_period is null
			--and (@cost_center_id is null or @cost_center_id = cost_center_id) 
		)
		
	select @exp = sum(
			case when cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join accts a
		on j.acct_id =a.acct_id inner join acct_heads h
		on a.acct_head_id = h.acct_head_id inner join acct_cats c
		on h.acct_cat_id = c.acct_cat_id
	where
		(
			(cat_code in (5,6,8))
			and (tx_date <= @ed)
			--and tx_date> @closedDate 
			--and (tx_date >=  dbo.fin_year_start(@ed))
			and acct_period is null
			--and (@cost_center_id is null or @cost_center_id = cost_center_id) 
		)
	
	select @bal = isnull(@rev,0)-isnull(@exp,0)
	
	return @bal
END
GO


-- =============================================
alter FUNCTION cur_earn_closed
(
	@end_date datetime, 
	@cost_center_id int =  null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float, @rev float, @exp float
	declare @ed datetime, @year int, @month int, @day int

	select @year=year(@end_Date), @month=month(@end_Date), @day = day(@end_date)
	select @ed =  DATETIMEFROMPARTS(@year, @month, @day, 23, 59, 59, 998)

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
			and (tx_date <= @ed)
			--and (((tx_date >=  dbo.fin_year_start(@ed))))
			and acct_period is null
			and (@cost_center_id is null or @cost_center_id = cost_center_id) 
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
			and (tx_date <= @ed)
			--and (((tx_date >=  dbo.fin_year_start(@ed))))
			and acct_period is null
			and (@cost_center_id is null or @cost_center_id = cost_center_id) 
		)
	
	select @bal = isnull(@rev,0)-isnull(@exp,0)
	
	return @bal
END
GO


