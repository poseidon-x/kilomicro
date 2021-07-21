use coreDB
go

create function ln.loanBalanceAsAt
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
	declare @pmsts float

	select 
		@pmsts = SUM(amountPaid)
	from ln.loanRepayment
	where loanID = @loanID and repaymentDate<= @date
		and repaymentTypeID in (1,2,3, 7)

	select
		@princBal=sum(PrincipalPayment),
		@intBal = sum((case when repaymentDate <@date then interestBalance else 0.0 end))
	from ln.repaymentSchedule r 
	where r.loanID = @loanID

	select @bal = isnull(@princBal, 0) + isnull(@intBal, 0) - isnull(@pmsts, 0)

	return @bal
end

go