use coreDB
go

create view ln.vwSusuContributionReport
with encryption
as
select
		sa.susuAccountID, c.clientID, ISNULL(CASE WHEN (c.clientTypeID = 3 OR
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
		isnull(commissionAmount, 0) as commissionAmount,
		isnull(dueDate, '2000-01-01') as dueDate,
		isnull(susuGroupNo, 0) as susuGroupNo,
		isnull(susuGroupName, '') as susuGroupName,
		ISNULL(susuContributionId, 0) as susuContributionId,
		isnull(amount, 0) as contributionAmount,
		isnull(contributionDate, getDate()) contributionDate,
		isnull(sc.modeOfPaymentId, 0) modeOfPaymentId,
		isnull(modeOfPaymentName, '') modeOfPaymentName,
		isnull(sc.checkNo, '') checkNo,
		isnull(narration, '') narration,
		isnull(s.surName + ', ' + s.otherNames, '') as staffName,
		isnull(a.surName + ', ' + a.otherNames, '') as agentName,
		isnull(a.agentId, 0) as agentId,
		isnull(s.staffId, 0) as staffId
from ln.susuAccount sa
	 inner join ln.client c on sa.clientId = c.clientId
     inner JOIN ln.susuGrade AS sg ON sa.susuGradeID = sg.susuGradeID 
	 INNER JOIN ln.susuPosition AS sp ON sa.susuPositionID = sp.susuPositionID
	 left outer join ln.susuGroup su on sa.susuGroupID = su.susuGroupID
	 inner join ln.susuContribution sc on sa.susuAccountId = sc.susuAccountId
	 inner join ln.modeOfPayment mp on mp.modeOfPaymentId = sc.modeOfPaymentId
	 left join ln.agent a on sc.agentId = a.agentId
	 left join fa.staff s on sc.staffId = s.staffId
go

create view ln.vwRegularSusuContributionReport
with encryption
as
select
		sa.regularSusuAccountID, c.clientID, ISNULL(CASE WHEN (c.clientTypeID = 3 OR
		c.clientTypeID = 4 OR
		c.clientTypeID = 5) THEN c.companyName WHEN (c.clientTypeID = 6) THEN c.accountName ELSE c.surName + ', ' + c.otherNames END, '') AS clientName, 
		ISNULL(c.accountNumber, '') AS accountNumber, 
		isnull(regularSusuAccountNo, '') as regularSusuAccountNo,
		ISNULL(sa.applicationDate, '2000-01-01') AS applicationDate, 
		ISNULL(ISNULL(sa.startDate, sa.applicationDate), '2000-01-01') AS startDate, 
		ISNULL(sa.amountEntitled, 0) AS amountEntitled, 
		ISNULL(sa.interestAmount, 0) AS interestAmount, ISNULL(sa.entitledDate, '2000-01-01') AS entitledDate, ISNULL(sa.netAmountEntitled, 0) AS netAmountEntitled, 
		isnull(regularSusCommissionAmount, 0) as commissionAmount,
		isnull(dueDate, '2000-01-01') as dueDate,
		isnull(susuGroupNo, 0) as regularSusuGroupNo,
		isnull(susuGroupName, '') as regularSusuGroupName,
		ISNULL(regularSusuContributionId, 0) as regularSusuContributionId,
		isnull(amount, 0) as contributionAmount,
		isnull(contributionDate, getDate()) contributionDate,
		isnull(sc.modeOfPaymentId, 0) modeOfPaymentId,
		isnull(modeOfPaymentName, '') modeOfPaymentName,
		isnull(sc.checkNo, '') checkNo,
		isnull(narration, '') narration,
		isnull(s.surName + ', ' + s.otherNames, '') as staffName,
		isnull(a.surName + ', ' + a.otherNames, '') as agentName,
		isnull(a.agentId, 0) as agentId,
		isnull(s.staffId, 0) as staffId
from ln.regularSusuAccount sa
	 inner join ln.client c on sa.clientId = c.clientId
	 left outer join ln.susuGroup su on sa.regularSusuGroupID = su.susuGroupID
	 inner join ln.regularSusuContribution sc on sa.regularSusuAccountId = sc.regularSusuAccountId
	 inner join ln.modeOfPayment mp on mp.modeOfPaymentId = sc.modeOfPaymentId
	 left join ln.agent a on sc.agentId = a.agentId
	 left join fa.staff s on sc.staffId = s.staffId
go
