use coreDB
go

create view ln.vwSusuDisbursement
with encryption
as
SELECT isnull(c.clientName, '') clientName, isnull(c.clientID, 0) clientID, 
		isnull(c.accountNumber, '') as accountNumber, isnull(sa.disbursementDate, getDate()) disbursementDate, 
		isnull(case when sa.amountTaken> 0 then amountTaken else netAmountEntitled end, 0) amount, isnull(sa.disbursedBy, '') cashierUsername, 
		isnull(sa.susuAccountNo, '') susuAccountNo,
		isnull(mp.modeOfPaymentID, 0) modeOfPaymentID,
		isnull(mp.modeOfPaymentName, '') modeOfPaymentName
FROM   ln.susuAccount AS sa INNER JOIN
	ln.vwClients AS c ON sa.clientID = c.clientID
	inner join ln.modeOfPayment mp on mp.modeOfPaymentID = sa.modeOfPaymentID
where disbursementDate is not null

go

create view ln.vwregularSusuDisbursement
with encryption
as
SELECT isnull(c.clientName, '') clientName, isnull(c.clientID, 0) clientID, 
		isnull(c.accountNumber, '') as accountNumber, isnull(sa.disbursementDate, getDate()) disbursementDate, 
		isnull(case when sa.amountTaken> 0 then amountTaken else netAmountEntitled end, 0) amount, isnull(sa.disbursedBy, '') cashierUsername, 
		isnull(sa.regularSusuAccountNo, '') regularSusuAccountNo,
		isnull(mp.modeOfPaymentID, 0) modeOfPaymentID,
		isnull(mp.modeOfPaymentName, '') modeOfPaymentName
FROM   ln.regularSusuAccount AS sa INNER JOIN
	ln.vwClients AS c ON sa.clientID = c.clientID
	inner join ln.modeOfPayment mp on mp.modeOfPaymentID = sa.modeOfPaymentID
where disbursementDate is not null

go
