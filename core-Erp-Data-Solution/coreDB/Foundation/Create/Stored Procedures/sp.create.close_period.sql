use coreDB
go

CREATE Procedure close_period
	( 
		@accPeriod int,
		@closeDate datetime
	)
	with encryption
AS
begin
	update jnl set 
		acct_period = @accPeriod
	where tx_date <= @closeDate
		and acct_period is null
	
end

GO
