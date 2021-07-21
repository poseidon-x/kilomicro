use coreDB
go

alter procedure ln.getCollection 
(
	@month int
)
as
begin
	declare @ed datetime, @sd datetime
	select @ed=
		dateadd(minute, -1, dateadd(month,1,cast(''+cast(left(@month,4) as nvarchar(4)) + '-' + cast(right(@month,2) as nvarchar(2))
		+'-01 00:00:00' as datetime)))
	select @sd=
		cast(''+cast(left(@month,4) as nvarchar(4)) + '-' + cast(right(@month,2) as nvarchar(2))
		+'-01 23:59:59' as datetime)
	select
		isnull(isnull(c.loanProductID, b.loanProductID), 0) as loanProductID,
		isnull(isnull(c.[collection], b.[collection]), 0) as [collection]
	from ln.[collection] c 
		full outer join
		(
				
			select
				loanProductID,
				case when sum(interestPayment + principalPayment)>0 then
					sum(interestPayment + principalPayment - interestBalance - principalBalance)/
					sum(interestPayment + principalPayment) * 100.0
					else 0 end as [collection]
			from ln.loan l
				inner join ln.prLoanDetail p on l.loanID = p.loanID
				inner join ln.repaymentSchedule rs on l.loanID = rs.loanID
			where repaymentDate between @sd and @ed 
			group by loanProductID
		) b on c.loanProductID = b.loanProductID and (c.[month] is null or c.[month] = @month)
end