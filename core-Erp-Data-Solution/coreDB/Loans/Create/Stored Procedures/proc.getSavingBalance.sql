use coreDB
go

alter proc ln.getSavingBalanceReport
(
	@date datetime
)
with encryption
as 
select
	dp.clientID,
	clientName,
	accountNumber,
	dp.savingID,
	dp.savingNo,
	savingTypeName,
	isnull(min([date]), getdate()) as [date],
	isnull(sum(principal),0) principal,
	isnull(sum(interest),0) interest, 
	isnull(sum(paidPrinc),0) paidPrinc,
	isnull(sum(paidInt),0) paidInt, 
	isnull(lg.relationsOfficerStaffId,isnull(s.staffID, 0)) as staffID,
	isnull(sum(interest),0) as interestExpected,
	isnull(max(interestBalance),0) as interestBalance,
	isnull(max(principalBalance),0) as principalBalance,
	isnull(max(maturityDate), getdate()) as maturityDate,
	isnull(max(firstSavingDate), getdate()) as firstSavingDate,
	isnull(s.staffName, '') staffName,
	isnull(s.staffNo, '') staffNo,
    isnull(lg.loangroupname, '') as LoanGroupName

from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		da.savingDate as [date],
		'Savings Deposit Made' as [Description],
		lt.savingTypeName,
		da.savingAmount as principal,
		0 as interest, 
		0 as paidPrinc,
		0 as paidInt, 
		2 as sortCode,
		isnull(lg.loangroupname, '') as LoanGroupName

	from  ln.savingAdditional da
		inner join ln.saving l  on da.savingID=l.savingID 
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.savingType lt on l.savingTypeID=lt.savingTypeID
	where da.savingDate<=@date  
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.interestDate as [date],
		'Interest on Savings' as [Description],
		lt.savingTypeName,
		0 as principal,
		lp.interestAmount as interest, 
		0 as paidPrinc,
		0 as paidInt, 
		1 as sortCode,
		isnull(lg.loangroupname, '') as LoanGroupName
	from ln.savingInterest lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.savingType lt on l.savingTypeID=lt.savingTypeID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
	where (lp.interestDate <= @date   ) 
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.withdrawalDate as [date],
		'Withdrawal from Savings' as [Description],
		lt.savingTypeName,
		0 as principal,
		0 as interest, 
		lp.principalWithdrawal as paidPrinc,
		0 as paidInt, 
		4 as sortCode,
		isnull(lg.loangroupname, '') as LoanGroupName
	from ln.savingWithdrawal lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.savingType lt on l.savingTypeID=lt.savingTypeID
	where lp.withdrawalDate <= @date	and lp.principalWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.savingID,
		l.savingNo,
		lp.withdrawalDate as [date],
		'Withdrawal of Interest from Savings' as [Description],
		lt.savingTypeName,
		0 as principal,
		0 as interest,
		0 as addInt, 
		lp.interestWithdrawal as paidInt, 
		4 as sortCode,
		isnull(lg.loangroupname, '') as LoanGroupName
	from ln.savingWithdrawal lp inner join ln.saving l on lp.savingID=l.savingID
		inner join ln.client c on l.clientID = c.clientID
		left join ln.loangroupclient lgc on c.clientid = lgc.clientid
		left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
		inner join ln.savingType lt on l.savingTypeID=lt.savingTypeID
	where  (lp.withdrawalDate <= @date and lp.interestWithdrawal>0 ) 
) t inner join ln.saving dp on t.savingID=dp.savingID
	left join hc.vwStaff s on dp.staffID=s.staffID
	left join ln.loangroupclient lgc on t.clientid = lgc.clientid
	left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
group by 
	dp.clientID,
	clientName,
	accountNumber,
	dp.savingID,
	dp.savingNo,
	savingTypeName ,
	isnull(s.staffName, ''),
	isnull(s.staffNo, ''), 
	isnull(lg.relationsOfficerStaffId,isnull(s.staffID, 0)),
	isnull(lg.loangroupname, '')
order by clientName, date 

go
