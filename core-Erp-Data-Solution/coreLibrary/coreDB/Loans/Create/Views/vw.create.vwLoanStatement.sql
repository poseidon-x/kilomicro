use coreDB
go

alter view ln.vwLoanStatement 
with encryption as
select
	t.clientID,
	t.clientName,
	t.accountNumber,
	t.loanID,
	t.loanNo,
	isnull([date], getdate()) as [date],
	[Description],
	isnull(Dr, 0) as Dr,
	isnull(Cr,0) as Cr,
	sortCode,
	isnull(cast(isnull(case when a.agentNo is null then 0 else 1 end, 0) as bit),0) as isAgentLoan,
	isnull(a.surname + ', ' + a.otherNames,'') as agentName,
	isnull(a.agentNo,'') as agentNo,
	isnull(l.balance,0) as balance, 
	isnull(disbursementDate, getdate()) as disbursementDate,
	isnull(amountApproved, 0) amountApproved,
	isnull(loanTenure, 0) loanTenure,
	isnull(dateadd(month, loanTenure, disbursementDate), getdate()) as expiryDate,
	isnull(amountDisbursed
			- (select sum(principalPaid) from ln.loanRepayment r where r.loanID = l.loanID), 0) as principalBalance,
	isnull (Arrears, 0 ) as Arrears
			-- isnull(t.companyId,0)as companyID , 
		 --isnull(company.comp_name,'')as comp_Name
from
(			
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		isnull(l.disbursementDate, getdate()) as [date],
		'Loan Approval' as [Description],
		l.amountDisbursed as Dr,
		0 as Cr,
		1 as sortCode, 
		0 as Arrears
		 --isnull(c.companyId,0)as companyID , 
		 --isnull(company.comp_name,'')as comp_Name

	from   ln.loan l 
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		feeDate as [date],
		case when f.feeTypeId = 1 then 'Processing Fee' when f.feeTypeId = 2 then 'Application Fee' end as [Description],
		feeAmount as Dr,
		0 as Cr,
		2 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loan l 
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanFee f on l.loanID = f.loanID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		repaymentDate as [date],
		'Insurance' as [Description],
		amountPaid as Dr,
		0 as Cr,
		2 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loan l 
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanRepayment f on l.loanID = f.loanID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	where repaymentTypeID=8 and amountPaid>0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.penaltyDate as [date],
		isnull(pt.narration, 'Additional Interest') as [Description],
		sum(lp.penaltyFee) as Dr,
		0 as Cr,
		case when lp.penaltyTypeId = 2 then 2 else 3 end as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loanPenalty lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		left join ln.penaltyType pt on pt.penaltyTypeId = lp.penaltyTypeId
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	group by 
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end ,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.penaltyDate,
		pt.narration,
		lp.penaltyTypeId
		--isnull(c.companyId,0), 
		-- isnull(company.comp_name,'')
	having sum(lp.penaltyFee) >0
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		l.disbursementDate as [date],
		'Interest on Loan' as [Description],
		sum(lp.interestPayment) as Dr,
		0 as Cr,
		3 as sortCode,
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name,
		sum(case when lp.repaymentDate <= getdate() then lp.interestBalance + lp.principalBalance else 0 end ) as Arrears
	from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	group by 
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		l.disbursementDate
		--isnull(c.companyId,0) , 
		-- isnull(company.comp_name,'')
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		max(w.writeOffDate) as [date],
		'Interest written off (early repayment)' as [Description],
		0 as Dr,
		sum(w.writeOffAmount) as Cr,
		3 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loan l 
		inner join ln.client c on l.clientID = c.clientID
		inner join ln.loanIterestWriteOff w on l.loanID = w.loanID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	group by 
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		l.disbursementDate
		--isnull(c.companyId,0) , 
		-- isnull(company.comp_name,'')
	having sum(writeOffAmount) >0
		
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Loan Repayment' as [Description],
		0 as Dr,
		lp.amountPaid as Cr,
		4 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	where lp.repaymentTypeID in (1,2,3) and modeOFPaymentID<>6

	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Negative Balance Applied' as [Description],
		isnull(case when lp.amountPaid<0 then -lp.amountPaid else 0 end, 0) as Dr,
		isnull(case when lp.amountPaid<0 then 0 else lp.amountPaid end, 0) as Cr,
		4 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	where lp.repaymentTypeID in (1,2,3) and modeOfPaymentID=6
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Processing Fee Payment' as [Description],
		0 as Dr,
		lp.amountPaid as Cr,
		4 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	where lp.repaymentTypeID in (6)
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Insurance Payment' as [Description],
		0 as Dr,
		lp.amountPaid as Cr,
		4 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	where lp.repaymentTypeID in (8)
	union all
	select
		c.clientID,
		case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		lp.repaymentDate as [date],
		'Additional Interest Payment' as [Description],
		0 as Dr,
		lp.amountPaid as Cr,
		4 as sortCode,
		0 as Arrears
		--isnull(c.companyId,0)as companyID , 
		-- isnull(company.comp_name,'')as comp_Name
	from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
		inner join ln.client c on l.clientID = c.clientID
		--left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
	where lp.repaymentTypeID in (7)
)  t inner join ln.loan l on t.loanID = l.loanID
	left join ln.agent a on l.agentID = a.agentID
	--left join dbo.comp_prof AS company on t.companyId = company.comp_prof_id
where l.loanStatusID>3 and l.loanStatusID<>7