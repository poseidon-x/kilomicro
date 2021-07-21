 
-- =============================================
alter FUNCTION ret_earn
(
	@end_date datetime,
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float, @rev float, @exp float, @closedBal float
	declare @ed datetime, @year int, @month int, @day int
	declare @fin_year_start datetime, @endOfLastYear as datetime
	declare @closedDate datetime

	select @year=year(@end_Date), @month=month(@end_Date), @day = day(@end_date)
	select @ed =  DATETIMEFROMPARTS(@year, @month, @day, 0, 0, 0, 0)
	 
	select @fin_year_start = dbo.fin_year_start(@ed)
	select @endOfLastYear = dateadd(ss, -1, @fin_year_start)
	select @fin_year_start = dbo.fin_year_start(@endOfLastYear)
	
	select @closedDate = max(close_date) from acct_period where close_date<=@end_date
	if(@closedDate is null)
		select @closedDate = '1900-01-01'
		  
	select @closedBal = SUM(loc_bal)
	from acct_bals ac inner join acct_period ap on ac.acct_period = ap.acct_period
	where acct_id IN 
	(
		SELECT acct_id from def_accts where code='RE' or code = 'CE'
	)
	 and close_date=@closedDate

	select @bal = sum(
			case when cat_code in (1,5,6,8) then dbt_amt-crdt_amt
				else crdt_amt - dbt_amt end
		)
	from jnl j inner join vw_accounts a on j.acct_id=a.acct_id
	where
		(
			j.acct_id IN 
				(
					SELECT acct_id from def_accts where code='RE' or code = 'CE'
				)  
			and (tx_date <= @ed) 
			and (@cost_center_id is null or @cost_center_id = cost_center_id)
		)
	
	select @bal = isnull(@bal,0) + isnull(@closedBal, 0)
	
	return @bal
END
GO

 
-- =============================================
ALTER FUNCTION ret_earn_closed
(
	@end_date datetime,
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float, @rev float, @exp float, @closedBal float
	declare @ed datetime, @year int, @month int, @day int

	select @year=year(@end_Date), @month=month(@end_Date), @day = day(@end_date)
	select @ed =  DATETIMEFROMPARTS(@year, @month, @day, 23, 59, 59, 998)

	select @closedBal =loc_bal
	FROM dbo.acct_bals ab inner join acct_period ap on ab.acct_period = ap.acct_period
	where acct_id in (select acct_id from dbo.def_accts where code in ('RE'))
	
	select @bal = isnull(@closedBal,0) + isnull(@rev,0)-isnull(@exp,0)
	
	return @bal
END
GO


