use coreDB
go

alter view ln.vwProductSummary
with encryption
as
select
	lt.loanTypeName as productName,
	'Loan Accounts' as productCategory,
	count(l.loanId) as allAccounts,
	count(distinct c.clientId) as distinctAccount, 
	count(distinct case when l.balance>9.9 then l.loanId else null end) as allActiveAccounts,
	count(distinct case when l.balance>9.9 then c.clientId else null end) as distinctActiveAccounts, 
	sum(amountDisbursed) as totalPrincipal,
	sum(totalInterest) as totalInterest,
	SUM(remainingBalance) remainingBalance,
	SUM(l.balance) principalBalance
from ln.loan l 
	inner join ln.client c on l.clientId = c.clientId
	inner join ln.loanType lt on l.loanTypeId = lt.loanTypeId
	inner join (
		select 
			loanId, 
			sum(case when sortCode in (3) then Dr else 0 end) as totalInterest,
			SUM(Dr-Cr) remainingBalance
		from ln.vwLoanStatement
		group by loanId
	) vls on l.loanId = vls.loanId
group by lt.loanTypeName
union all
select
	lt.savingTypeName as productName,
	'Regular Deposit Accounts' as productCategory,
	count(l.savingId) as allAccounts,
	count(distinct c.clientId) as distinctAccount, 
	count(distinct case when l.principalbalance>9.9 then l.savingId else null end) as allActiveAccounts,
	count(distinct case when l.principalbalance>9.9 then c.clientId else null end) as distinctActiveAccounts, 
	sum(l.amountInvested) as totalPrincipal,
	sum(l.interestAccumulated) as totalInterest,
	SUM(l.principalBalance+l.interestBalance) remainingBalance,
	SUM(l.principalBalance) principalBalance
from ln.saving l 
	inner join ln.client c on l.clientId = c.clientId
	inner join ln.savingType lt on l.savingTypeId = lt.savingTypeId
group by lt.savingTypeName
union all
select
	lt.investmentTypeName as productName,
	'Company Investment Accounts (Outgoing)' as productCategory,
	count(l.investmentId) as allAccounts,
	count(distinct c.clientId) as distinctAccount, 
	count(distinct case when l.principalbalance>9.9 then l.investmentId else null end) as allActiveAccounts,
	count(distinct case when l.principalbalance>9.9 then c.clientId else null end) as distinctActiveAccounts, 
	sum(l.amountInvested) as totalPrincipal,
	sum(l.interestAccumulated) as totalInterest,
	SUM(l.principalBalance+l.interestBalance) remainingBalance,
	SUM(l.principalBalance) principalBalance
from ln.investment l 
	inner join ln.client c on l.clientId = c.clientId
	inner join ln.investmentType lt on l.investmentTypeId = lt.investmentTypeId
group by lt.investmentTypeName
union all
select
	lt.depositTypeName as productName,
	'Client Investment Accounts (Incoming)' as productCategory,
	count(l.depositId) as allAccounts,
	count(distinct c.clientId) as distinctAccount, 
	count(distinct case when l.principalbalance>9.9 then l.depositId else null end) as allActiveAccounts,
	count(distinct case when l.principalbalance>9.9 then c.clientId else null end) as distinctActiveAccounts, 
	sum(l.amountInvested) as totalPrincipal,
	sum(l.interestAccumulated) as totalInterest,
	SUM(l.principalBalance+l.interestBalance) remainingBalance,
	SUM(l.principalBalance) principalBalance
from ln.deposit l 
	inner join ln.client c on l.clientId = c.clientId
	inner join ln.depositType lt on l.depositTypeId = lt.depositTypeId
group by lt.depositTypeName
union all
select
	lt.susuGradeName as productName,
	'Group Susu Accounts' as productCategory,
	count(l.susuAccountId) as allAccounts,
	count(distinct c.clientId) as distinctAccount, 
	count(distinct case when l.amountEntitled - isnull(sc.amount, 0) > 9.9 then l.susuAccountId else null end) as allActiveAccounts,
	count(distinct case when l.amountEntitled - isnull(sc.amount, 0) > 9.9 then c.clientId else null end) as distinctActiveAccounts, 
	sum(l.amountEntitled) as totalPrincipal,
	sum(l.interestAmount) as totalInterest,
	SUM(l.amountEntitled - isnull(sc.amount, 0)) remainingBalance,
	SUM(l.amountEntitled - isnull(sc.amount, 0)) principalBalance
from ln.susuAccount l 
	inner join ln.client c on l.clientId = c.clientId
	inner join ln.susuGrade lt on l.susuGradeId = lt.susuGradeId
	left outer join 
	(
		select susuAccountId,
			sum(amount) as amount
		from ln.susuContribution 
		group by susuAccountId
	) sc on l.susuAccountId = sc.susuAccountId
group by lt.susuGradeName
union all
select
	'Normal Susu' as productName,
	'Normal Susu Accounts' as productCategory,
	count(l.regularSusuAccountId) as allAccounts,
	count(distinct c.clientId) as distinctAccount, 
	count(distinct case when isnull(sc.amount, 0)-isnull(sw.amount, 0)>9.9 then l.regularSusuAccountId else null end) as allActiveAccounts,
	count(distinct case when isnull(sc.amount, 0)-isnull(sw.amount, 0)>9.9 then c.clientId else null end) as distinctActiveAccounts, 
	sum(isnull(sc.amount, 0)) as totalPrincipal,
	sum(0) as totalInterest,
	SUM(isnull(sc.amount, 0)-isnull(sw.amount, 0)) remainingBalance,
	SUM(isnull(sc.amount, 0)-isnull(sw.amount, 0)) principalBalance
from ln.regularSusuAccount l 
	inner join ln.client c on l.clientId = c.clientId 
	left outer join 
	(
		select regularSusuAccountId,
			sum(isnull(amount, 0)) as amount
		from ln.regularSusuContribution 
		group by regularSusuAccountId
	) sc on l.regularSusuAccountId = sc.regularSusuAccountId 
	left outer join 
	(
		select regularSusuAccountId,
			sum(isnull(amount, 0)) as amount
		from ln.regularSusuWithdrawal 
		group by regularSusuAccountId
	) sw on l.regularSusuAccountId = sc.regularSusuAccountId 

go

