use coreDB
go

create procedure ln.getLoanCashflow
(
	@date datetime
)
with encryption
as
begin
	select 
		c.clientID,
		isnull(c.branchID, 0) as branchID,
		isnull(branchName, '') as branchname,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		isnull(isnull(b.month, y.month), '2000-01-01') as [month],
		isnull(principalExpected, 0) principalExpected,
		isnull(principalReceived, 0) principalReceived,
		isnull(interestExpected, 0) interestExpected,
		isnull(interestReceived, 0) interestReceived,
		isnull(depositPrincipalPayable, 0) depositPrincipalPayable,
		isnull(depositPrincipalPaid, 0) depositPrincipalPaid,
		isnull(depositInterestPayable, 0) depositInterestPayable,
		isnull(depositInterestPaid, 0) depositInterestPaid,
		isnull(z.amountExpected, 0) groupAmountExpected,
		isnull(z.amountContributed, 0) groupAmountContributed,
		isnull(v.amountExpected, 0) regularAmountExpected,
		isnull(v.amountContributed, 0) regularAmountContributed
	from 
		ln.client c
		inner join ln.branch br on c.branchID = br.branchID
		left join 
		(
			select
				isnull(a.clientID, b.clientID) as clientID,
				isnull(a.month, b.month) as month,
				isnull(principalExpected, 0) principalExpected,
				isnull(principalReceived, 0) principalReceived,
				isnull(interestExpected, 0) interestExpected,
				isnull(interestReceived, 0) interestReceived
			from
			(
				SELECT        DATEADD(dd, - (DAY(repaymentDate) - 1), repaymentDate) AS month, clientID, 
					SUM(principalPayment) AS principalExpected, SUM(interestPayment) AS interestExpected
				FROM            ln.repaymentSchedule AS rs 
					inner join ln.loan l on rs.loanID = l.loanID
				GROUP BY DATEADD(dd, - (DAY(repaymentDate) - 1), repaymentDate), clientID
			) a
			full outer join
			(
				SELECT        DATEADD(dd, - (DAY(repaymentDate) - 1), repaymentDate) AS month, clientID, 
					SUM(principalPaid) AS principalReceived, SUM(interestPaid) AS interestReceived
				FROM            ln.loanRepayment AS rs
					inner join ln.loan l on rs.loanID = l.loanID
				GROUP BY DATEADD(dd, - (DAY(repaymentDate) - 1), repaymentDate), clientID
			) b on a.clientID = b.clientID and a.month=b.month
		) b on c.clientID = b.clientID
		left join
		(
			select
				isnull(a.clientID, b.clientID) as clientID,
				isnull(a.month, b.month) as month,
				depositPrincipalPayable,
				depositPrincipalPaid,
				depositInterestPayable,
				depositInterestPaid
			from
			(
				SELECT        DATEADD(dd, - (DAY(rs.withdrawalDate) - 1), rs.withdrawalDate) AS month, d.clientID, 
					SUM(rs.principalWithdrawal) AS depositPrincipalPaid, SUM(rs.interestWithdrawal) AS depositInterestPaid
				FROM            ln.depositWithdrawal AS rs INNER JOIN
										 ln.deposit AS d ON rs.depositID = rs.depositID
				GROUP BY DATEADD(dd, - (DAY(rs.withdrawalDate) - 1), rs.withdrawalDate), d.clientID
			) a
			full outer join
			(
				SELECT        DATEADD(dd, - (DAY(maturityDate) - 1), maturityDate) AS month, clientID, 
					SUM(amountInvested) AS depositPrincipalPayable, SUM(interestExpected) AS depositInterestPayable
				FROM            ln.deposit AS rs
				GROUP BY DATEADD(dd, - (DAY(maturityDate) - 1), maturityDate), clientID
			) b on a.clientID = b.clientID and a.month=b.month
		) y on c.clientID = y.clientID
		left join
		(
			select
				isnull(a.clientID, b.clientID) as clientID,
				isnull(a.month, b.month) as month,
				amountExpected,
				amountContributed 
			from
			(
				SELECT        DATEADD(dd, - (DAY(rs.contributionDate) - 1), rs.contributionDate) AS month, d.clientID, 
					SUM(amount) AS amountContributed
				FROM            ln.susuContribution AS rs INNER JOIN
										 ln.susuAccount AS d ON rs.susuAccountID = rs.susuAccountID
				GROUP BY DATEADD(dd, - (DAY(rs.contributionDate) - 1), rs.contributionDate), d.clientID
			) a
			full outer join
			(
				SELECT        DATEADD(dd, - (DAY(rs.plannedContributionDate) - 1), rs.plannedContributionDate) AS month, d.clientID, 
					SUM(amount) AS amountExpected
				FROM            ln.susuContributionSchedule AS rs INNER JOIN
										 ln.susuAccount AS d ON rs.susuAccountID = rs.susuAccountID
				GROUP BY DATEADD(dd, - (DAY(rs.plannedContributionDate) - 1), rs.plannedContributionDate), d.clientID
			) b on a.clientID = b.clientID and a.month=b.month
		) z on c.clientID = z.clientID
		left join
		(
			select
				isnull(a.clientID, b.clientID) as clientID,
				isnull(a.month, b.month) as month,
				amountExpected,
				amountContributed 
			from
			(
				SELECT        DATEADD(dd, - (DAY(rs.contributionDate) - 1), rs.contributionDate) AS month, d.clientID, 
					SUM(amount) AS amountContributed
				FROM            ln.regularsusuContribution AS rs INNER JOIN
										 ln.regularsusuAccount AS d ON rs.regularsusuAccountID = rs.regularsusuAccountID
				GROUP BY DATEADD(dd, - (DAY(rs.contributionDate) - 1), rs.contributionDate), d.clientID
			) a
			full outer join
			(
				SELECT        DATEADD(dd, - (DAY(rs.plannedContributionDate) - 1), rs.plannedContributionDate) AS month, d.clientID, 
					SUM(amount) AS amountExpected
				FROM            ln.regularsusuContributionSchedule AS rs INNER JOIN
										 ln.regularsusuAccount AS d ON rs.regularsusuAccountID = rs.regularsusuAccountID
				GROUP BY DATEADD(dd, - (DAY(rs.plannedContributionDate) - 1), rs.plannedContributionDate), d.clientID
			) b on a.clientID = b.clientID and a.month=b.month
		) v on c.clientID = v.clientID
end
go
