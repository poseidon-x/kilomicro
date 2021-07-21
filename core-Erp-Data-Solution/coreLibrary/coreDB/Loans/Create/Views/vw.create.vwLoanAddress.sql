 use coreDB
 go

 alter view ln.vwLoanAddress
 with encryption as
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	isnull(year(disbursementDate),0) as year0,
	isnull(month(disbursementDate),0) as month0,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	l.amountDisbursed,
	l.amountApproved,
	l.amountRequested,
	a.addressLine1,
	a.addressLine2,
	a.cityTown,
	(select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=1 and cp.clientID=c.clientID) as workPhone,
	(select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=2 and cp.clientID=c.clientID) as mobilePhone,
	(select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=3 and cp.clientID=c.clientID) as homePhone,
	case when (select isnull(sum (fairValue), 1) from ln.loanCollateral lc where lc.loanID = l.loanID)=0 then 0 
	else isnull((select sum(rs.principalPayment+rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID)/
		(select isnull(sum (fairValue), 1) from ln.loanCollateral lc where lc.loanID = l.loanID),0)*100 end as riskRatio,
	case when ((select  isnull(sum ((revenue-expenses-otherCosts)/frequencyID), 1) from ln.loanFinancial lc where lc.loanID = l.loanID)*loantenure)=0 then 0
		else isnull(isnull((select sum(rs.principalPayment+rs.interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),1)/l.repaymentModeID /
		((select  isnull(sum ((revenue-expenses-otherCosts)/frequencyID), 1) from ln.loanFinancial lc where lc.loanID = l.loanID)*loantenure),0)*100 end 
		as affordabilityRatio,
	(select top 1 [image] from ln.clientImage ai inner join ln.[image] i on ai.imageID = i.imageID and ai.clientID = c.clientID) as [image],
	l.approvalComments,
	l.creditOfficerNotes,
	l.interestRate,
	l.loanTenure,
	isnull((select min(rs.repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID), getDate()) as startDate,
	isnull((select max(rs.repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID), getDate()) as endDate,
	s.surName + ', ' + s.otherNames  as staffName

from ln.loan l 
	inner join ln.client c on l.clientID=c.clientID
	left outer join ln.clientAddress ca on c.clientID=ca.clientID
	left outer join ln.address a on ca.addressID = a.addressID
	left outer join fa.staff s on l.staffID = s.staffID

where addressTypeID is null or addressTypeID = 1
go