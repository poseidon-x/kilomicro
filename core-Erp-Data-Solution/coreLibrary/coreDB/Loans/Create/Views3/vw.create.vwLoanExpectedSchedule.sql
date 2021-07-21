use coreDB
go


alter view ln.vwLoanExpectedSchedule
with encryption as
select
	isnull(c.clientID, 0) as clientID,
	isnull(c.clientName, '') as clientName,
	isnull(c.accountNumber, '') as accountNumber,
	isnull(addressLine1,'') addressLine1,
	isnull(workPhone,'') workPhone,
	isnull(mobilePhone,'') mobilePhone,
	isnull(homePhone,'') homePhone,
	isnull(addressLine2,'') addressLine2,
	isnull(cityTown,'') cityTown,
	isnull(interestRate,0) interestRate,
	isnull(loanTenure,0)  loanTenure,
	isnull(loanNo,'')  loanNo,
	isnull(l.loanID,0) loanID,
	isnull(lt.loanTypeName, '') loanTypeName, 
	isnull([image], 0x0) as [image],
	isnull(case when [origPrincipalPayment] is null or [origPrincipalPayment]<=0 then principalPayment else [origPrincipalPayment] end, 0) as principal,
	isnull(case when [origInterestPayment] is null or [origInterestPayment]<=0 then interestPayment else [origInterestPayment] end, 0) as interest,
	isnull(case when (select min(rs2.repaymentDate) from ln.repaymentSchedule  rs2 where rs2.loanID=l.loanID)>disbursementDate then repaymentDate
		else dateadd(mm, 1,repaymentDate) end, getDate())  as repaymentDate,
	isnull(case when (select min(rs2.repaymentDate) from ln.repaymentSchedule  rs2 where rs2.loanID=l.loanID)>disbursementDate then dateadd(mm, -1, repaymentDate)
		else repaymentDate end, getDate())  as interestDate,
	isnull(row_number() over (partition by l.loanID order by repaymentDate), 0) as serialNum,
	isnull(amountDisbursed, 0) as amountDisbursed,
	isnull(disbursementDate, getdate()) as disbursementDate,
	isnull(lt.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name 
from ln.repaymentSchedule rs 
	inner join ln.loan l on rs.loanID = l.loanID
	inner join ln.vwClients c on l.clientID = c.clientID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left join dbo.comp_prof AS company on lt.companyId = company.companyId
where principalPayment>0 and loanStatusId=4
