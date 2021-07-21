use coreDB
go

alter procedure ln.getSusuCashFlow
(
	@date datetime
)
with encryption
as
begin
	SELECT isnull(sa.susuAccountID, 0) susuAccountID, c.clientID, ISNULL(CASE WHEN (c.clientTypeID = 3 OR
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
		ISNULL(ln.susuAccountStatus(sa.susuAccountID, @date), 0) AS statusID, 
		ISNULL((SELECT        SUM(amount) AS Expr1
			FROM            ln.susuContribution AS sc
			WHERE        (sa.susuAccountID = susuAccountID) AND (contributionDate <= @date)), 0.0) AS contributionsMade, sa.contributionAmount, 
		ISNULL(sa.contributionAmount * sp.noOfWaitingPeriods * conf.daysInPeriod, 0.0) AS contributionsExpected,
		ISNULL((SELECT        SUM(sc.balance) AS Expr1
			FROM            ln.susuContributionSchedule AS sc
			WHERE        (sa.susuAccountID = sc.susuAccountID) AND (sc.plannedContributionDate <= @date)), 0.0) AS contributionsDefaulted,
		isnull(commissionAmount, 0) as commissionAmount,
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
		) / sa.contributionAmount as int), 0) as daysDelayed,
		isnull([month], '2000-01-01') as month,
		isnull(amountReceivable, 0) as amountReceivable,
		isnull(amountPaid, 0) as amountPaid,
		isnull(case when dateadd(dd, (conf.daysInPeriod+5) *sp.noOfWaitingPeriods, startDate) > [month]
			and conf.periodsInCycle > sp.noOfWaitingPeriods and [month] between startDate and dueDate then
			interestAmount/(conf.periodsInCycle-sp.noOfWaitingPeriods)
		else 0 end, 0) as profitLoss
	FROM ln.susuAccount AS sa INNER JOIN
        ln.susuGrade AS sg ON sa.susuGradeID = sg.susuGradeID INNER JOIN
        ln.susuPosition AS sp ON sa.susuPositionID = sp.susuPositionID INNER JOIN
        ln.client AS c ON sa.clientID = c.clientID left outer join
		ln.susuGroup su on sa.susuGroupID = su.susuGroupID
		cross join susuConfig conf  
		left join 
		(
			select
				isnull(a.susuAccountID, b.susuAccountID) as susuAccountID,
				isnull(a.month, b.month) as [month],
				isnull(amountReceivable, 0) as amountReceivable,
				isnull(amountPaid, 0) as amountPaid
			from
			(
				SELECT  DATEADD(dd, - (DAY(plannedContributionDate) - 1), plannedContributionDate) AS month,  
						susuAccountID, SUM(amount) AS amountReceivable 
				FROM            ln.susuContributionSchedule
				GROUP BY DATEADD(dd, - (DAY(plannedContributionDate) - 1), plannedContributionDate),  
						susuAccountID
			)  a 
			full outer join 
			(
				SELECT   DATEADD(dd, - (DAY(contributionDate) - 1), contributionDate) AS month, 
						DATEADD(dd,-(DAY(DATEADD(mm,1,contributionDate))),DATEADD(mm,1,contributionDate)) as eom,
						susuAccountID, SUM(amount) AS amountPaid
				FROM ln.susuContribution
				GROUP BY  DATEADD(dd, - (DAY(contributionDate) - 1), contributionDate) ,
						DATEADD(dd,-(DAY(DATEADD(mm,1,contributionDate))),DATEADD(mm,1,contributionDate)),
						 susuAccountID
			) b on a.susuAccountID=b.susuAccountID and a.month=b.month 
		) b on sa.susuAccountID = b.susuAccountID
end
go
