use coreDB
go

alter view ln.vwCashierDisb
with encryption as
select   
	isnull(mp.modeOfPaymentName,'') as modeOfPaymentName,
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	lr.amount amountDisbursed,
	lr.txDate disbursementDate,
	isnull(l.amountApproved,0) as loanAmount,
	ct.userName,
	u.full_name,
	lr.posted,
	isnull(c.companyId,0)as CompanyId,
    isnull(company.comp_Name, '') AS comp_Name,
	isnull(a.surname+', ' + a.otherNames, '') as agentName
from ln.cashierDisbursement lr
	inner join ln.loan l on lr.loanID=l.loanID 
	left outer join ln.modeOfPayment mp on lr.paymentModeID = mp.modeOfPaymentID
	inner join ln.client c on l.clientID=c.clientID
	inner join ln.cashiersTill ct on ct.cashiersTillID=lr.cashierTillID
	inner join dbo.users u on lower(ltrim(rtrim(ct.userName)))  = lower(ltrim(rtrim(u.user_name)))
	left join ln.agent a on a.agentId = l.agentId
	left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	go
