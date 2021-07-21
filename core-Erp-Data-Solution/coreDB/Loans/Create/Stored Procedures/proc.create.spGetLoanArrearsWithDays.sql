
use coreDB
go


CREATE procedure spGetLoanArrearsWithDays
(
    @endDate datetime
	
)
with encryption
AS
select clientID,
clientName,
loanNo,
loanID,
sum(Payable) as Payable,sum(Paid) as Paid,
 max(PaidDate) as 'LastRepaymentDate',max(DueDate) as 'LastDueDate',max(writeOffDate) as 'WriteOffDate',
 sum(WriteOff) as 'WriteOffAmount',max(disbursementDate) as 'disbursementDate',
 max(amountDisbursed) as 'amountDisbursed',loanGroupId,loanGroupName,loanGroupNumber,
 isnull(ln.getDaysDue(loanID, @endDate),0) as daysDue
from
(
SELECT  l.clientID as clientID, concat(cl.Surname, ' ', cl.otherNames) AS clientName, l.loanNo,l.loanID,0 as 'Paid', 0 as 'WriteOff',
	interestPayment + principalPayment AS 'Payable',
 repaymentDate as 'DueDate',null as 'PaidDate',null as 'WriteOffDate',l.disbursementDate,l.amountDisbursed,lg.loanGroupId,lg.loanGroupName,lg.loanGroupNumber
FROM            ln.repaymentSchedule rs INNER JOIN
                         ln.loan l ON l.loanID = rs.loanID INNER JOIN
                         ln.client cl ON cl.clientID = l.clientID
       LEFT JOIN LN.loanGroupClient lgc on lgc.clientId = cl.clientID
       LEFT JOIN ln.loanGroup lg on lg.loanGroupId = lgc.loanGroupId
WHERE        repaymentDate <= @endDate AND loanStatusID = 4
--GROUP BY  loanNo, concat(cl.Surname, ' ', cl.otherNames)
UNION ALL
SELECT   l.clientID as clientID,concat(cl.Surname, ' ', cl.otherNames) AS clientName, l.loanNo,l.loanID, amountPaid AS 'Paid',
	0 as 'WriteOff', 0 as 'Payable',
 null as 'DueDate',repaymentDate as 'PaidDate',null as 'WriteOffDate',l.disbursementDate,l.amountDisbursed,lg.loanGroupId,lg.loanGroupName,lg.loanGroupNumber
FROM            ln.loanRepayment lr INNER JOIN
                         ln.loan l ON l.loanID = lr.loanID INNER JOIN
                         ln.client cl ON cl.clientID = l.clientID
       LEFT JOIN LN.loanGroupClient lgc on lgc.clientId = cl.clientID
       LEFT JOIN ln.loanGroup lg on lg.loanGroupId = lgc.loanGroupId
WHERE        repaymentDate <= @endDate AND loanStatusID = 4 AND repaymentTypeID = 1
UNION ALL
SELECT l.clientID as clientID, concat(cl.Surname, ' ', cl.otherNames) AS clientName, l.loanNo,l.loanID, 0 AS 'Paid',
lw.writeOffAmount as 'WriteOff', 0 as 'Payable',
 null as 'DueDate',null as 'PaidDate',lw.writeOffDate as 'WriteOffDate',l.disbursementDate,l.amountDisbursed,lg.loanGroupId,lg.loanGroupName,lg.loanGroupNumber
FROM            ln.loanIterestWriteOff lw INNER JOIN
                         ln.loan l ON l.loanID = lw.loanID INNER JOIN
                         ln.client cl ON cl.clientID = l.clientID
       LEFT JOIN ln.loanGroupClient lgc on lgc.clientId = cl.clientID
       LEFT JOIN ln.loanGroup lg on lg.loanGroupId = lgc.loanGroupId
WHERE        lw.writeOffDate <= @endDate AND loanStatusID = 4
--GROUP BY loanNo, concat(cl.Surname, ' ', cl.otherNames)
)  t
group by clientID, clientName,loanNo,loanID,loanGroupId,loanGroupName,loanGroupNumber
order by loanGroupName,clientName
GO

--EXEC spGetLoanArrearsWithDays '2019-02-26'

