use coreDB
go


-- =============================================
create FUNCTION acc_credit
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
	declare @credit float, @earning float
	declare @sd datetime, @ed datetime
	select @sd=
		cast(''+cast(datepart(yyyy,@start_Date) as nvarchar(4))+'-'+cast(datepart(mm,@start_Date) as nvarchar(2))
		+'-'+cast(datepart(dd,@start_Date) as nvarchar(2)) as datetime)
	select @ed=
		cast(''+cast(datepart(yyyy,@end_date) as nvarchar(4))+'-'+cast(datepart(mm,@end_date) as nvarchar(2))
		+'-'+cast(datepart(dd,@end_date) as nvarchar(2)) + ' 23:59:59' as datetime)
	
	if exists(select acct_id from def_accts where code='CE' and @acct_id = acct_id)
	begin
		select @earning = dbo.cur_earn(@ed, @cost_center_id)
	end
	
	if exists(select acct_id from def_accts where code='RE' and @acct_id = acct_id)
	begin
		select @earning = dbo.ret_earn(@ed, @cost_center_id)
	end

	select @credit = sum(
		case when @cat_code in (1, 5, 6, 8) then dbt_amt else 	crdt_amt   end
		)
	from jnl j inner join jnl_batch b
		on j.jnl_batch_id = b.jnl_batch_id
	where
		(
			(acct_id = @acct_id) 
			and (tx_date <= @ed)
			and (tx_date >= @sd)
			and (@cost_center_id is null or @cost_center_id = cost_center_id)
		)
	
	select @credit = ISNULL(@credit, 0) 
	if @earning>0
	begin
		select @credit= @credit + @earning
	end
	return @credit
END
GO


