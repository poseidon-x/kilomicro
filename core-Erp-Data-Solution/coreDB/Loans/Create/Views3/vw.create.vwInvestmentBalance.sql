use coreDB
go

create view ln.vwInvestmentBalance
with encryption as

select
	dp.clientID,
	clientName,
	accountNumber,
	dp.investmentID,
	dp.investmentNo,
	investmentTypeName,
	isnull(min([date]), getdate()) as [date],
	isnull(sum(principal),0) principal,
	isnull(sum(interest),0) interest, 
	isnull(sum(paidPrinc),0) paidPrinc,
	isnull(sum(paidInt),0) paidInt, 
	isnull(s.staffID, 0) as staffID,
	isnull(max(interestExpected),0) as interestExpected,
	isnull(max(interestBalance),0) as interestBalance,
	isnull(max(principalBalance),0) as principalBalance,
	isnull(max(maturityDate), getdate()) as maturityDate,
	isnull(max(firstInvestmentDate), getdate()) as firstInvestmentDate,
	isnull(max(dp.interestRate)*12.0, 0) as interestRate,
	isnull(sum(principal),0) + isnull(sum(interest),0) - isnull(sum(paidPrinc),0) - isnull(sum(paidInt),0) as balance  ,
	isnull(s.staffName, '') staffName,
	isnull(s.staffNo, '') staffNo,
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
		da.investmentDate as [date],
		'Investment Made' as [Description],
		lt.investmentTypeName,
		da.investmentAmount as principal,
		0 as interest, 
		0 as paidPrinc,
		0 as paidInt, 
		1 as sortCode ,
		l.interestRate,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from  ln.investmentAdditional da
		inner join ln.investment l  on da.investmentID=l.investmentID 
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.investmentType lt on l.investmentTypeID=lt.investmentTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where da.investmentDate<=getDate()  
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		lp.interestDate as [date],
		'Interest on Investment' as [Description],
		lt.investmentTypeName,
		0 as principal,
		lp.interestAmount as interest, 
		0 as paidPrinc,
		0 as paidInt, 
		3 as sortCode ,
		l.interestRate,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from ln.investmentInterest lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.investmentType lt on l.investmentTypeID=lt.investmentTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where (lp.interestDate <= getDate()) 
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		lp.withdrawalDate as [date],
		'Withdrawal from Investment' as [Description],
		lt.investmentTypeName,
		0 as principal,
		0 as interest, 
		lp.principalWithdrawal as paidPrinc,
		0 as paidInt, 
		4 as sortCode,
		l.interestRate,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from ln.investmentWithdrawal lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.investmentType lt on l.investmentTypeID=lt.investmentTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where lp.withdrawalDate <= getDate()	and lp.principalWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.investmentID,
		l.investmentNo,
		lp.withdrawalDate as [date],
		'Withdrawal of Interest from Investment' as [Description],
		lt.investmentTypeName,
		0 as principal,
		0 as interest,
		0 as addInt, 
		lp.interestWithdrawal as paidInt, 
		4 as sortCode ,
		l.interestRate,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	from ln.investmentWithdrawal lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.investmentType lt on l.investmentTypeID=lt.investmentTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where  (lp.withdrawalDate <= getDate() and lp.interestWithdrawal>0 ) 
) t inner join ln.investment dp on t.investmentID=dp.investmentID
	left join hc.vwStaff s on dp.staffID=s.staffID
	left join dbo.comp_prof AS company on t.companyId = company.companyId

group by 
	dp.clientID,
	clientName,
	accountNumber,
	dp.investmentID,
	dp.investmentNo,
	investmentTypeName ,
	isnull(s.staffName, ''),
	isnull(s.staffNo, ''), 
	isnull(s.staffID, 0),
	isnull(t.companyId,0),
    isnull(company.comp_Name, '')  

go

