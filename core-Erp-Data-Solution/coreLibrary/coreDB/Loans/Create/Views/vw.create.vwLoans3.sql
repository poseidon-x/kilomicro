use coreDB
go

alter view ln.vwLoans3 
with encryption as 
select
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	cat.categoryName,
	l.amountDisbursed,
	l.processingFee,
	l.processingFeeBalance,
	l.loanNo,
	isnull(l.disbursementDate, l.applicationDate) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select top 1 lc.fairValue from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull((select sum(interestBalance+principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as balance,
	isnull((select sum(interestPaid+principalPaid+feePaid+penaltyPaid) from ln.loanRepayment rs where rs.loanID=l.loanID),0) as paid,
	isnull((select sum(penaltyPaid) from ln.loanRepayment rs where rs.loanID=l.loanID),
		isnull((select sum(penaltyFee) from ln.loanPenalty rs where rs.loanID=l.loanID),0)) as penalty,
	isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interestBalance,
	isnull((select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principalBalance,
	isnull(datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()),0) as daysDue,
	isnull((select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID),applicationDate) as loanEndDate,
	isnull((select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID),applicationDate) as lastPaymentDate,
	isnull(
			(select avg(datediff(day, t1.repaymentDate, t2.repaymentDate)) from
			(select repaymentDate, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t1,			
			(select repaymentDate, row_number() over (order by repaymentDate) as rn from ln.loanRepayment rs where rs.loanID=l.loanID) t2
			where t1.rn=t2.rn)
		,9999) as repaymentDateDelta,
	isnull(
			(select avg(((t1.payment- t2.payment)/t1.payment)*100.0) from
			(select principalPayment+interestPayment as payment, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t1,			
			(select principalPayment+interestPayment-principalBalance-interestBalance as payment, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t2
			where t1.rn=t2.rn)
		,9999) as repaymentAmountDelta
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
where l.disbursementDate is not null

go

alter view ln.vwDeposits
with encryption as 
select
	l.depositID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	dt.depositTypeName,
	l.amountInvested,
	l.interestAccumulated,
	l.firstDepositDate,
	l.interestBalance,
	l.principalBalance
from ln.client c 
	inner join ln.deposit l on c.clientID=l.clientID
	inner join ln.depositType dt on l.depositTypeID=dt.depositTypeID 
go