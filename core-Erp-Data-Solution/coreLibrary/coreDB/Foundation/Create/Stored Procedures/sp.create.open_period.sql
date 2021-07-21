use coreDB
go

CREATE Procedure open_period
	( 
		@accPeriod int,
		@closeDate datetime
	)
	with encryption 
as
begin
	delete 
	acct_bals
	where acct_period >= @accPeriod

	delete acct_period
	where acct_period >= @accPeriod

	update jnl set 
		acct_period = null
	where acct_period >= @accPeriod
		and acct_period is not null
	
end

GO
