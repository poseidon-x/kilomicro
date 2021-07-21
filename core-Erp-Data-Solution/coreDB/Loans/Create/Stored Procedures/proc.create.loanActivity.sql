use coreDB
go

alter procedure ln.getLoanActivity
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
		isnull(a.totalLoanAmount, 0) as totalLoanAmount,
		isnull(a.totalAmountDisbursed,0) as totalAmountDisbursed,
		isnull(a.totalNoOfLoans, 0) as totalNoOfLoans,
		isnull(a.totalProcessingFee, 0) as totalProcessingFee,
		isnull(b.modeOfEntryName, '') as modeOfEntryName,
		isnull((b.totalLoanAmount/a.totalLoanAmount*100.0), 0) as amountPercent,
		isnull((b.totalNoOfLoans/a.totalNoOfLoans*100.0), 0) as numberPercent,
		isnull(b.totalLoanAmount, 0) as totalLoanAmount2,
		isnull(b.totalNoOfLoans, 0) as totalNoOfLoans2
	from (
		select
			sum(l.amountDisbursed) as totalLoanAmount,
			sum(l.amountDisbursed-l.processingFee) as totalAmountDisbursed,
			cast(count(distinct l.loanID) as float) as totalNoOfLoans,
			sum(l.processingFee) as totalProcessingFee
		from ln.loan l
			inner join ln.client c on l.clientID = c.clientID
			inner join 
			(
				select loanID, max(modeOfEntryID) as modeOfEntryID , max(loanProductID) as loanProductID
				from ln.prLoanDetail
				group by loanID
			) p on l.loanID = p.loanID 
		where l.disbursementDate between @sd and @ed
	) a cross join
	(
		select
			m.modeOfEntryName,
			sum(l.amountDisbursed) as totalLoanAmount,
			count(distinct l.loanID) as totalNoOfLoans
		from ln.loan l
			inner join ln.client c on l.clientID = c.clientID
			inner join 
			(
				select loanID, max(modeOfEntryID) as modeOfEntryID , max(loanProductID) as loanProductID
				from ln.prLoanDetail
				group by loanID
			) p on l.loanID = p.loanID
			inner join ln.modeOfEntry m on p.modeOfEntryID = m.modeOfEntryID
		where l.disbursementDate between @sd and @ed
		group by m.modeOfEntryName
	) b
	order by 7 desc
end
go

alter procedure ln.getLoanActivity2
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
		+'-'+cast(datepart(dd,@startDate) as nvarchar(2)) + ' 23:59:59' as datetime)

	select 
		isnull(a.totalLoanAmount, 0) as totalLoanAmount,
		isnull(a.totalAmountDisbursed,0) as totalAmountDisbursed,
		isnull(a.totalNoOfLoans, 0) as totalNoOfLoans,
		isnull(a.totalProcessingFee, 0) as totalProcessingFee,
		isnull(b.loanProductName, '') as loanProductName,
		isnull((b.totalLoanAmount/a.totalLoanAmount*100.0), 0) as amountPercent,
		isnull((b.totalNoOfLoans/a.totalNoOfLoans*100.0), 0) as numberPercent,
		isnull(b.totalLoanAmount, 0) as totalLoanAmount2,
		isnull(b.totalNoOfLoans, 0) as totalNoOfLoans2
	from (
		select
			sum(l.amountDisbursed) as totalLoanAmount,
			sum(l.amountDisbursed-l.processingFee) as totalAmountDisbursed,
			cast(count(distinct l.loanID) as float) as totalNoOfLoans,
			sum(l.processingFee) as totalProcessingFee
		from ln.loan l
			inner join ln.client c on l.clientID = c.clientID
			inner join 
			(
				select loanID, max(modeOfEntryID) as modeOfEntryID , max(loanProductID) as loanProductID
				from ln.prLoanDetail
				group by loanID
			) p on l.loanID = p.loanID 
		where l.disbursementDate between @sd and @ed
	) a cross join
	(
		select
			m.loanProductName,
			sum(l.amountDisbursed) as totalLoanAmount,
			count(distinct l.loanID) as totalNoOfLoans
		from ln.loan l
			inner join ln.client c on l.clientID = c.clientID
			inner join 
			(
				select loanID, max(modeOfEntryID) as modeOfEntryID , max(loanProductID) as loanProductID
				from ln.prLoanDetail
				group by loanID
			) p on l.loanID = p.loanID
			inner join ln.loanProduct m on p.loanProductID = m.loanProductID
		where l.disbursementDate between @sd and @ed 
		group by m.loanProductName
	) b
	order by 7 desc
end
go

alter procedure ln.getLoanActivity3
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
		isnull(disbursementDate, getDate()) as disbursementDate,
		isnull(loanProductName, '') as loanProductName,
		isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end, '') as clientName,
		isnull(l.loanNo, '') as loanID,
		isnull(c.accountNumber, '') as accountNumber,
		isnull(amountDisbursed, 0) as loanAmount ,
		isnull(processingFee, 0) as processingFee,
		isnull(l.loanTenure, 0) as loanTenure 
	from ln.loan l
		inner join ln.client c on l.clientID = c.clientID
		inner join 
		(
				select loanID, max(modeOfEntryID) as modeOfEntryID , max(loanProductID) as loanProductID
				from ln.prLoanDetail
				group by loanID
		) p on l.loanID = p.loanID 
		inner join ln.loanProduct lp on lp.loanProductID = p.loanProductID
	where disbursementDate between @sd and @ed
end
go