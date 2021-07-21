use coreDB
go

alter view ln.vwProjection
with encryption 
as
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	rs.repaymentDate,
	rs.principalBalance,
	rs.interestBalance,
	0.0 applicationFeeBalance,
	0.0 processingFeeBalance, 
	0.0 commissionBalance,
	'Principal + Interest Repayment' as TypeOfBalance
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.repaymentSchedule rs on l.loanID=rs.loanID
where rs.interestBalance> 0 or rs.principalBalance>0
union all
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	isnull(l.finalApprovalDate,getdate()) as repaymentDate,
	0.0 principalBalance,
	0.0 interestBalance,
	l.applicationFeeBalance,
	0.0 processingFeeBalance, 
	0.0 commissionBalance,
	'Application Fee' as TypeOfBalance
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID 
where l.applicationFeeBalance>0
union all
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	isnull(l.finalApprovalDate,getdate()) as repaymentDate,
	0.0 principalBalance,
	0.0 interestBalance,
	0.0 applicationFeeBalance,
	l.processingFeeBalance, 
	0.0 commissionBalance,
	'Processing Fee' as TypeOfBalance
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID 
where l.processingFeeBalance>0
union all
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	isnull(l.finalApprovalDate,getdate()) as repaymentDate,
	0.0 principalBalance,
	0.0 interestBalance,
	0.0 applicationFeeBalance,
	0.0 processingFeeBalance,
	l.commissionBalance,
	'Commission' as TypeOfBalance
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID 
where l.commissionBalance>0
go
