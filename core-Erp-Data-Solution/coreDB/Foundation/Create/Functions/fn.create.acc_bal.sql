use coreDB
go


-- =============================================
alter FUNCTION acc_bal
(
	@acct_id int,
	@cat_code tinyint,
	@start_date datetime,
	@end_date datetime,
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float
	declare @closedBal float  
	declare @closedDate datetime
	declare @sd datetime, @ed datetime
	select @sd=
		cast(''+cast(datepart(yyyy,@start_Date) as nvarchar(4))+'-'+cast(datepart(mm,@start_Date) as nvarchar(2))
		+'-'+cast(datepart(dd,@start_Date) as nvarchar(2)) as datetime)
	select @ed=
		cast(''+cast(datepart(yyyy,@end_date) as nvarchar(4))+'-'+cast(datepart(mm,@end_date) as nvarchar(2))
		+'-'+cast(datepart(dd,@end_date) as nvarchar(2)) + ' 23:59:59' as datetime)
	
	select @closedDate = max(close_date) from acct_period where close_date<=@ed
	if(@closedDate is null)
		select @closedDate = '1900-01-01'
	 
	select @closedBal = SUM(loc_bal)
	from acct_bals ac inner join acct_period ap on ac.acct_period = ap.acct_period
	where acct_id = @acct_id and close_date=@closedDate and (@cat_code <4 )

	select @bal = sum(
			case when @cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j 
	where
		(
			(acct_id = @acct_id) 
			and
			(
			 (@cat_code <4 and tx_date>@closedDate )
			 or
			 (
				(@cat_code >3) and (acct_period is null)
			 )
			)
			and (tx_date <= @ed) 
			and (@cost_center_id is null or @cost_center_id = cost_center_id)
		)
	
	select @bal = ISNULL(@bal, 0) + ISNULL(@closedBal, 0)
	return @bal
END
GO


alter FUNCTION acc_bal_closed
(
	@acct_id int,
	@cat_code tinyint,
	@start_date datetime,
	@end_date datetime,
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float
	declare @closedBal float 
	declare @fin_year_start datetime
	declare @sd datetime, @ed datetime
	select @sd=
		cast(''+cast(datepart(yyyy,@start_Date) as nvarchar(4))+'-'+cast(datepart(mm,@start_Date) as nvarchar(2))
		+'-'+cast(datepart(dd,@start_Date) as nvarchar(2)) as datetime)
	select @ed=
		cast(''+cast(datepart(yyyy,@end_date) as nvarchar(4))+'-'+cast(datepart(mm,@end_date) as nvarchar(2))
		+'-'+cast(datepart(dd,@end_date) as nvarchar(2)) + ' 23:59:59' as datetime)
	 
	select @fin_year_start = dbo.fin_year_start(@ed)
	
	select @closedBal = SUM(loc_bal)
	from acct_bals
	where acct_id = @acct_id 

	select @bal = sum(
			case when @cat_code in (1,5,6,8) then dbt_amt-crdt_amt
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
			 (tx_date >= @fin_year_start)
			)
			and (tx_date <= @ed) 
			and (@cost_center_id is null or @cost_center_id = cost_center_id)
		)
	
	select @bal = ISNULL(@bal, 0) + ISNULL(@closedBal, 0)
	return @bal
END
GO


