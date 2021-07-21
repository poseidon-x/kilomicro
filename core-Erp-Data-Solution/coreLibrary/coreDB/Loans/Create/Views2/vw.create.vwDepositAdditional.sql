use coreDB
go

alter view ln.vwDepositAdditional
with encryption 
as
SELECT      d.depositID, d.clientID, case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) 
			then c.companyName 
			else c.surName + ' ' + c.otherNames end AS clientName, 
			w.depositDate, 
			c.accountNumber, b.branchName, isnull(w.creation_date, getDate()) creation_date, w.creator, 
			case when (d.depositTypeID = 5) then s else 
			w.depositAmount end as depositAmount,
			w.depositAdditionalID, t2.depositTypeName, 
            t2.depositTypeID,  d.interestExpected,
			isnull(modeOfPaymentName, '') as modeOfPaymentName,
			d.depositNo,
			isnull(naration, '') as naration,  
			isnull(t.principal,0) principal,
			isnull(t.interest,0) interest, 
			isnull(t.princPaid,0) paidPrinc,
			isnull(t.intPaid,0) paidInt, 
			isnull(d.staffID, 0) as staffID,
			isnull(interest-intPaid,0) as interestBalance,
			isnull(principal - princPaid, 0) as principalBalance,
			isnull(d.maturityDate, getdate()) as maturityDate,
			isnull(d.firstDepositDate, getdate()) as firstDepositDate,
			isnull(d.interestRate, 0) as interestRate,
			isnull(principal - princPaid + interest-intPaid, 0) as balance  ,
			isnull(s.staffName, '') staffName,
			isnull(s.staffNo, '') staffNo,
			isnull(w.posted, 0) as posted,
			isnull(CompanyId ,0)as CompanyId,
	        isnull(comp_Name, '') AS comp_Name,
	isnull(isnull(a.surname+' ' + a.otherNames, s.surname+', ' + s.otherNames), '') as agentName
FROM       
(select 
	depositID, 
	clientID,
	clientName,
	accountNumber,
	depositNo, 
	sum(depositAmount) as principal,
	sum(interestAccruedAmount) as interest,
	sum(princWithdrawalAmount) as princPaid,
	sum(intWithdrawalAmount) as intPaid
	from
	(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.depositID,
		l.depositNo,
		--da.deposit,
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
		da.posted,
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
		case when comp_name like '%JIREH%' then 
			 (select max(lp2.interestDate) from ln.depositInterest lp2 where lp.depositID=lp2.depositID
				and (year(lp.interestDate)=year(lp2.interestDate) and month(lp.interestDate)=month(lp2.interestDate))
			)
		else dateadd(dd, 1, DATEADD(DD, 1 - DATEPART(DW, lp.interestDate),lp.interestDate))
		end  as [date],
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
	    isnull(company.comp_Name, '') AS comp_Name,
		cast(1 as bit) as posted
	from ln.depositInterest lp inner join ln.deposit l on lp.depositID=l.depositID
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
	    isnull(company.comp_Name, '') AS comp_Name,
		lp.posted
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
	    isnull(company.comp_Name, '') AS comp_Name,
		lp.posted
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
	    isnull(company.comp_Name, '') AS comp_Name,
		cast(1 as bit) as posted
	from ln.depositCharge lp inner join ln.deposit l on lp.depositID=l.depositID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.chargeType ct on lp.chargeTypeID = ct.chargeTypeID
		left join dbo.comp_prof AS company on c.companyId = company.companyId
	where amount>0
) t
	group by 
	depositID, 
	clientID,
	clientName,
	accountNumber,
	depositNo 
) t inner join ln.deposit d on t.depositID = d.depositID
  INNER JOIN ln.client AS c ON d.clientID = c.clientID INNER JOIN
	--case when (d.depositTypeID == 5)

    ln.depositAdditional AS w ON d.depositID = w.depositID INNER JOIN 
	--inner join 
	(
			select sum(w.amountInvested) as ai
			from ln.depositAdditional
			group by depositID
		   ) s  on t.depositID = s.depositID 	INNER JOIN
    ln.branch AS b ON c.branchID = b.branchID   INNER JOIN
	
    ln.depositType AS t2 ON d.depositTypeID = t2.depositTypeID
	left join ln.modeOfPayment mp on w.modeOfPaymentID=mp.modeOfPaymentID	
	left join hc.vwStaff s on d.staffID=s.staffID  
	left join ln.agent a on a.agentId = d.agentId

	

go

