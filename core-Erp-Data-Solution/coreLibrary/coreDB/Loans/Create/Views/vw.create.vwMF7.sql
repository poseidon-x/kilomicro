use coreDB
go

alter view ln.vwMF7
with encryption as
with tbl as
(
	select
		0 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  <=0
	union all
	select
		30 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  between 1 and 30
	union all
	select
		60 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  between 31 and 60
	union all
	select
		90 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  between 61 and 90
	union all
	select
		120 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  between 91 and 120
	union all
	select
		150 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  between 121 and 150
	union all
	select
		151 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, getdate())),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as NewAdvances,
		datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, getdate())),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate())  > 151
)
select
	'Previous balance(Gross)' as [Description],
	isnull(sum(case when part=0 then Balance end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance end),0) as beyond_150
from tbl
union all
select
	'Add new advances made during half-year' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances end),0) as beyond_150
from tbl
union all
select
	'Add interest charged for the half year' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged end),0) as beyond_150
from tbl
union all
select
	'Less amount received' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received end),0) as beyond_150
from tbl
union all
select
	'Less amount written off' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as beyond_150
from tbl
union all
select
	'(+/-) changes in classification from previous half-year' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received-changesInClass end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as beyond_150
from tbl
union all
select
	'Current balance (Gross)' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received-changesInClass end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as beyond_150
from tbl
go


alter proc ln.spMF7 
(
	@startDate datetime
) as
with tbl as
(
	select
		0 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate))  <=0
	union all
	select
		30 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where 
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate))   between 1 and 30
	union all
	select
		60 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where 
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate))  between 31 and 60
	union all
	select
		90 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where 
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate))  between 61 and 90
	union all
	select
		120 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where 
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate))   between 91 and 120
	union all
	select
		150 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where 
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate))  between 121 and 150
	union all
	select
		151 as part,
		l.loanID,
		isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)) +
		(select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate < dateadd(MONTH, -6, @startDate)),0) as Balance,
		isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0)  as interestCharged,
		isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as NewAdvances,
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) as daysDue,
		isnull((select sum(rs.principalPayment-rs.principalBalance+rs.interestPayment-rs.interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID and disbursementDate > dateadd(MONTH, -6, @startDate)),0) as received,
		0 as writtenOff,
		0 as changesInClass
	from ln.client c 
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.category cat on c.categoryID=cat.categoryID
	where 
		datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= @startDate), 
			(select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @startDate)) > 151
)
select
	'Previous balance(Gross)' as [Description],
	isnull(sum(case when part=0 then Balance end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance end),0) as beyond_150
from tbl
union all
select
	'Add new advances made during half-year' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances end),0) as beyond_150
from tbl
union all
select
	'Add interest charged for the half year' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged end),0) as beyond_150
from tbl
union all
select
	'Less amount received' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received end),0) as beyond_150
from tbl
union all
select
	'Less amount written off' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received-writtenOff end),0) as beyond_150
from tbl
union all
select
	'(+/-) changes in classification from previous half-year' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received-changesInClass end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as beyond_150
from tbl
union all
select
	'Current balance (Gross)' as [Description],
	isnull(sum(case when part=0 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as current_loans,
	isnull(sum(case when part between 1 and 30 then Balance+ NewAdvances+interestCharged-received-changesInClass end),0) as up_to_30_Days,
	isnull(sum(case when part between 31 and 60 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_31_to_60,
	isnull(sum(case when part between 61 and 90 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_61_to_90,
	isnull(sum(case when part between 91 and 120 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_91_to_120,
	isnull(sum(case when part between 121 and 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as from_121_to_150,
	isnull(sum(case when part > 150 then Balance+ NewAdvances+interestCharged-received-writtenOff-changesInClass end),0) as beyond_150
from tbl
go