use coreDB
go

alter procedure ln.getMF6
(
	@endDate datetime
)
with encryption as
begin
	declare @paidCapital float
	declare @tbl table
	(
			clientID int,
			clientName nvarchar(200),
			accountNumber nvarchar(30),
			balance float
	)
	declare @totalCredit float

	select @paidCapital = sum(dbo.acc_bal(acct_id, cat_code, '1950-01-01', @endDate, null))
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
		clientID,
		clientName,
		accountNumber,
		sum(dr-cr) as balance
	from ln.vwDepositStatement
	where [date] <= @endDate
	group by 
		clientID,
		clientName,
		accountNumber
	having sum(dr-cr)>5
	union all
	select
		clientID,
		clientName,
		accountNumber,
		sum(dr-cr) as balance
	from ln.vwSavingStatement
	where [date] <= @endDate
	group by 
		clientID,
		clientName,
		accountNumber
	having sum(dr-cr)>5 

	select @totalCredit = ( select sum(balance) from @tbl)

	select
		isnull(clientID,0) as clientID,
		clientName,
		accountNumber, 
		isnull(balance, 0) as balance,
		isnull(round((balance/@paidCapital)*100.0, 2),0) as percentPaidCapital,
		isnull(round((balance/@totalCredit)*100.0, 2),0) as percentCredit
	from @tbl t
	where isnull(round((balance/@paidCapital)*100.0, 2),0)>5
	order by 5 desc
end