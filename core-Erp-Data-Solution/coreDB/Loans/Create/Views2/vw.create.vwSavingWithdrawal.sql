use coreDB
go

alter view ln.vwSavingWithdrawal
with encryption 
as
SELECT        d.SavingID, d.clientID, case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end AS clientName, w.withdrawalDate, c.accountNumber, b.branchName, isnull(w.creation_date, getDate()) creation_date, w.creator, 
                         w.interestWithdrawal + w.principalWithdrawal AS amountWithdrawn, w.SavingWithdrawalID, w.interestWithdrawal, w.principalWithdrawal, t.SavingTypeName, 
                         t.SavingTypeID, d.interestRate, d.principalBalance, d.interestBalance, d.interestExpected,
						 isnull(modeOfPaymentName,'') as modeOfPaymentName, savingNo,
						 isnull(naration, '') as naration,
			w.posted,
	isnull(a.surname+', ' + a.otherNames, '') as agentName
FROM            ln.Saving AS d INNER JOIN
                         ln.client AS c ON d.clientID = c.clientID INNER JOIN
                         ln.SavingWithdrawal AS w ON d.SavingID = w.SavingID INNER JOIN
                         ln.branch AS b ON c.branchID = b.branchID INNER JOIN
                         ln.SavingType AS t ON d.SavingTypeID = t.SavingTypeID
						 left join ln.modeOfPayment mp on mp.modeOfPaymentID = w.modeOfpaymentID
	left join ln.agent a on a.agentId = d.agentId
go

