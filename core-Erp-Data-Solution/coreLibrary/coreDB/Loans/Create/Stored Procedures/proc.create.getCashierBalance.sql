alter proc ln.getCashierBalance
(
	@startDate datetime,
	@endDate datetime
)
with encryption
as 
select
		isnull(isnull(isnull(isnull(isnull(a.PaymentType, h.PaymentType), b.PaymentType), d.PaymentType), e.PaymentType),f.PaymentType) as PaymentType, 
		isnull(isnull(isnull(isnull(isnull(a.userName, h.userName), b.userName), d.userName), e.username),f.userName) as userName,
		isnull(isnull(isnull(isnull(isnull(a.full_name, h.full_name), d.full_name), b.full_name), e.full_name),f.full_name) as full_name,
		isnull(balanceBefore,0) as balanceBefore,
		isnull(balanceAfter,0) as balanceAfter,
		isnull(loanRepayment,0) as loanRepayment, 
		isnull(fees,0) as fees,
		isnull(amountDisbursed,0) as loanDisbursed,
		isnull(isnull(h.deposits,d.deposits),0) as deposits,
		isnull(isnull(b.withdrawn,e.withdrawn),0) as withdrawn,		
		isnull(chargeAmount,0) as chargeAmount,
		isnull(isnull(b.withdrawalCount,e.withdrawalCount),0) as withdrawalCount,
				isnull(g.disbursementCount,0) as disbursementCount

from
(select
		isnull(mp.modeOfPaymentName,'') as PaymentType,
		0 as balanceBefore,
		0 as balanceAfter, 
		sum(amount) loanRepayment,
		sum(feeAmount) as fees,
		ct.userName,
		u.full_name,
		0.0 as disbursementCount,
		0.0 as withdrawalCount
	from ln.cashierReceipt lr
		inner join ln.loan l on lr.loanID=l.loanID
		left outer join ln.repaymentType rt on lr.repaymentTypeID=rt.repaymentTypeID
		left outer join ln.modeOfPayment mp on lr.paymentModeID = mp.modeOfPaymentID
		inner join ln.client c on l.clientID=c.clientID
		inner join ln.cashiersTill ct on ct.cashiersTillID=lr.cashierTillID
		inner join dbo.users u on lower(ltrim(rtrim(ct.userName)))  = lower(ltrim(rtrim(u.user_name)))
		where txDate between @startDate and @endDate
		group by mp.modeOfPaymentName,
		ct.userName,
		u.full_name
	) a
	full outer join
	(
		select   
			isnull(mp.modeOfPaymentName,'') as PaymentType, 
			sum(lr.amount) amountDisbursed,
			isnull(sum(l.amountApproved),0) as loanAmount,
			ct.userName,
			u.full_name,
			count(*) as disbursementCount,
		0.0 as withdrawalCount
		from ln.cashierDisbursement lr
			inner join ln.loan l on lr.loanID=l.loanID 
			left outer join ln.modeOfPayment mp on lr.paymentModeID = mp.modeOfPaymentID
			inner join ln.client c on l.clientID=c.clientID
			inner join ln.cashiersTill ct on ct.cashiersTillID=lr.cashierTillID
			inner join dbo.users u on lower(ltrim(rtrim(ct.userName)))  = lower(ltrim(rtrim(u.user_name)))
		where txDate between @startDate and @endDate
		group by isnull(mp.modeOfPaymentName,''), 
			ct.userName,
			u.full_name
	)g on a.userName=g.userName and a.PaymentType = g.PaymentType
full outer join 
(
	select
		isnull(mp.[modeOfPaymentName],'') as PaymentType, 
		isnull(u.user_name, '') as userName,
		isnull(u.full_name, '') as full_name,
		isnull(sum(depositAmount),0)  as deposits
	from ln.depositAdditional lr
			left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
			left outer join dbo.users u on ltrim(rtrim(lr.creator))=ltrim(rtrim(u.user_name))
	where depositDate between @startDate and @endDate 
	group by isnull(mp.[modeOfPaymentName],''),
		isnull(u.user_name, ''),
		isnull(u.full_name, '') 
) h on a.userName=h.userName and a.PaymentType = h.PaymentType
full outer join 
(
	select
		isnull(mp.[modeOfPaymentName],'') as PaymentType, 
		isnull(u.user_name, '') as userName,
		isnull(u.full_name, '') as full_name,
		isnull(sum(principalWithdrawal+interestWithdrawal),0)  as withdrawn,
		0.0 as disbursementCount,
		count(*) as withdrawalCount
	from ln.depositWithdrawal lr
			left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
			left outer join dbo.users u on ltrim(rtrim(lr.creator))=ltrim(rtrim(u.user_name))
	where withdrawalDate between @startDate and @endDate 
	group by isnull(mp.[modeOfPaymentName],''),
		isnull(u.user_name, ''),
		isnull(u.full_name, '') 
) b on (a.userName=b.userName and a.PaymentType = b.PaymentType) or (b.userName = h.userName and b.PaymentType = h.PaymentType)
full outer join 
(
	select
		isnull(mp.[modeOfPaymentName],'') as PaymentType, 
		isnull(u.user_name, '') as userName,
		isnull(u.full_name, '') as full_name,
		isnull(sum(savingAmount),0)  as deposits
	from ln.savingAdditional lr
			left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
			left outer join dbo.users u on ltrim(rtrim(lr.creator))=ltrim(rtrim(u.user_name))
	where savingDate between @startDate and @endDate 
	group by isnull(mp.[modeOfPaymentName],''),
		isnull(u.user_name, ''),
		isnull(u.full_name, '') 
) d on a.userName=d.userName and a.PaymentType = d.PaymentType
full outer join 
(
	select
		isnull(mp.[modeOfPaymentName],'') as PaymentType, 
		isnull(u.user_name, '') as userName,
		isnull(u.full_name, '') as full_name,
		isnull(sum(principalWithdrawal+interestWithdrawal),0)  as withdrawn,
		0.0 as disbursementCount,
		count(*) as withdrawalCount
	from ln.savingWithdrawal lr
			left outer join ln.modeOfPayment mp on lr.modeOfPaymentID = mp.modeOfPaymentID
			left outer join dbo.users u on ltrim(rtrim(lr.creator))=ltrim(rtrim(u.user_name))
	where withdrawalDate between @startDate and @endDate 
	group by isnull(mp.[modeOfPaymentName],''),
		isnull(u.user_name, ''),
		isnull(u.full_name, '') 
) e on a.userName=e.userName and a.PaymentType = e.PaymentType
full outer join 
(
	select
		'Cash' as PaymentType, 
		isnull(u.user_name, '') as userName,
		isnull(u.full_name, '') as full_name,
		sum(lr.chargeAmount)  as chargeAmount
	from ln.clientServiceCharge lr
			left outer join dbo.users u on ltrim(rtrim(lr.creator))=ltrim(rtrim(u.user_name))
	where lr.chargeDate between @startDate and @endDate 
	group by 
		isnull(u.user_name, ''),
		isnull(u.full_name, '') 
) f on a.userName=f.userName and a.PaymentType = f.PaymentType

