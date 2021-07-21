use coreDB
go

select 
	Date,
		isnull(naration,'') naration ,
		isnull(principal, 0) as principal,
		isnull(,0) as expectedInterest,
		isnull(,0) as AmountPaid,
		isnull(,0) as Penalty, 
		isnull(,0) clientID,
		isnull(,'') clientName,
		isnull(,'') accountNumber,
		isnull(,'') addressLIne1,
		isnull(,'') workPhone,
		isnull(,'') mobilePhone,
		isnull(,'') homePhone,
		isnull(,'') addressLine2,
		isnull(,'') cityTown,
		isnull(interestRate,0) interestRate,
		isnull(loanTenure,0)  loanTenure,
		isnull(loanNo,'')  loanNo,
		isnull(,0) loanID,
		lt.loanTypeName,
		[image]
from
(
	select l.disbursementDate as Date,
		'Loan Apporved' as naration ,
		amountdisbursed as principal,
		0.0 as expectedInterest,
		0.0 as AmountPaid,
		0.0 as Penalty, 
		c.clientID,
		c.clientName,
		c.accountNumber,
		c.addressLIne1,
		c.workPhone,
		c.mobilePhone,
		c.homePhone,
		c.addressLine2,
		c.cityTown,
		l.interestRate,
		l.loanTenure,
		loanNo,
		l.loanID,
		lt.loanTypeName,
		[image]
	from ln.vwClients c
		inner join ln.loan l on c.clientID = l.clientID 
		inner join ln.loanType lt on lt.loanTypeID=l.loanTypeID
	union all
	select repaymentDate as Date,
		'Principal + Interest' as naration ,
		ln.loanBalanceAsAt(l.loanID, r.repaymentDate) as principal,
		interestPayment as expectedInterest,
		0.0 as AmountPaid,
		0.0 as Penalty, 
		c.clientID,
		c.clientName,
		c.accountNumber,
		c.addressLIne1,
		c.workPhone,
		c.mobilePhone,
		c.homePhone,
		c.addressLine2,
		c.cityTown,
		l.interestRate,
		l.loanTenure,
		loanNo,
		l.loanID		,
		lt.loanTypeName,
		[image]
	from ln.vwClients c
		inner join ln.loan l on c.clientID = l.clientID 
		inner join ln.repaymentSchedule r on r.loanID=l.loanID
		inner join ln.loanType lt on lt.loanTypeID=l.loanTypeID
	union all
	select repaymentDate as Date,
		'Loan Repayment' as naration ,
		ln.loanBalanceAsAt(l.loanID, r.repaymentDate) as principal,
		0 as expectedInterest,
		amountPaid as AmountPaid,
		0.0 as Penalty, 
		c.clientID,
		c.clientName,
		c.accountNumber,
		c.addressLIne1,
		c.workPhone,
		c.mobilePhone,
		c.homePhone,
		c.addressLine2,
		c.cityTown,
		l.interestRate,
		l.loanTenure,
		loanNo,
		l.loanID	,
		lt.loanTypeName,
		[image]
	from ln.vwClients c
		inner join ln.loan l on c.clientID = l.clientID 
		inner join ln.loanRepayment r on r.loanID=l.loanID
		inner join ln.loanType lt on lt.loanTypeID=l.loanTypeID
	where repaymentTypeID in (1, 2, 3, 7)
) t	
	