use coreDB
go

alter view ln.vwLoanAgreement
with encryption as
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo, 
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull(l.finalApprovalDate, getdate()) as finalApprovalDate,
	isnull(l.applicationDate, getdate()) as applicationDate,
	l.amountDisbursed,
	l.amountApproved,
	l.amountRequested,
	lt.loanTypeID,
	lt.loanTypeName,
	l.interestRate,
	l.loanTenure,
	isnull((select sum(rs.principalPayment) from ln.repaymentSchedule rs where l.loanID=rs.loanID),0) as principalPayment,
	isnull((select sum(rs.interestPayment) from ln.repaymentSchedule rs where l.loanID=rs.loanID),0) as interestPayment,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),'') as collateralType,
	isnull((select top 1 lc.fairValue from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull((select max(p.phoneNo) from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=1 and cp.clientID=c.clientID),'') as workPhone,
	isnull((select max(p.phoneNo) from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=2 and cp.clientID=c.clientID),'') as mobilePhone,
	isnull((select max(p.phoneNo) from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=3 and cp.clientID=c.clientID),'') as homePhone,
	isnull((select max(p.addressLine1) from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=1 and cp.clientID=c.clientID),'') as phyAddr1,
	isnull((select max(p.addressLine2) from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=1 and cp.clientID=c.clientID),'') as phyAddr2,
	isnull((select max(p.cityTown) from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=1 and cp.clientID=c.clientID),'') as phyCity,
	isnull((select max(p.addressLine1) from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=2 and cp.clientID=c.clientID),'') as mailAddr1,
	isnull((select max(p.addressLine2) from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=2 and cp.clientID=c.clientID),'') as mailAddr2,
	isnull((select max(p.cityTown) from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=2 and cp.clientID=c.clientID),'') as mailCity,
	row_number() over (partition by l.clientID order by l.applicationDate) as rowNum,
	isnull(l.processingFee,0) as processingFee,
	isnull(c.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name 
from ln.loan l 
	inner join ln.client c on l.clientID=c.clientID 
	inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			left join dbo.comp_prof AS company on c.companyId = company.companyId

where loanStatusID <> 7