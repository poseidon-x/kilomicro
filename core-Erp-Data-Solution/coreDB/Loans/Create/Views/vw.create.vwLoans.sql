use coreDB
go

alter view ln.vwLoans
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
	lt.loanTypeID,
	lt.loanTypeName,
	isnull(sum(rs.principalPayment),0) as principalPayment,
	isnull(sum(rs.interestPayment),0) as interestPayment,
	isnull(sum(rs.principalPayment+rs.interestPayment)/(select isnull(sum (fairValue), 1) from ln.loanCollateral lc where lc.loanID = l.loanID),0) as riskRatio,
	isnull(isnull(sum(rs.principalPayment+rs.interestPayment),1)/(select case when max(lc.financialTypeID) = 1 then count(rs.repaymentScheduleID) else 1 end *
		isnull(sum (revenue-expenses), 1) from ln.loanFinancial lc where lc.loanID = l.loanID),0) as affordabilityRatio,
	isnull(dateadd(mm,loanTenure, l.finalApprovalDate), getdate()) as expiryDate,
	isnull(loanTenure,0) as loanTenure,
	isnull(finalApprovalDate,getdate()) as approvalDate,
	l.amountApproved + isnull(sum(rs.interestPayment),0) as totalPayment,
	isnull(checkedBy,'') as checkedBy,
	isnull(approvedBy,'') as approvedBy,
	isnull(enteredBy, '') as enteredBy,
	isnull(lt.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name 
from ln.loan l 
	inner join ln.client c on l.clientID=c.clientID
	inner join ln.repaymentSchedule rs on l.loanID=rs.loanID
	inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
	left join dbo.comp_prof AS company on lt.companyId = company.companyId

where interestBalance>0 or principalBalance>0 
group by c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	l.disbursementDate,
	l.amountDisbursed,
	l.amountApproved,
	l.amountRequested,
	lt.loanTypeID,
	lt.loanTypeName,
	isnull(year(disbursementDate),0),
	isnull(month(disbursementDate),0),
	loanTenure,
	finalApprovalDate,
	isnull(checkedBy,'') ,
	isnull(approvedBy,'') ,
	isnull(enteredBy, ''),
	isnull(lt.companyId,0),
	isnull(company.comp_Name, '')  
go
go