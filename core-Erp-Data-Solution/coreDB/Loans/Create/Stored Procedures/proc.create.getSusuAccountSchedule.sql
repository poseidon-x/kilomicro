use coreDB
go

alter procedure ln.getSusuAccountSchedule
(
	@startDate datetime,
	@date datetime,
	@clientId int = null
)
with encryption
as
begin
	SELECT sa.susuAccountID, c.clientID, ISNULL(CASE WHEN (c.clientTypeID = 3 OR
		c.clientTypeID = 4 OR
		c.clientTypeID = 5) THEN c.companyName WHEN (c.clientTypeID = 6) THEN c.accountName ELSE c.surName + ', ' + c.otherNames END, '') AS clientName, 
		ISNULL(c.accountNumber, '') AS accountNumber, 
		isnull(susuAccountNo, '') as susuAccountNo,
		ISNULL(sa.applicationDate, '2000-01-01') AS applicationDate, 
		ISNULL(ISNULL(sa.startDate, sa.applicationDate), '2000-01-01') AS startDate, 
		ISNULL(sg.susuGradeNo, '') AS susuGradeNo, 
		ISNULL(sg.susuGradeName, '') AS susuGradeName, ISNULL(sp.susuPositionNo, 0) AS susuPositionNo, ISNULL(sp.susuPositionName, '') AS susuPositionName, 
		ISNULL(sa.amountEntitled, 0) AS amountEntitled, 
		ISNULL(sa.interestAmount, 0) AS interestAmount, ISNULL(sa.entitledDate, '2000-01-01') AS entitledDate, ISNULL(sa.netAmountEntitled, 0) AS netAmountEntitled, 
		ISNULL(ln.susuAccountStatus(sa.susuAccountID, 
		@date), 0) AS statusID, ISNULL
		((SELECT        SUM(amount) AS Expr1
			FROM            ln.susuContribution AS sc
			WHERE        (sa.susuAccountID = sc.susuAccountID) AND (contributionDate <= @date)), 0.0) AS contributionsMade, sa.contributionAmount, 
		ISNULL(sa.contributionAmount * sp.noOfWaitingPeriods * conf.daysInPeriod, 0.0) AS contributionsExpected,
		ISNULL((SELECT        SUM(sc.balance) AS Expr1
			FROM            ln.susuContributionSchedule AS sc
			WHERE        (sa.susuAccountID = sc.susuAccountID) AND (sc.plannedContributionDate <= @date)), 0.0) AS contributionsDefaulted,
		isnull(commissionAmount, 0) as commissionAmount,
		isnull (scs.plannedContributionDate, '2000-01-01') plannedContributionDate,
		isnull(scs.balance	, 0) as balance,
		isnull(scs.actualContributionDate, '2000-01-01') as actualContributionDate,
		isnull(dueDate, '2000-01-01') as dueDate,
		isnull(susuGroupNo, 0) as susuGroupNo,
		isnull(susuGroupName, '') as susuGroupName, 
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.susuContribution AS sc
			WHERE        (sa.susuAccountID = susuAccountID)), 0.0) as allContributions,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.susuContributionSchedule AS sc
			WHERE        (sa.susuAccountID = susuAccountID)), 0.0) as allExpected,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.susuContribution AS sc
			WHERE        (sa.susuAccountID = susuAccountID) and contributionDate <= @date), 0.0) as periodContributions,
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.susuContributionSchedule AS sc
			WHERE        (sa.susuAccountID = susuAccountID) and plannedContributionDate <= @date), 0.0) as periodExpected, 
		ISNULL(sa.disbursementDate, '2000-01-01') AS disbursementDate,
		ISNULL(cast((
			ISNULL((SELECT        SUM(amount) AS Expr1
				FROM            ln.susuContributionSchedule AS sc
				WHERE        (sa.susuAccountID = sc.susuAccountID) and plannedContributionDate <= @date), 0.0)
			-
			ISNULL((SELECT        SUM(amount) AS Expr1
				FROM            ln.susuContribution AS sc
				WHERE        (sa.susuAccountID = sc.susuAccountID) and contributionDate <= @date), 0.0)
		) / sa.contributionAmount as int), 0) as daysDelayed
	FROM ln.susuAccount AS sa INNER JOIN
        ln.susuGrade AS sg ON sa.susuGradeID = sg.susuGradeID INNER JOIN
        ln.susuPosition AS sp ON sa.susuPositionID = sp.susuPositionID INNER JOIN
        ln.client AS c ON sa.clientID = c.clientID inner join
		ln.susuContributionSchedule scs on sa.susuAccountID = scs.susuAccountID left outer join
		ln.susuGroup su on sa.susuGroupID = su.susuGroupID
		cross join susuConfig conf 
	where scs.balance>0 or plannedContributionDate <= @date
		and ( @clientId is null or c.clientId=@clientId)
end
go
