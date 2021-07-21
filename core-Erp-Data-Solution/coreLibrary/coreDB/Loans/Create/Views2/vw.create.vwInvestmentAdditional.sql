use coreDB
go

create view ln.vwInvestmentAdditional
with encryption 
as
SELECT        d.investmentID, d.clientID, case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) 
			then c.companyName else c.surName + ', ' + c.otherNames end AS clientName, w.investmentDate, 
			c.accountNumber, b.branchName, isnull(w.creation_date, getDate()) creation_date, w.creator, 
            w.investmentAmount, w.investmentAdditionalID, t2.investmentTypeName, 
            t2.investmentTypeID,  d.interestExpected,
			isnull(modeOfPaymentName, '') as modeOfPaymentName,
			d.investmentNo,
			isnull(naration, '') as naration,  
			isnull(t.principal,0) principal,
			isnull(t.interest,0) interest, 
			isnull(t.princPaid,0) paidPrinc,
			isnull(t.intPaid,0) paidInt, 
			isnull(d.staffID, 0) as staffID,
			isnull(interest-intPaid,0) as interestBalance,
			isnull(principal - princPaid, 0) as principalBalance,
			isnull(d.maturityDate, getdate()) as maturityDate,
			isnull(d.firstInvestmentDate, getdate()) as firstInvestmentDate,
			isnull(d.interestRate, 0) as interestRate,
			isnull(principal - princPaid + interest-intPaid, 0) as balance  ,
			isnull(s.staffName, '') staffName,
			isnull(s.staffNo, '') staffNo
FROM       
(select 
	investmentID, 
	clientID,
	clientName,
	accountNumber,
	investmentNo,
	sum(investmentAmount) as principal,
	sum(interestAccruedAmount) as interest,
	sum(princWithdrawalAmount) as princPaid,
	sum(intWithdrawalAmount) as intPaid
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
		0.0 as  intWithdrawalAmount
	from   ln.investmentAdditional da 
		inner join ln.investment l  on da.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
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
		0.0 intWithdrawalAmount
	from ln.investmentInterest lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
		cross join comp_prof
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
		0.0 as  intWithdrawalAmount
	from ln.investmentWithdrawal lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
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
		interestWithdrawal as  intWithdrawalAmount
	from ln.investmentWithdrawal lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
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
		0.0 as  intWithdrawalAmount
	from ln.investmentCharge lp inner join ln.investment l on lp.investmentID=l.investmentID
		inner join ln.client c on l.clientID = c.clientID 
		inner join ln.chargeType ct on lp.chargeTypeID = ct.chargeTypeID
	where amount>0
) t
	group by 
	investmentID, 
	clientID,
	clientName,
	accountNumber,
	investmentNo) t inner join ln.investment d on t.investmentID = d.investmentID
  INNER JOIN ln.client AS c ON d.clientID = c.clientID INNER JOIN
                         ln.investmentAdditional AS w ON d.investmentID = w.investmentID INNER JOIN
                         ln.branch AS b ON c.branchID = b.branchID INNER JOIN
                         ln.investmentType AS t2 ON d.investmentTypeID = t2.investmentTypeID
						 left join ln.modeOfPayment mp on w.modeOfPaymentID=mp.modeOfPaymentID	
	left join hc.vwStaff s on d.staffID=s.staffID  

go
