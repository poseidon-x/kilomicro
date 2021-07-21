use coreDB
go

alter view ln.vwInvestmentStatement
with encryption as
select
	d.clientID,
	clientName,
	accountNumber,
	d.investmentID loanID,
	d.investmentNo loanNo,
	isnull([date], getdate()) as [date],
	[Description],
	isnull(Dr, 0) as Dr,
	isnull(Cr,0) as Cr,
	sortCode,
	isnull(d.firstInvestmentDate,getdate()) as firstInvestmentDate,
	isnull(d.maturityDate,getdate()) as maturityDate,
	isnull(s.staffName, '') as staffName,
	isnull(s.staffNo, '') as staffNo,
	isnull(investmentAmount,0) investmentAmount,
	isnull(withdrawalAmount,0) withdrawalAmount,
	isnull(interestAccruedAmount,0) interestAccruedAmount,
	isnull(chargeAmount,0) chargeAmount,
	isnull(princWithdrawalAmount, 0.0) princWithdrawalAmount,
	isnull(intWithdrawalAmount, 0.0) intWithdrawalAmount,
	isnull(t.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name 
from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		isnull(da.investmentDate, getdate()) as [date],
		isnull(naration, 'Investment Made') as [Description],
		da.investmentAmount as Dr,
		0 as Cr,
		1 as sortCode ,
		da.investmentAmount as investmentAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from   ln.investmentAdditional da 
		inner join ln.investment l  on da.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		case when comp_name like '%JIREH%' then 
			 (select max(lp2.interestDate) from ln.investmentInterest lp2 where lp.investmentID=lp2.investmentID
				and (year(lp.interestDate)=year(lp2.interestDate) and month(lp.interestDate)=month(lp2.interestDate))
			)
		when comp_name like '%LINK%' then lp.interestDate
		else dateadd(dd, 1, DATEADD(DD, 1 - DATEPART(DW, lp.interestDate),lp.interestDate))
		end  as [date],
		'Interest on Investment' as [Description],
		lp.interestAmount as Dr,
		0 as Cr,
		3 as sortCode,
		0.0 as investmentAmount,
		0.0 as  withdrawalAmount,
		lp.interestAmount as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 princWithdrawalAmount,
		0.0 intWithdrawalAmount,
		 isnull(c.companyId,0)as CompanyId,
	   isnull(company.comp_Name, '') AS comp_Name
	from ln.investmentInterest lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where interestAmount>0   
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		lp.withdrawalDate as [date],
		isnull(naration, 'Principal Withdrawal') as [Description],
		0 as Dr,
		lp.principalWithdrawal as Cr,
		4 as sortCode,
		0.0 as investmentAmount,
		lp.principalWithdrawal as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		principalWithdrawal as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from ln.investmentWithdrawal lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where principalWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		lp.withdrawalDate as [date],
		isnull(naration, 'Interest Withdrawal') as [Description],
		0 as Dr,
		lp.interestWithdrawal as Cr,
		4 as sortCode,
		0.0 as investmentAmount,
		lp.interestWithdrawal as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
				isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name,
		interestWithdrawal as  intWithdrawalAmount
	from ln.investmentWithdrawal lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where interestWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		lp.chargeDate as [date],
		ct.chargeTypeName as [Description],
		0 as Dr,
		lp.amount as Cr,
		4 as sortCode,
		0.0 as investmentAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		lp.amount as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from ln.investmentCharge lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.chargeType ct on lp.chargeTypeID = ct.chargeTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where amount>0
)  t inner join ln.investment d on t.investmentID = d.investmentID
	left join hc.vwStaff s on d.staffID=s.staffID 
	left join dbo.comp_prof AS company on t.companyId = company.companyId
