use coreDB
go

alter proc ln.getDepositBalanceReport
(
	@date datetime
)
with encryption
as 
select
	dp.clientID,
	clientName,
	accountNumber,
	dp.depositID,
	dp.depositNo,
	depositTypeName,
	isnull(min([date]), getdate()) as [date],
	isnull(sum(principal),0) principal,
	isnull(sum(interest),0) interest, 
	isnull(sum(paidPrinc),0) paidPrinc,
	isnull(sum(paidInt),0) paidInt, 
	isnull(s.staffID, 0) as staffID,
	isnull(max(interestExpected),0) as interestExpected,
	isnull(sum(interest),0)- isnull(sum(paidInt),0) as interestBalance,
	isnull(sum(principal),0)- isnull(sum(paidPrinc),0) as principalBalance,
	isnull(max(maturityDate), getdate()) as maturityDate,
	isnull(max(firstDepositDate), getdate()) as firstDepositDate,
	isnull(max(dp.interestRate)*12.0, 0) as interestRate,
	isnull(sum(principal),0) + isnull(sum(interest),0) - isnull(sum(paidPrinc),0) - isnull(sum(paidInt),0) as balance  ,
	isnull(s.staffName, '') staffName,
	isnull(s.staffNo, '') staffNo,
	isnull(lg.loangroupname, '') as LoanGroupName

from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		da.depositDate as [date],
		'Deposit Made' as [Description],
		lt.depositTypeName,
		da.depositAmount as principal,
		0 as interest, 
		0 as paidPrinc,
		0 as paidInt, 
		1 as sortCode ,
		l.interestRate,
			isnull(lg.loangroupname, '') as LoanGroupName

	from  ln.depositAdditional da
		inner join ln.deposit l  on da.depositID=l.depositID 
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.depositType lt on l.depositTypeID=lt.depositTypeID
	where da.depositDate<=@date  
		--where da.depositDate<=2014-09-10   

	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		lp.interestDate as [date],
		'Interest on Deposit' as [Description],
		lt.depositTypeName,
		0 as principal,
		lp.interestAmount as interest, 
		0 as paidPrinc,
		0 as paidInt, 
		3 as sortCode ,
		l.interestRate,
		isnull(lg.loangroupname, '') as LoanGroupName
	from ln.depositInterest lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.depositType lt on l.depositTypeID=lt.depositTypeID
	where (lp.interestDate <= @date)
		--where (lp.interestDate <= 2014-09-10)

	 
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		lp.withdrawalDate as [date],
		'Withdrawal from Deposit' as [Description],
		lt.depositTypeName,
		0 as principal,
		0 as interest, 
		lp.principalWithdrawal as paidPrinc,
		0 as paidInt, 
		4 as sortCode,
		l.interestRate ,
		isnull(lg.loangroupname, '') as LoanGroupName
	from ln.depositWithdrawal lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.depositType lt on l.depositTypeID=lt.depositTypeID
	where lp.withdrawalDate <= @date
		--where lp.withdrawalDate <= 2015-09-10

		and lp.principalWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		lp.withdrawalDate as [date],
		'Withdrawal of Interest from Deposit' as [Description],
		lt.depositTypeName,
		0 as principal,
		0 as interest,
		0 as addInt, 
		lp.interestWithdrawal as paidInt, 
		4 as sortCode ,
		l.interestRate,
		isnull(lg.loangroupname, '') as LoanGroupName
	from ln.depositWithdrawal lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.depositType lt on l.depositTypeID=lt.depositTypeID
	where  (lp.withdrawalDate <= @date
		--where  (lp.withdrawalDate <= 2015-09-10
 
	and lp.interestWithdrawal>0 ) 
) t inner join ln.deposit dp on t.depositID=dp.depositID
	left join hc.vwStaff s on dp.staffID=s.staffID
			left join ln.loangroupclient lgc on t.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
group by 
	dp.clientID,
	clientName,
	accountNumber,
	dp.depositID,
	dp.depositNo,
	depositTypeName ,
	isnull(s.staffName, ''),
	isnull(s.staffNo, ''), 
	isnull(s.staffID, 0),
	isnull(lg.loangroupname, '')
 
order by clientName, date

go

