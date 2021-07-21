use coreDB
go


-- =============================================
create FUNCTION gl.bud_bal
(
	@acct_id int, 
	@end_date datetime,
	@cost_center_id int = null
)
RETURNS float
with encryption
AS
BEGIN
	declare @bal float 
	declare @ed datetime

	select @ed=
		dateadd(second, -1, dateadd( month, 1, cast(''+cast(datepart(yyyy,@end_date) as nvarchar(4))+'-'+cast(datepart(mm,@end_date) as nvarchar(2))
		+'-01' as datetime)))
	  
	select @bal = SUM(budgetAmount)
	from gl.budget
	where acct_id = @acct_id 
		and monthEndDate = @ed
		and (@cost_center_id = cost_center_id or (@cost_center_id is null and cost_center_id is null))
 
	select @bal = ISNULL(@bal, 0)
	return @bal
END
GO


