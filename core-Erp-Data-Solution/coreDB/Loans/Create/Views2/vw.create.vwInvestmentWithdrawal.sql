use coreDB
go

create view ln.vwInvestmentWithdrawal
with encryption 
as
SELECT        d.investmentID, d.clientID, case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end AS clientName, w.withdrawalDate, c.accountNumber, b.branchName, isnull(w.creation_date, getDate()) creation_date, w.creator, 
                         w.interestWithdrawal + w.principalWithdrawal AS amountWithdrawn, w.investmentWithdrawalID, w.interestWithdrawal, w.principalWithdrawal, t.investmentTypeName, 
                         t.investmentTypeID, d.interestRate, d.principalBalance, d.interestBalance, d.interestExpected,
						 isnull(modeOfPaymentName,'') as modeOfPaymentName, investmentNo,
						 isnull(naration, '') as naration,
						 isnull(c.companyId,0)as CompanyId,
                         isnull(company.comp_Name, '') AS comp_Name 
FROM            ln.investment AS d INNER JOIN
                         ln.client AS c ON d.clientID = c.clientID INNER JOIN
                         ln.investmentWithdrawal AS w ON d.investmentID = w.investmentID INNER JOIN
                         ln.branch AS b ON c.branchID = b.branchID INNER JOIN
                         ln.investmentType AS t ON d.investmentTypeID = t.investmentTypeID
						 left join ln.modeOfPayment mp on mp.modeOfPaymentID = w.modeOfpaymentID
						 left join dbo.comp_prof AS company on c.companyId = company.companyId

go
