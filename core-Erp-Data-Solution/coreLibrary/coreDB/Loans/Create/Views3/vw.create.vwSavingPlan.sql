
use coreDB
go

alter view ln.vwSavingPlan
with encryption 
as
SELECT d.SavingID, d.clientID, c.clientName, 	 
	[accountNumber],
	[addressLine1],
	[directions],
	[cityTown],
	[workPhone],
	[mobilePhone],
	[homePhone],
	[officeEmail],
	[personalEmail],
	[image],
	[DOB],
	t.SavingTypeID, d.interestRate, d.principalBalance, d.interestBalance, d.interestExpected, savingNo,
	w.savingPlanID,
	plannedDate,
	plannedAmount,
	deposited,
	amountDeposited, 
	savingTypeName,
	isnull((select top 1 savingAmount from ln.savingAdditional sa where sa.savingId = d.savingId order by savingDate)
		, 0.0) as initialDeposit
FROM ln.Saving AS d INNER JOIN
	ln.vwClients AS c ON d.clientID = c.clientID INNER JOIN
	ln.savingPlan AS w ON d.SavingID = w.SavingID INNER JOIN
	ln.branch AS b ON c.branchID = b.branchID INNER JOIN
	ln.SavingType AS t ON d.SavingTypeID = t.SavingTypeID
go
