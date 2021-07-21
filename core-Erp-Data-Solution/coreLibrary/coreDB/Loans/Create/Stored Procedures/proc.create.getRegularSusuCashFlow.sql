use coreDB
go

create procedure ln.getRegularSusuCashFlow
(
	@date datetime
)
with encryption
as
begin
	SELECT isnull(sa.regularSusuAccountID, 0) regularSusuAccountID, c.clientID, ISNULL(CASE WHEN (c.clientTypeID = 3 OR
		c.clientTypeID = 4 OR
		c.clientTypeID = 5) THEN c.companyName WHEN (c.clientTypeID = 6) THEN c.accountName ELSE c.surName + ', ' + c.otherNames END, '') AS clientName, 
		ISNULL(c.accountNumber, '') AS accountNumber, 
		isnull(regularSusuAccountNo, '') as regularSusuAccountNo,
		ISNULL(sa.applicationDate, '2000-01-01') AS applicationDate, 
		ISNULL(sa.startDate, '2000-01-01') AS startDate, 
		ISNULL(sa.amountEntitled, 0) AS amountEntitled, 
		ISNULL(sa.interestAmount, 0) AS interestAmount, ISNULL(sa.entitledDate, '2000-01-01') AS entitledDate, ISNULL(sa.netAmountEntitled, 0) AS netAmountEntitled, 
		ISNULL(ln.susuAccountStatus(sa.regularSusuAccountID, @date), 0) AS statusID, 
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.regularSusuContribution AS sc
			WHERE        (sa.regularSusuAccountID = regularSusuAccountID) AND (contributionDate <= @date)), 0.0) AS contributionsMade, sa.contributionAmount, 
		ISNULL(sa.contributionAmount * conf.regularSusuPeriodsInCycle * conf.regularSusuDaysInPeriod, 0.0) AS contributionsExpected,
		ISNULL((SELECT        SUM(sc.balance) AS Expr1
			FROM            ln.regularSusuContributionSchedule AS sc
			WHERE        (sa.regularSusuAccountID = sc.regularSusuAccountID) AND (sc.plannedContributionDate <= @date)), 0.0) AS contributionsDefaulted,
		isnull(regularSusCommissionAmount, 0) as regularSusCommissionAmount,
		isnull(dueDate, '2000-01-01') as dueDate,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.regularSusuContribution AS sc
			WHERE        (sa.regularSusuAccountID = regularSusuAccountID)), 0.0) as allContributions,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.regularSusuContributionSchedule AS sc
			WHERE        (sa.regularSusuAccountID = regularSusuAccountID)), 0.0) as allExpected,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.regularSusuContribution AS sc
			WHERE        (sa.regularSusuAccountID = regularSusuAccountID) and contributionDate <= @date), 0.0) as periodContributions,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.regularSusuContributionSchedule AS sc
			WHERE        (sa.regularSusuAccountID = regularSusuAccountID) and plannedContributionDate <= @date), 0.0) as periodExpected, 
		ISNULL(sa.disbursementDate, '2000-01-01') AS disbursementDate,
		ISNULL(cast((
			ISNULL((SELECT        SUM(amount) AS Expr1
				FROM            ln.regularSusuContributionSchedule AS sc
				WHERE        (sa.regularSusuAccountID = sc.regularSusuAccountID) and plannedContributionDate <= @date), 0.0)
			-
			ISNULL((SELECT        SUM(amount) AS Expr1
				FROM            ln.regularSusuContribution AS sc
				WHERE        (sa.regularSusuAccountID = sc.regularSusuAccountID) and contributionDate <= @date), 0.0)
		) / sa.contributionAmount as int), 0) as daysDelayed,
		isnull([month], '2000-01-01') as month,
		isnull(amountReceivable, 0) as amountReceivable,
		isnull(amountPaid, 0) as amountPaid,
		0.0 as profitLoss
	FROM ln.regularSusuAccount AS sa INNER JOIN
        ln.client AS c ON sa.clientID = c.clientID 
		cross join susuConfig conf  
		left join 
		(
			select
				isnull(a.regularSusuAccountID, b.regularSusuAccountID) as regularSusuAccountID,
				isnull(a.month, b.month) as [month],
				isnull(amountReceivable, 0) as amountReceivable,
				isnull(amountPaid, 0) as amountPaid
			from
			(
				SELECT  DATEADD(dd, - (DAY(plannedContributionDate) - 1), plannedContributionDate) AS month,  
						regularSusuAccountID, SUM(amount) AS amountReceivable 
				FROM            ln.regularSusuContributionSchedule
				GROUP BY DATEADD(dd, - (DAY(plannedContributionDate) - 1), plannedContributionDate),  
						regularSusuAccountID
			)  a 
			full outer join 
			(
				SELECT   DATEADD(dd, - (DAY(contributionDate) - 1), contributionDate) AS month, 
						DATEADD(dd,-(DAY(DATEADD(mm,1,contributionDate))),DATEADD(mm,1,contributionDate)) as eom,
						regularSusuAccountID, SUM(amount) AS amountPaid
				FROM ln.regularSusuContribution
				GROUP BY  DATEADD(dd, - (DAY(contributionDate) - 1), contributionDate) ,
						DATEADD(dd,-(DAY(DATEADD(mm,1,contributionDate))),DATEADD(mm,1,contributionDate)),
						 regularSusuAccountID
			) b on a.regularSusuAccountID=b.regularSusuAccountID and a.month=b.month 
		) b on sa.regularSusuAccountID = b.regularSusuAccountID
end
go
