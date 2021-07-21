use coreDB
go

alter proc ln.getCollectionRatio
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

	select *
	from
	(
		select 
			isnull(repaymentDate, getDate()) as [month],
			isnull(loanProductName, '') as loanProductName,
			isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end, '') as clientName,
			isnull(l.loanNo, '') as loanID,
			isnull(expected, 0) as expected,
			isnull([collection], 0) as collectionRatio, 
			isnull(collected, 0) as collected
		from ln.loan l
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.prLoanDetail p on l.loanID = p.loanID 
			inner join ln.loanProduct lp on lp.loanProductID = p.loanProductID 
			inner join 
			(
				select 
					loanID, 
					isnull( cast(''+cast(datepart(yy,repaymentDate) as nvarchar(4))+'-'+
					cast(datepart(mm,repaymentDate) as nvarchar(2))
						+'-15 00:00:00' as datetime), getDate()) as repaymentDate,
					sum(interestPayment + principalPayment) as expected,
					sum(interestPayment + principalPayment - principalBalance - interestBalance)/
					sum(interestPayment + principalPayment) *100.0 as [collection],
					sum(interestPayment + principalPayment - principalBalance - interestBalance) as collected
				from ln.repaymentSchedule rs
				where rs.repaymentDate between @sd and @ed
				group by loanID,
					isnull( cast(''+cast(datepart(yy,repaymentDate) as nvarchar(4))+'-'+
					cast(datepart(mm,repaymentDate) as nvarchar(2))
					+'-15 00:00:00' as datetime), getDate())
			) rs on  rs.loanID = l.loanID  
		group by 
			isnull(repaymentDate, getDate()),
			isnull(loanProductName, '')  ,
			isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end, '')  ,
			isnull(l.loanNo, '')  , 
			rs.expected,
			isnull([collection], 0) , 
			isnull(collected, 0)
	) b 
	order by 1 asc, 5 desc
end
go