use coreDB
go

alter view ln.vwDepositWithdrawal
with encryption 
as
SELECT        d.depositID, d.clientID, case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end AS clientName, w.withdrawalDate, c.accountNumber, b.branchName, isnull(w.creation_date, getDate()) creation_date, w.creator, 
                         w.interestWithdrawal + w.principalWithdrawal AS amountWithdrawn, w.depositWithdrawalID, w.interestWithdrawal, w.principalWithdrawal, t.depositTypeName, 
                         t.depositTypeID, d.interestRate, d.principalBalance, d.interestBalance, d.interestExpected,
						 isnull(modeOfPaymentName,'') as modeOfPaymentName, depositNo,
						 isnull(naration, '') as naration,
						 w.posted,
	isnull(a.surname+', ' + a.otherNames, '') as agentName
FROM            ln.deposit AS d INNER JOIN
                         ln.client AS c ON d.clientID = c.clientID INNER JOIN
                         ln.depositWithdrawal AS w ON d.depositID = w.depositID INNER JOIN
                         ln.branch AS b ON c.branchID = b.branchID INNER JOIN
                         ln.depositType AS t ON d.depositTypeID = t.depositTypeID
						 left join ln.modeOfPayment mp on mp.modeOfPaymentID = w.modeOfpaymentID
	left join ln.agent a on a.agentId = d.agentId 
go


