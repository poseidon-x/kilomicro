--select * [ln].[vwOutstandingLoans];

CREATE view [ln].[vwOutstandingLoans]
with encryption
AS

select clientName,loanNo,sum(Payable) as Payable,sum(Paid) as Paid,
	max(PaidDate) as 'LastRepaymentDate',max(DueDate) as 'LastDueDate',max(disbursementDate) as 'disbursementDate',
	max(amountDisbursed) as 'amountDisbursed',loanGroupId,loanGroupName,loanGroupNumber
from
(
SELECT        concat(cl.Surname, ' ', cl.otherNames) AS clientName, l.loanNo,0 as 'Paid', interestPayment + principalPayment AS 'Payable',
	repaymentDate as 'DueDate',null as 'PaidDate',l.disbursementDate,l.amountDisbursed,lg.loanGroupId,lg.loanGroupName,lg.loanGroupNumber
FROM            ln.repaymentSchedule rs INNER JOIN
                         ln.loan l ON l.loanID = rs.loanID INNER JOIN
                         ln.client cl ON cl.clientID = l.clientID
						 LEFT JOIN LN.loanGroupClient lgc on lgc.clientId = cl.clientID
						 LEFT JOIN ln.loanGroup lg on lg.loanGroupId = lgc.loanGroupId
WHERE        repaymentDate < GETDATE() AND loanStatusID = 4
--GROUP BY  loanNo, concat(cl.Surname, ' ', cl.otherNames)
UNION ALL
SELECT        concat(cl.Surname, ' ', cl.otherNames) AS clientName, l.loanNo, amountPaid AS 'Paid', 0 as 'Payable',
	null as 'DueDate',repaymentDate as 'PaidDate',l.disbursementDate,l.amountDisbursed,lg.loanGroupId,lg.loanGroupName,lg.loanGroupNumber
FROM            ln.loanRepayment lr INNER JOIN
                         ln.loan l ON l.loanID = lr.loanID INNER JOIN
                         ln.client cl ON cl.clientID = l.clientID
						 LEFT JOIN LN.loanGroupClient lgc on lgc.clientId = cl.clientID
						 LEFT JOIN ln.loanGroup lg on lg.loanGroupId = lgc.loanGroupId
WHERE        repaymentDate < GETDATE() AND loanStatusID = 4 AND repaymentTypeID = 1
--GROUP BY loanNo, concat(cl.Surname, ' ', cl.otherNames)
)  t
group by clientName,loanNo,loanGroupId,loanGroupName,loanGroupNumber