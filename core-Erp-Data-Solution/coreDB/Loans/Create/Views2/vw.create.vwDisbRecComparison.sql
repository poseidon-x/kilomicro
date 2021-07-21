use coreDB
go

create view ln.vwDisbRecComparison
with encryption
as
select
	isnull(isnull(p.month, d.month),0) as month,
	isnull(round(p.paid,2),0) as paid,
	isnull(round(d.disbursed,2),0) as disbursed
from
(
	SELECT        CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2) AS month, 
							 SUM(principalPaid + interestPaid) AS paid
	FROM            ln.loanRepayment AS lr
	WHERE        (repaymentDate >= DATEADD(year, - 1, GETDATE()))
	GROUP BY CAST(DATEPART(year, repaymentDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, repaymentDate) AS varchar(2)), 2)
) p full outer join
(
	SELECT        CAST(DATEPART(year, disbursementDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, disbursementDate) AS varchar(2)), 2) AS month, 
							 SUM(amountDisbursed) AS disbursed
	FROM            ln.loan AS lr
	WHERE        (disbursementDate >= DATEADD(year, - 1, GETDATE()))
	GROUP BY CAST(DATEPART(year, disbursementDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, disbursementDate) AS varchar(2)), 2)
) d on p.month = d.month 
go


create proc ln.getDisbRecComparison
(
	@branchID int
)
with encryption
as
select
	isnull(isnull(p.month, d.month),0) as month,
	isnull(round(p.paid,2),0) as paid,
	isnull(round(d.disbursed,2),0) as disbursed
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
	SELECT        CAST(DATEPART(year, disbursementDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, disbursementDate) AS varchar(2)), 2) AS month, 
							 SUM(amountDisbursed) AS disbursed
	FROM            ln.loan AS lr 
		inner join ln.client c on lr.clientID=c.clientID
	WHERE        (disbursementDate >= DATEADD(year, - 1, GETDATE()))
		and (@branchID is null or branchID = @branchID)
	GROUP BY CAST(DATEPART(year, disbursementDate) AS char(4)) + '' + RIGHT(REPLICATE('0', 2) + CAST(DATEPART(month, disbursementDate) AS varchar(2)), 2)
) d on p.month = d.month 
go
