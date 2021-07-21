use coreDB
go


-- =============================================
alter FUNCTION acc_amt
(
	@acct_id int,
	@cat_code tinyint,
	@end_date datetime,
	@m int,
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float
	declare @m_date datetime
	declare @ed datetime, @year int, @month int, @day int

	select @year=year(@end_Date), @month=month(@end_Date), @day = day(@end_date)
	select @ed =  DATETIMEFROMPARTS(@year, @month, @day, 23, 59, 59, 998)

	select @m_date = dateadd(mm,@m-1, @ed)
	
	select @bal = sum(
			case when @cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join jnl_batch b
		on j.jnl_batch_id = b.jnl_batch_id
	where
		(
			(acct_id = @acct_id)
			and (year(tx_date) = year(@m_date))
			and (month(tx_date) = month(@m_date))
			and (@cost_center_id is null or @cost_center_id = cost_center_id)
		)
		
	return isnull(@bal,0)
END
GO


