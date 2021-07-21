use coreDB
go

alter procedure ln.getMF7
(
	@endDate datetime,
	@clientID int)
with encryption as 
 
	declare @ed datetime, @pd datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime);
	select @pd=dateadd(month, -1, @ed);

with t as
( 
	select  
		1 as Part,
		'Previous balance(Gross)' as [description],
		isnull(case when daysDue<=0 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment,
		0  as interestPayment,
		isnull(case when daysDue between 1 and 30 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment_30,
		0  as interestPayment_30,		
		isnull(case when daysDue between 31 and 60 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment_60,
		0  as interestPayment_60,
		isnull(case when daysDue between 61 and 90 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment_90,
		0  as interestPayment_90,
		isnull(case when daysDue between 91 and 120 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment_120,
		0  as interestPayment_120,
		isnull(case when daysDue between 121 and 150 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment_150,
		0  as interestPayment_150,
		isnull(case when daysDue > 150 then sum(case when proposedAmount=0 then provisionAmount else proposedAmount end) else null end,0)  as principalPayment_151,
		0  as interestPayment_151,
		l.loanID
	from ln.loan l inner join 
		 ln.loanProvision t on t.loanID=l.loanID
	where  provisionDate = @pd
		and (@clientID is null or l.clientID=@clientID)
	group by 
		l.loanID, daysDue
	union all
	select
		2 as Part,
		'Add new advances made during half-year' as [description],
		case when daysDue<=0 then sum(t.principalBalance) else null end  as principalPayment,
		0  as interestPayment,
		case when daysDue between 1 and 30 then sum(t.principalBalance) else null end  as principalPayment_30,
		0  as interestPayment_30,		
		case when daysDue between 31 and 60 then sum(t.principalBalance) else null end  as principalPayment_60,
		0  as interestPayment_60,
		case when daysDue between 61 and 90 then sum(t.principalBalance) else null end  as principalPayment_90,
		0  as interestPayment_90,
		case when daysDue between 91 and 120 then sum(t.principalBalance) else null end  as principalPayment_120,
		0  as interestPayment_120,
		case when daysDue between 121 and 150 then sum(t.principalBalance) else null end  as principalPayment_150,
		0  as interestPayment_150,
		case when daysDue > 150 then sum(t.principalBalance) else null end  as principalPayment_151,
		0  as interestPayment_151,
		l.loanID
	from ln.loan l inner join 		
		 ln.loanProvision t on t.loanID=l.loanID
	where  provisionDate = @ed 
		and (@clientID is null or l.clientID=@clientID)
	group by 
		l.loanID, daysDue
	union all
	select
		3 as Part,
		'Add interest charged for the half year' as [description],
		0  as principalPayment,
		0  as interestPayment,
		0  as principalPayment_30,
		0  as interestPayment_30,
		0  as principalPayment_60,
		0  as interestPayment_60,
		0  as principalPayment_90,
		0  as interestPayment_90,
		0  as principalPayment_120,
		0  as interestPayment_120,
		0  as principalPayment_150,
		0  as interestPayment_150,
		0 as principalPayment_151,
		0  as interestPayment_151,
		l.loanID
	from ln.loan l
		inner join ln.repaymentSchedule rs on l.loanID=rs.loanID
	where (null is null or l.clientID = null) and repaymentDate between dateadd(month, -60, @endDate) and @endDate
	group by 
		l.loanID
	union all
	select
		4 as Part,
		'Less amount received' as [description],
		case when daysDue<=0 then sum(paid) else null end  as principalPayment,
		0  as interestPayment,
		case when daysDue between 1 and 30 then sum(paid) else null end  as principalPayment_30,
		0  as interestPayment_30,		
		case when daysDue between 31 and 60 then sum(paid) else null end  as principalPayment_60,
		0  as interestPayment_60,
		case when daysDue between 61 and 90 then sum(paid) else null end  as principalPayment_90,
		0  as interestPayment_90,
		case when daysDue between 91 and 120 then sum(paid) else null end  as principalPayment_120,
		0  as interestPayment_120,
		case when daysDue between 121 and 150 then sum(paid) else null end  as principalPayment_150,
		0  as interestPayment_150,
		case when daysDue > 150 then sum(paid) else null end  as principalPayment_151,
		0  as interestPayment_151,
		l.loanID
	from ln.loan l
		inner join 
		(
			select
				l.loanID,
				ln.getDaysDue(loanID, @endDate) as daysDue,
				isnull((select sum(principalPaid+interestPaid) from ln.loanRepayment lr where lr.loanID=l.loanID and repaymentDate between @pd and @ed), 0) as paid,
				isnull((select sum(principalPayment) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as principal,
				case when disbursementDate > dateadd(month,-6,@endDate) then 1 else 0 end as inc
			from ln.loan l  
		) q on l.loanID = q.loanID
	where inc=1  
		and (@clientID is null or l.clientID=@clientID)
		and disbursementDate <= @endDate
	group by 
		l.loanID, daysDue	
	union all
	select
		5 as Part,
		'Less amount written off' as [description],
		case when daysDue<=0 then sum(writtenOff) else null end  as principalPayment,
		0  as interestPayment,
		case when daysDue between 1 and 30 then sum(writtenOff) else null end  as principalPayment_30,
		0  as interestPayment_30,		
		case when daysDue between 31 and 60 then sum(writtenOff) else null end  as principalPayment_60,
		0  as interestPayment_60,
		case when daysDue between 61 and 90 then sum(writtenOff) else null end  as principalPayment_90,
		0  as interestPayment_90,
		case when daysDue between 91 and 120 then sum(writtenOff) else null end  as principalPayment_120,
		0  as interestPayment_120,
		case when daysDue between 121 and 150 then sum(writtenOff) else null end  as principalPayment_150,
		0  as interestPayment_150,
		case when daysDue > 150 then sum(writtenOff) else null end  as principalPayment_151,
		0  as interestPayment_151,
		l.loanID
	from ln.loan l
		inner join 
		(
			select
				l.loanID,
				ln.getDaysDue(loanID, @endDate) as daysDue,
				isnull((select sum(principalPaid+interestPaid) from ln.loanRepayment lr where lr.loanID=l.loanID  and repaymentDate <= @endDate), 0) as paid,
				isnull((select sum(principalPayment) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as principal,
				isnull((select sum(interestWritenOff) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as writtenOff,
				case when disbursementDate > dateadd(month,-6,@endDate) then 1 else 0 end as inc
			from ln.loan l  
		) q on l.loanID = q.loanID
	where inc=1  
		and (@clientID is null or l.clientID=@clientID)
		and disbursementDate <= @endDate
	group by 
		l.loanID, daysDue 
	union all	
	select
		9,
		'Less allowable security(cash and near cash instruments)' as [description],
		0  as principalPayment,
		case when daysDue<=0  then max(fairValue) else null end  as interestPayment,
		0  as principalPayment_30,
		case when daysDue between 1 and 30 then max(fairValue) else null end  as interestPayment_30,
		0  as principalPayment_60,
		case when daysDue between 31 and 60 then max(fairValue) else null end  as interestPayment_60,
		0  as principalPayment_90,
		case when daysDue between 61 and 90 then max(fairValue) else null end  as interestPayment_90,
		0  as principalPayment_120,
		case when daysDue between 91 and 120 then max(fairValue) else null end  as interestPayment_120,
		0  as principalPayment_150,
		case when daysDue between 121 and 150 then max(fairValue) else null end  as interestPayment_150,
		0 as principalPayment_151,
		case when daysDue > 150  then max(fairValue) else null end  as interestPayment_151,
		l.loanID
	from ln.loan l
		inner join ln.loanCollateral lc on l.loanID=lc.loanID		
		inner join 
		(
			select
				l.loanID,
				ln.getDaysDue(loanID, @endDate) as daysDue,
				isnull((select sum(principalPaid+interestPaid) from ln.loanRepayment lr where lr.loanID=l.loanID  and repaymentDate <= @endDate), 0) as paid,
				isnull((select sum(principalPayment) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as principal,
				isnull((select sum(interestWritenOff) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as writtenOff,
				case when disbursementDate > dateadd(month,-6,@endDate) then 1 else 0 end as inc
			from ln.loan l  
		) q on l.loanID = q.loanID
	where inc=1
		and (@clientID is null or l.clientID=@clientID)
		and disbursementDate <= @endDate
		and collateralTypeID in (4)
	group by 
		l.loanID , daysDue
)
select
	part,
	[description],
	isnull(sum(isnull(principalPayment,0)+isnull(interestPayment,0)),0) as amount,
	isnull(sum(isnull(principalPayment_30,0)+isnull(interestPayment_30,0)),0) as amount_30,
	isnull(sum(isnull(principalPayment_60,0)+isnull(interestPayment_60,0)),0) as amount_60,
	isnull(sum(isnull(principalPayment_90,0)+isnull(interestPayment_90,0)),0) as amount_90,
	isnull(sum(isnull(principalPayment_120,0)+isnull(interestPayment_120,0)),0) as amount_120,
	isnull(sum(isnull(principalPayment_150,0)+isnull(interestPayment_150,0)),0) as amount_150,
	isnull(sum(isnull(principalPayment_151,0)+isnull(interestPayment_151,0)),0) as amount_151
from t
where part <> 9
group by part, [description]
union all
select
	6,
	'+/- changes in classification from previous half-year' as [description],
	0 as amount,
	0 as amount_30,
	0 as amount_60,
	0 as amount_90,
	0 as amount_120,
	0 as amount_150,
	0 as amount_151
union all
select
	7 as part,
	'Current balance (Gross)' as [description],
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment,0)+isnull(interestPayment,0) 
		when part between 4 and 6 then -isnull(principalPayment,0)-isnull(interestPayment,0) end ),0) as amount, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_30,0)+isnull(interestPayment_30,0) 
		when part between 4 and 6 then -isnull(principalPayment_30,0)-isnull(interestPayment_30,0) end ),0) as amount_30, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_60,0)+isnull(interestPayment_60,0) 
		when part between 4 and 6 then -isnull(principalPayment_60,0)-isnull(interestPayment_60,0) end ),0) as amount_60, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_90,0)+isnull(interestPayment_90,0) 
		when part between 4 and 6 then -isnull(principalPayment_90,0)-isnull(interestPayment_90,0) end ),0) as amount_90,
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_120,0)+isnull(interestPayment_120,0) 
		when part between 4 and 6 then -isnull(principalPayment_120,0)-isnull(interestPayment_120,0) end ),0) as amount_120,  
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_150,0)+isnull(interestPayment_150,0) 
		when part between 4 and 6 then -isnull(principalPayment_150,0)-isnull(interestPayment_150,0) end ),0) as amount_150, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_151,0)+isnull(interestPayment_151,0) 
		when part between 4 and 6 then -isnull(principalPayment_151,0)-isnull(interestPayment_151,0) end ),0) as amount_151
from t
union all
select
	9,
	'Less allowable security(cash and near cash instruments)' as [description],	
	isnull(sum(interestPayment),0)  as amount, 
	isnull(sum(interestPayment_30),0)  as amount_30, 
	isnull(sum(interestPayment_60),0)  as amount_60, 
	isnull(sum(interestPayment_90),0)  as amount_90, 
	isnull(sum(interestPayment_120),0)  as amount_120, 
	isnull(sum(interestPayment_150),0)  as amount_150,
	isnull(sum(interestPayment_151),0)  as amount_151
from t
	where (part=9) and loanID in (select loanID from t where  part<>9)
union all
select
	10 as part,
	'Net current balance' as [description],
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment,0)+isnull(interestPayment,0) 
		when part between 4 and 9 then -isnull(principalPayment,0)-isnull(interestPayment,0) end ),0) as amount, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_30,0)+isnull(interestPayment_30,0) 
		when part between 4 and 9 then -isnull(principalPayment_30,0)-isnull(interestPayment_30,0) end ),0) as amount_30, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_60,0)+isnull(interestPayment_60,0) 
		when part between 4 and 9 then -isnull(principalPayment_60,0)-isnull(interestPayment_60,0) end ),0) as amount_60, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_90,0)+isnull(interestPayment_90,0) 
		when part between 4 and 9 then -isnull(principalPayment_90,0)-isnull(interestPayment_90,0) end ),0) as amount_90,
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_120,0)+isnull(interestPayment_120,0) 
		when part between 4 and 9 then -isnull(principalPayment_120,0)-isnull(interestPayment_120,0) end ),0) as amount_120,  
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_150,0)+isnull(interestPayment_150,0) 
		when part between 4 and 9 then -isnull(principalPayment_150,0)-isnull(interestPayment_150,0) end ),0) as amount_150, 
	isnull(sum(case when part between 1 and 3 then isnull(principalPayment_151,0)+isnull(interestPayment_151,0) 
		when part between 4 and 9 then -isnull(principalPayment_151,0)-isnull(interestPayment_151,0) end ),0) as amount_151
from t
union all
select
	11,
	'Priovision required' as [description],
	1 as amount,
	5 as amount_30,
	20 as amount_60,
	40 as amount_90,
	60 as amount_120,
	80 as amount_150,
	100 as amount_151
union all
select
	12 as part,
	'Amount provided(per MF2A)' as [description],
	isnull(sum(case when daysDue<=0 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount,
	isnull(sum(case when daysDue between 1 and 30 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount_30,
	isnull(sum(case when daysDue between 31 and 60 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount_60,
	isnull(sum(case when daysDue between 61 and 90 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount_90,
	isnull(sum(case when daysDue between 91 and 120 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount_120,
	isnull(sum(case when daysDue between 121 and 150 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount_150,
	isnull(sum(case when daysDue > 150 then case when proposedAmount=0 then provisionAmount else proposedAmount end else null end),0)  as amount_151		
from ln.loanProvision lp inner join ln.loan l on lp.loaniD=l.loanID
where provisionDate=@ed 
	and (@clientID is null or l.clientID=@clientID)
order by part asc