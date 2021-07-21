use coreDB
go

alter view ln.vwSchedule
with encryption as 
select	
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	principalPayment,
	interestPayment,
	principalBalance,
	interestBalance,
	repaymentDate,
	amountDisbursed,
	rm.repaymentModeName,
	principalPayment+
	interestPayment as totalPayment
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.repaymentSchedule rs on l.loanID=rs.loanID
	inner join ln.repaymentMode rm on l.repaymentModeID = rm.repaymentModeID
go