use coreDB
go

create function ln.intBalanceAsAt
(
	@loanID int,
	@date datetime
)
returns float
with encryption as 
begin
	declare @bal float
	declare @princBal float
	declare @intBal float

	select 
		@intBal = sum((case when repaymentDate <@date then interestBalance else 0.0 end))
	from ln.repaymentSchedule r 
	where r.loanID = @loanID

	select @bal =  @intBal

	return @bal
end

go