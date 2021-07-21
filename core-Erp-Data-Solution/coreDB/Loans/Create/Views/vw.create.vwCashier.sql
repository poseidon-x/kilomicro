use coreDB
go

alter view vwCashierRepayments 
with encryption as
select 
	isnull (amountPaid,0) as amountPaid, 
	isnull (interestPaid,0) as interestPaid,
	isnull (principalPaid,0) as principalPaid,
	isnull(repaymentTypeName, '') as repaymentTypeName,
	isnull(modeOfPaymentName,'') as modeOfPaymentName,
	clientID,
	isnull (clientName, '')  as clientName,
	accountNumber,
	loanID,
	loanNo,
	isnull(penaltyPaid,0) as penaltyPaid,
	isnull(feePaid, 0) as feePaid,
	repaymentDate,
	cashierTillID,
	userName,
	full_name,
	isnull(agentName,'') as agentName,
	posted
from (
select
	lr.amount amountPaid,
	case when rt.repaymentTypeName='Interest Only' then lr.amount 
		when rt.repaymentTypeName='Principal and Interest' and lr.interestAmount=0 then 
			round((select suM(interestPayment)/sum(interestPayment + PrincipalPayment)
				from ln.repaymentSchedule rs where rs.loanID=l.loanID)*lr.amount,2)
		else lr.interestAmount end interestPaid,
	case when rt.repaymentTypeName='Principal Only' then lr.amount
		when rt.repaymentTypeName='Principal and Interest' and lr.interestAmount=0 then 
			round((select suM(PrincipalPayment)/sum(interestPayment + PrincipalPayment)
				from ln.repaymentSchedule rs where rs.loanID=l.loanID)*lr.amount,2)
		else lr.principalAmount end principalPaid,
	isnull(rt.repaymentTypeName, '') as repaymentTypeName,
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	isnull(c.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	case when rt.repaymentTypeName='Penalty' then lr.amount else lr.addInterestAmount end penaltyPaid,
	case when rt.repaymentTypeName='Processing Fee' then lr.amount else lr.feeAmount end feePaid,
	lr.txDate repaymentDate,
	lr.cashierTillID,
	ct.userName,
	u.full_name,
	isnull(isnull(a.surname+', ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName,
	lr.posted
from ln.cashierReceipt lr
	inner join ln.loan l on lr.loanID=l.loanID
	left outer join ln.repaymentType rt on lr.repaymentTypeID=rt.repaymentTypeID
	left outer join ln.modeOfPayment mp on lr.paymentModeID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID
	inner join ln.cashiersTill ct on ct.cashiersTillID=lr.cashierTillID
	inner join dbo.users u on lower(ltrim(rtrim(ct.userName)))  = lower(ltrim(rtrim(u.user_name))) 
	left join ln.agent a on a.agentId = l.agentId
	left join fa.staff s on s.staffId = l.staffId
	left join dbo.comp_prof AS company on c.companyId = company.companyId

union all
select
	lr.amount amountPaid,
	0.0 interestPaid,
	lr.amount principalPaid,
	'Group Susu Contribution' as repaymentTypeName,
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.susuAccountId as loanID,
	l.susuAccountNo as loanNo,
	0.0 penaltyPaid,
	0.0 feePaid,
	lr.contributionDate repaymentDate,
	isnull(isnull(lr.staffId, lr.agentId), 0) as cashierTillID,
	isnull(isnull(u.[user_name], a.surname), '') as userName,
	isnull(isnull(isnull(u.full_name, s.surname+', ' + s.otherNames), a.surname+', ' + a.otherNames), ''),
	isnull(isnull(a.surname+', ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName,
	lr.posted,
	isnull(c.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name
from ln.susuContribution lr
	inner join ln.susuAccount l on lr.susuAccountId=l.susuAccountId 
	left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID 
	left join fa.staff s on s.staffId = lr.staffId
	left join ln.agent a on a.agentId = lr.agentId
	left join dbo.users u on lower(ltrim(rtrim(s.userName)))  = lower(ltrim(rtrim(u.user_name))) 
	left join dbo.comp_prof AS company on c.companyId = company.companyId

union all
select
	lr.amount amountPaid,
	0.0 interestPaid,
	lr.amount principalPaid,
	'Normal Susu Contribution' as repaymentTypeName,
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.regularSusuAccountId as loanID,
	l.regularSusuAccountNo as loanNo,
	0.0 penaltyPaid,
	0.0 feePaid,
	lr.contributionDate repaymentDate,
	isnull(isnull(lr.staffId, lr.agentId), 0) as cashierTillID,
	isnull(isnull(u.[user_name], a.surname), '') as userName,
	isnull(isnull(isnull(u.full_name, s.surname+', ' + s.otherNames), a.surname+', ' + a.otherNames), ''),
	isnull(isnull(a.surname+', ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName,
	lr.posted,
	isnull(c.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name
from ln.regularSusuContribution lr
	inner join ln.regularSusuAccount l on lr.regularSusuAccountId=l.regularSusuAccountId 
	left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID 
	left join fa.staff s on s.staffId = lr.staffId
	left join ln.agent a on a.agentId = lr.agentId
	left join dbo.users u on lower(ltrim(rtrim(s.userName)))  = lower(ltrim(rtrim(u.user_name))) 
	left join dbo.comp_prof AS company on c.companyId = company.companyId

	) t






use coreDB
go

alter view vwCashierRepaymentsGrouped 
with encryption as
select 
	isnull (amountPaid,0) as amountPaid, 
	isnull (interestPaid,0) as interestPaid,
	isnull (principalPaid,0) as principalPaid,
	isnull(repaymentTypeName, '') as repaymentTypeName,
	isnull(modeOfPaymentName,'') as modeOfPaymentName,
	clientID,
	isnull (clientName, '')  as clientName,
	accountNumber,
	loanID,
	loanNo,
	isnull(penaltyPaid,0) as penaltyPaid,
	isnull(feePaid, 0) as feePaid,
	repaymentDate,
	cashierTillID,
	userName,
	full_name,
	isnull(agentName,'') as agentName,
	posted,
	loanGroupName,
	loanGroupNumber
from (
select
	lr.amount amountPaid,
	case when rt.repaymentTypeName='Interest Only' then lr.amount 
		when rt.repaymentTypeName='Principal and Interest' and lr.interestAmount=0 then 
			round((select suM(interestPayment)/sum(interestPayment + PrincipalPayment)
				from ln.repaymentSchedule rs where rs.loanID=l.loanID)*lr.amount,2)
		else lr.interestAmount end interestPaid,
	case when rt.repaymentTypeName='Principal Only' then lr.amount
		when rt.repaymentTypeName='Principal and Interest' and lr.interestAmount=0 then 
			round((select suM(PrincipalPayment)/sum(interestPayment + PrincipalPayment)
				from ln.repaymentSchedule rs where rs.loanID=l.loanID)*lr.amount,2)
		else lr.principalAmount end principalPaid,
	isnull(rt.repaymentTypeName, '') as repaymentTypeName,
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	case when rt.repaymentTypeName='Penalty' then lr.amount else lr.addInterestAmount end penaltyPaid,
	case when rt.repaymentTypeName='Processing Fee' then lr.amount else lr.feeAmount end feePaid,
	lr.txDate repaymentDate,
	lr.cashierTillID,
	ct.userName,
	u.full_name,
	isnull(isnull(a.surname+', ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName,
	lr.posted,
	isnull(lg.loanGroupName,'') as loanGroupName,
	isnull(lg.loanGroupNumber,'') as loanGroupNumber
from ln.cashierReceipt lr
	inner join ln.loan l on lr.loanID=l.loanID
	left outer join ln.repaymentType rt on lr.repaymentTypeID=rt.repaymentTypeID
	left outer join ln.modeOfPayment mp on lr.paymentModeID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID
	inner join ln.cashiersTill ct on ct.cashiersTillID=lr.cashierTillID
	inner join dbo.users u on lower(ltrim(rtrim(ct.userName)))  = lower(ltrim(rtrim(u.user_name))) 
	left join ln.agent a on a.agentId = l.agentId
	left join fa.staff s on s.staffId = l.staffId
	left join ln.loanGroupClient lgc on c.clientID = lgc.clientID
	left join ln.loanGroup lg on lgc.loanGroupId = lg.loanGroupId
union all
select
	lr.amount amountPaid,
	0.0 interestPaid,
	lr.amount principalPaid,
	'Group Susu Contribution' as repaymentTypeName,
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.susuAccountId as loanID,
	l.susuAccountNo as loanNo,
	0.0 penaltyPaid,
	0.0 feePaid,
	lr.contributionDate repaymentDate,
	isnull(isnull(lr.staffId, lr.agentId), 0) as cashierTillID,
	isnull(isnull(u.[user_name], a.surname), '') as userName,
	isnull(isnull(isnull(u.full_name, s.surname+', ' + s.otherNames), a.surname+', ' + a.otherNames), ''),
	isnull(isnull(a.surname+', ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName,
	lr.posted,
	isnull(lg.loanGroupName,'') as loanGroupName,
	isnull(lg.loanGroupNumber,'') as loanGroupNumber
from ln.susuContribution lr
	inner join ln.susuAccount l on lr.susuAccountId=l.susuAccountId 
	left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID 
	left join fa.staff s on s.staffId = lr.staffId
	left join ln.agent a on a.agentId = lr.agentId
	left join dbo.users u on lower(ltrim(rtrim(s.userName)))  = lower(ltrim(rtrim(u.user_name)))	
	left join ln.loanGroupClient lgc on c.clientID = lgc.clientID
	left join ln.loanGroup lg on lgc.loanGroupId = lg.loanGroupId
union all
select
	lr.amount amountPaid,
	0.0 interestPaid,
	lr.amount principalPaid,
	'Normal Susu Contribution' as repaymentTypeName,
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.regularSusuAccountId as loanID,
	l.regularSusuAccountNo as loanNo,
	0.0 penaltyPaid,
	0.0 feePaid,
	lr.contributionDate repaymentDate,
	isnull(isnull(lr.staffId, lr.agentId), 0) as cashierTillID,
	isnull(isnull(u.[user_name], a.surname), '') as userName,
	isnull(isnull(isnull(u.full_name, s.surname+', ' + s.otherNames), a.surname+', ' + a.otherNames), ''),
	isnull(isnull(a.surname+', ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName,
	lr.posted,
	isnull(lg.loanGroupName,'') as loanGroupName,
	isnull(lg.loanGroupNumber,'') as loanGroupNumber
from ln.regularSusuContribution lr
	inner join ln.regularSusuAccount l on lr.regularSusuAccountId=l.regularSusuAccountId 
	left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID 
	left join fa.staff s on s.staffId = lr.staffId
	left join ln.agent a on a.agentId = lr.agentId
	left join dbo.users u on lower(ltrim(rtrim(s.userName)))  = lower(ltrim(rtrim(u.user_name))) 	
	left join ln.loanGroupClient lgc on c.clientID = lgc.clientID
	left join ln.loanGroup lg on lgc.loanGroupId = lg.loanGroupId
	
	union all
select
	lr.chargeAmount amountPaid,
	0.0 interestPaid,
	0.0 principalPaid,
	'Client Service Charge' as repaymentTypeName,
	'Cash' as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	lr.clientServiceChargeId as loanID,
	c.accountNumber as loanNo,
	0.0 penaltyPaid,
	lr.chargeAmount feePaid,
	lr.chargeDate repaymentDate,
	0 as cashierTillID,
	isnull(isnull(u.[user_name], lr.creator), '') as userName,
	isnull(u.full_name, ''),
	'' as agentName,
	lr.posted,
	isnull(lg.loanGroupName,'') as loanGroupName,
	isnull(lg.loanGroupNumber,'') as loanGroupNumber
from ln.clientServiceCharge lr
	inner join ln.client c on lr.clientID=c.clientID 
	left join dbo.users u on lower(ltrim(rtrim(lr.creator)))  = lower(ltrim(rtrim(u.user_name))) 	
	left join ln.loanGroupClient lgc on c.clientID = lgc.clientID
	left join ln.loanGroup lg on lgc.loanGroupId = lg.loanGroupId
	) t