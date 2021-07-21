use coreDB
go

ALTER proc ln.getCashFlow
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
			isnull(i.[collection], 0) as collectionRatio 
		from ln.loan l
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.prLoanDetail p on l.loanID = p.loanID 
			inner join ln.loanProduct lp on lp.loanProductID = p.loanProductID
			inner join 
			(				
				select *
				from
				(
					SELECT
						loanProductID,
						[collection],
						[month],
						row_number() over (partition by loanProductID order by [month] desc) as rn
					from ln.[collection]
					where cast(''+cast(left([month], 4) as nvarchar(4))+'-'+cast(right([month], 2) as nvarchar(2))
						+'-01 00:00:00' as datetime) >= @sd
				) i
			) i on p.loanProductID = i.loanProductID
			inner join 
			(
				select 
					loanID, 
					isnull( cast(''+cast(datepart(yy,repaymentDate) as nvarchar(4))+'-'+
					cast(datepart(mm,repaymentDate) as nvarchar(2))
						+'-15 00:00:00' as datetime), getDate()) as repaymentDate,
					sum(interestPayment + principalPayment) as expected
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
			isnull(i.[collection], 0) 
	) b 
	order by 1 asc, 5 desc
end
go

alter view vwCashFlow
as
	select isnull( getdate(), getDate()) as [month],
			isnull('', '') as loanProductName,
			isnull('', '') as clientName,
			isnull('', '') as loanID,
			isnull(0.0, 0) as expected,
			isnull(0.0, 0) as collectionRatio 
go