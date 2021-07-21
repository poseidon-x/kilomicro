use coreDB
go

alter view ln.vwDepositStatement
with encryption as
select
	d.clientID,
	clientName,
	accountNumber,
	d.depositID loanID,
	d.depositNo loanNo,
	dt.depositTypeName depositTypeName,
	isnull([date], getdate()) as [date],
	[Description],
	isnull(Dr, 0) as Dr,
	isnull(Cr,0) as Cr,
	sortCode,
	isnull(d.firstDepositDate,getdate()) as firstDepositDate,
	isnull(d.maturityDate,getdate()) as maturityDate,
	isnull(s.staffName, '') as staffName,
	isnull(s.staffNo, '') as staffNo,
	isnull(depositAmount,0) depositAmount,
	isnull(withdrawalAmount,0) withdrawalAmount,
	isnull(interestAccruedAmount,0) interestAccruedAmount,
	isnull(chargeAmount,0) chargeAmount,
	isnull(princWithdrawalAmount, 0.0) princWithdrawalAmount,
	isnull(intWithdrawalAmount, 0.0) intWithdrawalAmount,
	isnull(t.companyId,0)as CompanyId,
	isnull(comp_Name, '') AS comp_Name

from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		isnull(da.depositDate, getdate()) as [date],
		isnull(naration, 'Deposit Made') as [Description],
		da.depositAmount as Dr,
		0 as Cr,
		1 as sortCode ,
		da.depositAmount as depositAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
       isnull(c.companyId,0)as CompanyId,
	   isnull(company.comp_Name, '') AS comp_Name
	from   ln.depositAdditional da 
		inner join ln.deposit l  on da.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID 
		left join dbo.comp_prof AS company on c.companyId = company.companyId

	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		case when comp_name like '%LINK%' then 
			 (select max(lp2.interestDate) from ln.depositInterest lp2 where lp.depositID=lp2.depositID
				and (year(lp.interestDate)=year(lp2.interestDate) and month(lp.interestDate)=month(lp2.interestDate))
			)
		ELSE lp.interestDate END [date],
		'Interest on Deposit' as [Description],
		lp.interestAmount as Dr,
		0 as Cr,
		3 as sortCode,
		0.0 as depositAmount,
		0.0 as  withdrawalAmount,
		lp.interestAmount as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 princWithdrawalAmount,
		0.0 intWithdrawalAmount,
		 isnull(c.companyId,0)as CompanyId,
	   isnull(company.comp_Name, '') AS comp_Name
	from 
		(
			select 
				depositId,
				SUM(interestAmount) as interestAmount,
				INTERESTDATE
			FROM
			(
				select 
					depositId,
					interestAmount,
					MAX(interestDate) over(PARTITION BY depositID, YEAR(InterestDate), Month(InterestDate) 
						ORDER by depositID, YEAR(InterestDate), Month(InterestDate)) as INTERESTDATE
				from ln.depositInterest
			) T
			GROUP  BY
				depositId, 
				INTERESTDATE
		)lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID 
		left join dbo.comp_prof AS company on c.companyId = company.companyId

	where interestAmount>0   
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		lp.withdrawalDate as [date],
		isnull(naration, 'Principal Withdrawal') as [Description],
		0 as Dr,
		lp.principalWithdrawal as Cr,
		4 as sortCode,
		0.0 as depositAmount,
		lp.principalWithdrawal as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		principalWithdrawal as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		isnull(c.companyId,0)as CompanyId,
	   isnull(company.comp_Name, '') AS comp_Name
	from ln.depositWithdrawal lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID 
		left join dbo.comp_prof AS company on c.companyId = company.companyId

	where principalWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		lp.withdrawalDate as [date],
		isnull(naration, 'Interest Withdrawal') as [Description],
		0 as Dr,
		lp.interestWithdrawal as Cr,
		4 as sortCode,
		0.0 as depositAmount,
		lp.interestWithdrawal as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		0.0 as chargeAmount,
		0.0 as  princWithdrawalAmount,
		interestWithdrawal as  intWithdrawalAmount,
		isnull(c.companyId,0)as CompanyId,
	    isnull(company.comp_Name, '') AS comp_Name
	from ln.depositWithdrawal lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where interestWithdrawal>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		lp.chargeDate as [date],
		ct.chargeTypeName as [Description],
		0 as Dr,
		lp.amount as Cr,
		4 as sortCode,
		0.0 as depositAmount,
		0.0 as  withdrawalAmount,
		0.0 as interestAccruedAmount,
		lp.amount as chargeAmount,
		0.0 as  princWithdrawalAmount,
		0.0 as  intWithdrawalAmount,
		isnull(c.companyId,0)as CompanyId,
	    isnull(company.comp_Name, '') AS comp_Name
	from ln.depositCharge lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.chargeType ct on lp.chargeTypeID = ct.chargeTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId

	where amount>0
)  t inner join ln.deposit d on t.depositID = d.depositID
	 inner join ln.depositType dt on dt.depositTypeId=d.depositTypeId
	left join hc.vwStaff s on d.staffID=s.staffID 