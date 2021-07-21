use coreDB
go

alter procedure ln.getPublicDepositStats
(
	@date datetime
)
with encryption
as
begin
	declare @tbl table
	(
			clientID int,
			clientName nvarchar(200), 
			gender nvarchar(10),
			balance float,
			accountType nvarchar(100)
	)
	declare @totalCredit float, @paidCapital float

	select @paidCapital = sum(dbo.acc_bal(acct_id, cat_code, '1950-01-01', @date, null))
	from dbo.vw_accounts
	where acct_id not in
	(
		select acct_id
		from dbo.def_accts
		where code in ('RE','CE')
	)
	and cat_code=3

	if @paidCapital<=0
		select @paidCapital=1

	insert into @tbl 
	select
		a.clientID,
		clientName, 
		b.sex,
		sum(dr-cr) as balance,
		'Fixed Deposit' as accountType
	from ln.vwDepositStatement a inner join ln.client b on a.clientId=b.clientId
	where [date] <= @date
	group by 
		a.clientID,
		clientName,
		b.sex 
	having sum(dr-cr)>5
	union all
	select
		a.clientID,
		clientName, 
		b.sex,
		sum(dr-cr) as balance,
		'Savings Account' as accountType
	from ln.vwSavingStatement a inner join ln.client b on a.clientId=b.clientId
	where [date] <= @date
	group by 
		a.clientID,
		clientName,
		b.sex 
	having sum(dr-cr)>5 
	union all
	select
		a.clientID,
		clientName, 
		c.sex,
		isnull((select sum(sc.amount) from ln.susuContribution sc where sc.susuAccountId = a.susuAccountId and contributionDate <= @date), 0) as balance,
		'Group Susu' as accountType
	from ln.susuAccount a inner join ln.vwClients b on a.clientId=b.clientId 
		inner join ln.client c on b.clientId = c.clientId
	group by 
		a.clientID,
		clientName,
		c.sex ,
		susuAccountId
	having isnull((select sum(sc.amount) from ln.susuContribution sc where sc.susuAccountId = a.susuAccountId), 0)>5 
	union all
	select
		a.clientID,
		clientName, 
		c.sex,
		isnull((select sum(sc.amount) from ln.regularSusuContribution sc where sc.regularSusuAccountId = a.regularSusuAccountId and contributionDate <= @date), 0)
		- isnull((select sum(sc.amount) from ln.regularSusuWithdrawal sc where sc.regularSusuAccountId = a.regularSusuAccountId and withdrawalDate <= @date), 0) as balance,
		'Normal Susu' as accountType
	from ln.regularSusuAccount a inner join ln.vwClients b on a.clientId=b.clientId 
		inner join ln.client c on b.clientId = c.clientId
	group by 
		a.clientID,
		clientName,
		c.sex ,
		regularSusuAccountId
	having isnull((select sum(sc.amount) from ln.regularSusuContribution sc where sc.regularSusuAccountId = a.regularSusuAccountId), 0)
		- isnull((select sum(sc.amount) from ln.regularSusuWithdrawal sc where sc.regularSusuAccountId = a.regularSusuAccountId and withdrawalDate <= @date), 0)>5 

	select @totalCredit = ( select sum(balance) from @tbl)

	select  
		count(distinct case when gender='F' then clientId else null end) as countOfFemaleDepositors,
		count(distinct case when gender='M' then clientId else null end) as countOfMaleDepositors,
		sum(case when isnull(round((balance/@paidCapital)*100.0, 2),0)<5 then balance else 0 end) as balanceBelowFivePercent,
		sum(case when isnull(round((balance/@paidCapital)*100.0, 2),0)>=5 then balance else 0 end) as balanceAboveFivePercent, 
		count(case when isnull(round((balance/@paidCapital)*100.0, 2),0)<5 then clientId else null end) as countBelowFivePercent,
		count(case when isnull(round((balance/@paidCapital)*100.0, 2),0)>=5 then clientId else null end) as countAboveFivePercent,  
		accountType
	from @tbl t 
	group by  
		accountType
	order by 1 Asc
end
go