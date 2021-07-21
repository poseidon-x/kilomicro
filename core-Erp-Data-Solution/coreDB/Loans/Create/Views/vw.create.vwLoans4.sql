use coreDB
go

alter view ln.vwLoans4
with encryption
as 
select
	clientID,
	clientName,
	accountNumber,
	loanID,
	loanNo,
	loanTypeName,
	isnull(min([date]), getdate()) as [date],
	isnull(sum(principal),0) principal,
	isnull(sum(interest),0) interest,
	isnull(sum(addInt),0) addInt,
	isnull(sum(procFee),0) procFee,
	isnull(sum(paidPrinc),0) paidPrinc,
	isnull(sum(paidInt),0) paidInt,
	isnull(sum(paidAddInt),0) paidAddInt,
	isnull(sum(paidProcFee),0) paidProcFee,
	isnull(sum(writtenOff), 0) as writtenOff
from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		l.disbursementDate as [date],
		'Loan Approval' as [Description],
		lt.loanTypeName,
		l.amountDisbursed as principal,
		0 as interest,
		0 as addInt,
		0 as procFee,
		0 as paidPrinc,
		0 as paidInt,
		0 as paidAddInt,
		0 as paidProcFee,
		0 as writtenOff,
		1 as sortCode
	from  ln.loan l 
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		l.disbursementDate as [date],
		'Processing Fee' as [Description],
		lt.loanTypeName,
		0 as principal,
		0 as interest,
		0 as addInt,
		l.processingFee as procFee,
		0 as paidPrinc,
		0 as paidInt,
		0 as paidAddInt,
		0 as paidProcFee,
		0 as writtenOff,
		2 as sortCode
	from ln.loan l 
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.penaltyDate as [date],
		'Additional Interest' as [Description],
		lt.loanTypeName,
		0 as principal,
		0 as interest,
		lp.penaltyFee as addInt,
		0 as procFee,
		0 as paidPrinc,
		0 as paidInt,
		0 as paidAddInt,
		0 as paidProcFee,
		0 as writtenOff,
		3 as sortCode
	from ln.loanPenalty lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		l.disbursementDate as [date],
		'Interest on Loan' as [Description],
		lt.loanTypeName,
		0 as principal,
		lp.interestPayment as interest,
		0 as addInt,
		0 as procFee,
		0 as paidPrinc,
		0 as paidInt,
		0 as paidAddInt,
		0 as paidProcFee,
		lp.interestWritenOff as writtenOff,
		3 as sortCode
	from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Loan Repayment' as [Description],
		lt.loanTypeName,
		0 as principal,
		0 as interest,
		0 as addInt,
		0 as procFee,
		lp.principalPaid as paidPrinc,
		0 as paidInt,
		0 as paidAddInt,
		0 as paidProcFee,
		0 as writtenOff,
		4 as sortCode
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where lp.repaymentTypeID in (1,2,3)
		and loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Loan Repayment' as [Description],
		lt.loanTypeName,
		0 as principal,
		0 as interest,
		0 as addInt,
		0 as procFee,
		0 as paidPrinc,
		lp.interestPaid as paidInt,
		0 as paidAddInt,
		0 as paidProcFee,
		0 as writtenOff,
		4 as sortCode
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where lp.repaymentTypeID in (1,2,3)
		and loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Processing Fee Payment' as [Description],
		lt.loanTypeName,
		0 as principal,
		0 as interest,
		0 as addInt,
		0 as procFee,
		0 as paidPrinc,
		0 as paidInt,
		0 as paidAddInt,
		lp.feePaid as paidProcFee,
		0 as writtenOff,
		4 as sortCode
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where lp.repaymentTypeID in (6)
		and loanStatusID <> 7
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Additional Interest Payment' as [Description],	
		lt.loanTypeName,	
		0 as principal,
		0 as interest,
		0 as addInt,
		0 as procFee,
		0 as paidPrinc,
		0 as paidInt,
		lp.penaltyPaid as paidAddInt,
		0 as paidProcFee,
		0 as writtenOff,
		4 as sortCode
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	where lp.repaymentTypeID in (7)
		and loanStatusID <> 7
) t
group by 
	clientID,
	clientName,
	accountNumber,
	loanID,
	loanNo,
	loanTypeName