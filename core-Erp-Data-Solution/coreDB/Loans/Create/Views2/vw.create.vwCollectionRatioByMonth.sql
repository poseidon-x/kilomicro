use coreDB
go

create view vwCollectionRatioByMonth
with encryption
as
select
	isnull(isnull(p.month, d.month),0) as month,
	isnull(round(p.paid,2),0) as paid,
	isnull(round(d.expected,2),0) as expected,
	isnull(round (case when expected=0 then 0 else (paid/expected)*100.0 end, 2), 0) as collection
from
(
	SELECT        CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2) AS month, 
							 SUM(principalPaid + interestPaid) AS paid
	FROM            ln.loanRepayment AS lr
	WHERE        (repaymentDate >= DATEADD(year, - 1, GETDATE()))
	GROUP BY CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2)
) p full outer join
(
	SELECT        CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2) AS month, 
							 SUM(principalPayment+interestPayment) AS expected
	FROM            ln.repaymentSchedule AS lr
	WHERE        (repaymentDate >= DATEADD(year, - 1, GETDATE())) and repaymentDate <= getDate()
	GROUP BY CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2)
) d on p.month = d.month 
go

create proc getCollectionRatioByMonth
(
	@branchID int
)
with encryption
as
select
	isnull(isnull(p.month, d.month),0) as month,
	isnull(round(p.paid,2),0) as paid,
	isnull(round(d.expected,2),0) as expected,
	isnull(round (case when expected=0 then 0 else (paid/expected)*100.0 end, 2), 0) as collection
from
(
	SELECT        CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2) AS month, 
							 SUM(principalPaid + interestPaid) AS paid
	FROM            ln.loanRepayment AS lr 
		inner join ln.loan l on lr.loanID = l.loanID
		inner join ln.client c on l.clientID=c.clientID
	WHERE        (repaymentDate >= DATEADD(year, - 1, GETDATE()))
		and (@branchID is null or branchID = @branchID)
	GROUP BY CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2)
) p full outer join
(
	SELECT        CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2) AS month, 
							 SUM(principalPayment+interestPayment) AS expected
	FROM            ln.repaymentSchedule AS lr
		inner join ln.loan l on lr.loanID = l.loanID
		inner join ln.client c on l.clientID=c.clientID
	WHERE        (repaymentDate >= DATEADD(year, - 1, GETDATE())) and repaymentDate <= getDate()
		and (@branchID is null or branchID = @branchID)
	GROUP BY CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2)
) d on p.month = d.month 
go
