use coreDB
go

create view ln.vwDepositTenor
with encryption as
	select
	'1 Month' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType
	union all
	select
	'3 Months' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType
	union all
	select
	'6 Months' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType
	union all
	select
	'9 Months' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType
	union all
	select
	'12 Months' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType
	union all
	select
	'18 Months' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType
	union all
	select
	'24 Months' as tenor,
		isnull(max(case when depositTypeID=1 then interestRate else null end),0) as rateSavings,
		isnull(max(case when depositTypeID=2 then interestRate else null end),0) as rateTerm,
		isnull(max(case when depositTypeID=3 then interestRate else null end),0) as rateOther,
		isnull(max(case when depositTypeID=4 then interestRate else null end),0) as rateCash
	from ln.depositType

go