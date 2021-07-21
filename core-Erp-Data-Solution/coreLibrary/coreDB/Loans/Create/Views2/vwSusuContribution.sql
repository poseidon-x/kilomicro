use coreDB
go

create view ln.vwSusuContribution
with encryption
as
SELECT isnull(c.clientName, '') clientName, isnull(c.clientID, 0) clientID, 
		isnull(c.accountNumber, '') as accountNumber, isnull(sc.contributionDate, getDate()) contributionDate, 
		isnull(sc.amount, 0) amount, isnull(sc.cashierUsername, '') cashierUsername, 
		isnull(sa.susuAccountNo, '') susuAccountNo,
		isnull(mp.modeOfPaymentID, 0) modeOfPaymentID,
		isnull(mp.modeOfPaymentName, '') modeOfPaymentName
FROM  ln.susuContribution AS sc INNER JOIN
	ln.susuAccount AS sa ON sc.susuAccountID = sa.susuAccountID INNER JOIN
	ln.vwClients AS c ON sa.clientID = c.clientID
	inner join ln.modeOfPayment mp on mp.modeOfPaymentID = sc.modeOfPaymentID

go

create view ln.vwregularSusuContribution
with encryption
as
SELECT isnull(c.clientName, '') clientName, isnull(c.clientID, 0) clientID, 
		isnull(c.accountNumber, '') as accountNumber, isnull(sc.contributionDate, getDate()) contributionDate, 
		isnull(sc.amount, 0) amount, isnull(sc.cashierUsername, '') cashierUsername, 
		isnull(sa.regularSusuAccountNo, '') regularSusuAccountNo,
		isnull(mp.modeOfPaymentID, 0) modeOfPaymentID,
		isnull(mp.modeOfPaymentName, '') modeOfPaymentName
FROM  ln.regularSusuContribution AS sc INNER JOIN
	ln.regularSusuAccount AS sa ON sc.regularSusuAccountID = sa.regularSusuAccountID INNER JOIN
	ln.vwClients AS c ON sa.clientID = c.clientID
	inner join ln.modeOfPayment mp on mp.modeOfPaymentID = sc.modeOfPaymentID

go
