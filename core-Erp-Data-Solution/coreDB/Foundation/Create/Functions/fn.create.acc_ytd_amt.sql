use coreDB
go


-- =============================================
alter FUNCTION acc_ytd_amt
(
	@acct_id int,
	@cat_code tinyint,
	@end_date datetime, 
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float
	declare @f_date datetime
	 declare @ed datetime, @year int, @month int, @day int

	select @year=year(@end_Date), @month=month(@end_Date), @day = day(@end_date)
	select @ed =  DATETIMEFROMPARTS(@year, @month, @day, 23, 59, 59, 998)

	select @f_date = dbo.fin_year_start(@ed)
	
	select @bal = sum(
			case when @cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join jnl_batch b
		on j.jnl_batch_id = b.jnl_batch_id
	where
		(
			(acct_id = @acct_id)
			and (tx_date between @f_date and @ed)
			and (@cost_center_id is null or @cost_center_id = cost_center_id)
		)
		
	return isnull(@bal,0)
END
GO


