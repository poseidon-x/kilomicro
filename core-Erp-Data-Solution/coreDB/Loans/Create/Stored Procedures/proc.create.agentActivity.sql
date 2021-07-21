use coreDB
go

alter proc ln.getAgentActivity
(
	@startDate datetime,
	@endDate datetime
)
as
begin
	declare @ed datetime, @sd datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4)) + '-' + cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
	select @sd=
		cast(''+cast(datepart(yyyy,@startDate) as nvarchar(4))+'-'+cast(datepart(mm,@startDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@startDate) as nvarchar(2)) + ' 00:00:00' as datetime)

	select
		isnull(a.surName + ', ' + a.otherNames, '') as agentName,
		isnull(a.agentNo, '') as agentID,
		isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end, '') as clientName,
		isnull(l.loanNo, '') as loanID,
		isnull(l.amountDisbursed  - l.processingFee, 0) as amountDisbursed,
		isnull(i.incentiveAmount, 0) as incentiveAmount,
		isnull(i.commissionAmount-withholdingAmount ,0) as commissionAmount,
		isnull(case when i.paid=1 and i.commPaid=1 then i.incentiveAmount+i.commissionAmount-withholdingAmount
				when paid=1 then i.incentiveAmount
				when commPaid  = 1 then i.commissionAmount-withholdingAmount
				else 0 end, 0) as totalPaid
	from ln.loan l
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.prLoanDetail p on l.loanID = p.loanID
		inner join ln.agent a on a.agentID = l.agentID
		inner join ln.loanIncentive i on l.loanID = i.loanID
	where l.disbursementDate between @sd and @ed
end
go