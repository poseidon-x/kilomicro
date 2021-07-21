use coreDB
go

alter view ln.vwSummarySheet
with encryption
as
select distinct
	c.clientID,
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end as clientName,
	c.accountNumber,
	isnull(s.employeeNumber, '') as employeeNumber,
	isnull(a.addressLine1, '') as addressLine1,
	isnull(a.addressLine2, '') as addressLine2,
	isnull(a.cityTown, '') as cityTown,
	isnull((select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=1 and cp.clientID=c.clientID),'') as workPhone,
	isnull((select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=2 and cp.clientID=c.clientID),'') as mobilePhone,
	isnull((select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=3 and cp.clientID=c.clientID),'') as homePhone,
	i.idNo,
	it.idNoTypeName,
	p.grossSalary,
	p.loanDeductionsNotOnPr+p.socialSecWelfare+p.tax+p.totalDeductions as totalDeductions,
	p.netSalary,
	p.amd,
	isnull(l.finalApprovalDate, getdate()) as finalApprovalDate,
	isnull(l.amountApproved,0) as amountApproved,
	isnull((select count(*) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as noOfDeductions,
	isnull((select max(principalPayment+interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as monthlyDeduction,
	isnull((select sum(principalPayment+interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as totalCollectible,	
	isnull(ag.surName + ', ' + ag.otherNames,'') as agentName,
	isnull(ag.agentNo,'') as agentNo,	
	isnull(st.surName + ', ' + st.otherNames,'') as staffName,
	isnull(st.staffNo,'') as staffNo
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.prLoanDetail p on l.loanID=p.loanID
	inner join ln.staffCategory s on c.clientID = s.clientID
	left outer join ln.clientAddress ca on c.clientID=ca.clientID
	left outer join ln.address a on ca.addressID = a.addressID
	inner join ln.idNo i on c.idNoID = i.idNoId
	inner join ln.idNoType it on i.idNoTypeID=it.idNoTypeID
	left outer join ln.agent ag on l.agentID = ag.agentID
	left outer join fa.staff st on l.staffID = st.staffID
where addresstypeID is null or addresstypeID=1
go