use coreDB
go

alter procedure ln.getRegularSusuAccountSchedule
(
	@startDate datetime,
	@date datetime,
	@clientId int = null
)
with encryption
as
begin
	SELECT sa.regularSusuAccountID, c.clientID, ISNULL(CASE WHEN (c.clientTypeID = 3 OR
		c.clientTypeID = 4 OR
		c.clientTypeID = 5) THEN c.companyName WHEN (c.clientTypeID = 6) THEN c.accountName ELSE c.surName + ', ' + c.otherNames END, '') AS clientName, 
		ISNULL(c.accountNumber, '') AS accountNumber, 
		isnull(regularSusuAccountNo, '') as regularSusuAccountNo,
		ISNULL(sa.applicationDate, '2000-01-01') AS applicationDate, 
		ISNULL(ISNULL(sa.startDate, sa.applicationDate), '2000-01-01') AS startDate, 
		ISNULL(sa.amountEntitled, 0) AS amountEntitled, 
		ISNULL(sa.interestAmount, 0) AS interestAmount, ISNULL(sa.entitledDate, '2000-01-01') AS entitledDate, ISNULL(sa.netAmountEntitled, 0) AS netAmountEntitled, 
		ISNULL(ln.regularSusuAccountStatus(sa.regularSusuAccountID, 
		@date), 0) AS statusID, ISNULL
		((SELECT        SUM(amount) AS Expr1
			FROM            ln.regularSusuContribution AS sc
			WHERE        (sa.regularSusuAccountID = sc.regularSusuAccountID) AND (contributionDate <= @date)), 0.0) AS contributionsMade, sa.contributionAmount, 
		ISNULL(sa.contributionAmount * conf.regularSusuPeriodsInCycle * conf.regularSusuDaysInPeriod, 0.0) AS contributionsExpected,
		ISNULL((SELECT        SUM(sc.balance) AS Expr1
			FROM            ln.regularSusuContributionSchedule AS sc
			WHERE        (sa.regularSusuAccountID = sc.regularSusuAccountID) AND (sc.plannedContributionDate <= @date)), 0.0) AS contributionsDefaulted,
		isnull(regularSusCommissionAmount, 0) as regularSusCommissionAmount,
		isnull (scs.plannedContributionDate, '2000-01-01') plannedContributionDate,
		isnull(scs.balance	, 0) as balance,
		isnull(scs.actualContributionDate, '2000-01-01') as actualContributionDate,
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
		) / sa.contributionAmount as int), 0) as daysDelayed
	FROM ln.regularSusuAccount AS sa INNER JOIN
		ln.client AS c ON sa.clientID = c.clientID inner join
		ln.regularSusuContributionSchedule scs on sa.regularSusuAccountID = scs.regularSusuAccountID 
		cross join susuConfig conf 
	where scs.balance>0 or plannedContributionDate <= @date
		and ( @clientId is null or c.clientId=@clientId)
end
go
