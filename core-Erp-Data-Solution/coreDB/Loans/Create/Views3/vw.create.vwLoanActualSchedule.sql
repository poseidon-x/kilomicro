use coreDB
go


alter view ln.vwLoanActualSchedule
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
	isnull(principalPayment, 0) as principal,
	isnull(interestPayment, 0) as interest, 
	isnull(case when (select min(rs2.repaymentDate) from ln.repaymentSchedule  rs2 where rs2.loanID=l.loanID)>disbursementDate then dateadd(mm, -1, repaymentDate)
		else repaymentDate end, getDate())  as [date],
	isnull(row_number() over (partition by l.loanID order by repaymentDate), 0) as serialNum,
	isnull(amountDisbursed, 0) as amountDisbursed,
	isnull(disbursementDate, getdate()) as disbursementDate,
	isnull(0.0, 0) as amountPaid,
	isnull([penaltyAmount], 0) as penaltyAmount,
	isnull(0.0 , 0) as principalPaid,
	isnull(0.0 , 0) as interestPaid,
	isnull(0.0 , 0) as penaltyPaid,
	isnull(0.0 , 0) as feePaid,
	isnull([image], 0x0) as [image],
	isnull(0, 0) as modeOfPaymentId,
	isnull(lt.companyId,0)as CompanyId,
    isnull(company.comp_Name, '') AS comp_Name
from ln.repaymentSchedule rs 
	inner join ln.loan l on rs.loanID = l.loanID
	inner join ln.vwClients c on l.clientID = c.clientID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	 left join dbo.comp_prof AS company on lt.companyId = company.comp_prof_id

WHERE loanStatusId = 4
union all
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
	isnull(0, 0) as principal,
	isnull(0, 0) as interest, 
	isnull(penaltyDate, getDate())  as [date],
	isnull(row_number() over (partition by l.loanID order by penaltyDate), 0) as serialNum,
	isnull(amountDisbursed, 0) as amountDisbursed,
	isnull(disbursementDate, getdate()) as disbursementDate,
	isnull(0.0, 0) as amountPaid,
	isnull([penaltyFee], 0) as penaltyAmount,
	isnull(0.0 , 0) as principalPaid,
	isnull(0.0 , 0) as interestPaid,
	isnull(0.0 , 0) as penaltyPaid,
	isnull(0.0 , 0) as feePaid,
	isnull([image], 0x0) as [image],
	isnull(0, 0) as modeOfPaymentId,
	isnull(lt.companyId,0)as CompanyId,
    isnull(company.comp_Name, '') AS comp_Name
from ln.loanPenalty rs 
	inner join ln.loan l on rs.loanID = l.loanID
	inner join ln.vwClients c on l.clientID = c.clientID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left join dbo.comp_prof AS company on lt.companyId = company.comp_prof_id
WHERE loanStatusId = 4
union all
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
	isnull(0.0, 0) as principal,
	isnull(0.0, 0) as interest,
	isnull(repaymentDate, getDate())  as [date], 
	isnull(row_number() over (partition by l.loanID order by repaymentDate),0) as serialNum,
	isnull(amountDisbursed, 0) as amountDisbursed,
	isnull(disbursementDate, getdate()) as disbursementDate,
	isnull(amountPaid, 0) as amountPaid,
	0.0 as penaltyAmount,
	isnull(principalPaid , 0) as principalPaid,
	isnull(interestPaid , 0) as interestPaid,
	isnull(penaltyPaid , 0) as penaltyPaid,
	isnull(feePaid, 0) as feePaid,
	isnull([image], 0x0) as [image],
	isnull(modeOfPaymentId, 0) as modeOfPaymentId,
	isnull(lt.companyId,0)as CompanyId,
    isnull(company.comp_Name, '') AS comp_Name
from ln.loanRepayment rs 
	inner join ln.loan l on rs.loanID = l.loanID
	inner join ln.vwClients c on l.clientID = c.clientID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left join dbo.comp_prof AS company on lt.companyId = company.comp_prof_id
where repaymentTypeID in (1,2,3,7) 
