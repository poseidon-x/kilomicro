
use coreDB
go

create view ln.vwSavingAdditionalWithClientGroup
with encryption 
as
SELECT d.SavingID, d.clientID, case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then 
		c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end AS clientName, 
	w.savingDate, c.accountNumber, b.branchName, isnull(w.creation_date, getDate()) creation_date, w.creator, 
	w.savingAmount, w.SavingAdditionalID, t.SavingTypeName, 
	t.SavingTypeID, d.interestRate, d.principalBalance, d.interestBalance, d.interestExpected,
	isnull(modeOfPaymentName,'') as modeOfPaymentName, savingNo,
	isnull(naration, '') as naration,
	w.posted,
	isnull(a.surname+', ' + a.otherNames, '') as agentName,
	isnull(lg.loanGroupName,'No Group') as loanGroupName
FROM ln.Saving AS d INNER JOIN
	ln.client AS c ON d.clientID = c.clientID INNER JOIN
	ln.SavingAdditional AS w ON d.SavingID = w.SavingID INNER JOIN
	ln.branch AS b ON c.branchID = b.branchID INNER JOIN
	ln.SavingType AS t ON d.SavingTypeID = t.SavingTypeID
	left join ln.loanGroupClient as lgc on lgc.clientId = c.clientID
	left join ln.loanGroup as lg on lg.loanGroupId = lgc.loanGroupId
	left join ln.modeOfPayment mp on mp.modeOfPaymentID = w.modeOfpaymentID
	left join ln.agent a on a.agentId = d.agentId
go

