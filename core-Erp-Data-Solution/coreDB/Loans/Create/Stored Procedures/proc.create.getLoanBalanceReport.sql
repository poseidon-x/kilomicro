use coreDB
go

alter proc ln.getLoanBalanceReport
(
	@date datetime
)
with encryption
as 
begin
	declare @ed datetime
	set @ed=
		cast(''+cast(datepart(yyyy,@date) as nvarchar(4))+'-'+cast(datepart(mm,@date) as nvarchar(2))
		+'-'+cast(datepart(dd,@date) as nvarchar(2)) + ' 23:59:59' as datetime)
	select
		t.clientID,
		t.clientName,
		t.accountNumber,
		t.loanID,
		t.loanNo,
		t.loanTypeName,
		isnull(min([date]), getdate()) as [date],
		isnull(sum(principal),0) principal,
		isnull(sum(interest),0) interest,
		isnull(sum(addInt),0) addInt,
		isnull(sum(procFee),0) procFee,
		isnull(sum(paidPrinc),0) paidPrinc,
		isnull(sum(paidInt),0) paidInt,
		isnull(sum(paidAddInt),0) paidAddInt,
		isnull(sum(paidProcFee),0) paidProcFee,
		isnull(sum(writtenOff), 0) as writtenOff,
		isnull(t.staffID, 0) as staffID,
		isnull(l.loanTenure, 0) as loanTenure,
		isnull(l.disbursementDate, getdate()) as disbursementDate ,
		isnull(max(amountPayable), 0.0) as amountPayable,
		isnull(max(amountPaid), 0.0) as amountPaid,
		isnull(max(cumPayable), 0.0) as cumPayable,
		isnull(max(cumPaid), 0.0) as cumPaid,
		isnull(dateadd(MM, loanTenure, l.disbursementDate), getDate()) as expiryDate,
		isnull(sum(paidPrinc + paidInt + paidAddInt), 0) as paid,
		isnull(sum(principal+interest+addInt), 0) as payable
	from
	(			
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			l.disbursementDate as [date],
			'Loan Approval' as [Description],
			lt.loanTypeName,
			isnull(case when lr.amountDisbursed is not null then lr.amountDisbursed else l.amountDisbursed end, 0 ) as principal,
			0 as interest,
			0 as addInt,
			0 as procFee,
			0 as paidPrinc,
			0 as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			1 as sortCode,
			l.staffID
		from  ln.loan l 
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			left join 
				(
					select loanID,
						sum(amountDisbursed) as amountDisbursed,
						max(disbursementDate) as disbursementDate,
						count(*) over (partition by loanID) as cnt
					from ln.loanTranch 
					where disbursementDate <= @ed
					group by loanID
				) lr on l.loanID=lr.loanID
		where isnull(lr.disbursementDate, l.disbursementDate)<=@ed
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			feeDate as [date],
			'Processing Fee' as [Description],
			lt.loanTypeName,
			0 as principal,
			0 as interest,
			0 as addInt,
			feeAmount as procFee,
			0 as paidPrinc,
			0 as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			2 as sortCode,
			l.staffID
		from ln.loan l 
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			left join ln.loanFee f on l.loanID = f.loanID
		where  (feeDate<=@ed) and feeTypeID = 1
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			lp.penaltyDate as [date],
			'Additional Interest' as [Description],
			lt.loanTypeName,
			0 as principal,
			0 as interest,
			lp.penaltyFee as addInt,
			0 as procFee,
			0 as paidPrinc,
			0 as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			3 as sortCode,
			l.staffID
		from ln.loanPenalty lp inner join ln.loan l on lp.loanID=l.loanID
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID 
		where penaltyDate <= @ed
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			l.disbursementDate as [date],
			'Interest on Loan' as [Description],
			lt.loanTypeName,
			0 as principal,
			case when cnt>1 then isnull(lr.amt, lp.interestPayment) else lp.interestPayment end as interest,
			0 as addInt,
			0 as procFee,
			0 as paidPrinc,
			0 as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			3 as sortCode,
			l.staffID
		from 
			(
				select 
					l.loanID,
					sum(interestPayment) as interestPayment
				from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
				where  ((l.loanTypeID<> 5 and l.disbursementDate <= @ed   ) or (l.loanTypeID=5 and (lp.repaymentDate <= @ed)))
				group by l.loanID
			)lp inner join ln.loan l on lp.loanID=l.loanID
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			left join 
			(
				select
					loanID, 
					sum(crdt_amt) as amt
				from ln.loan l inner join dbo.jnl j on l.loanNo=j.ref_no
				where tx_date <= @ed and description like 'Loan Disbursement Interest%'
					and tx_date >='2013-06-01'
					and description not like '%RVSL%'
				group by loanID
			) lr on l.loanID=lr.loanID  
			left join
			(
				select 
					lt.loanID,
					count(*) as cnt
				from ln.loanTranch lt inner join ln.loan l on lt.loanID=l.loanID
				where l.disbursementDate <=@ed
				group by lt.loanID
			) lt2 on l.loanID = lt2.loanID
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			lp.repaymentDate as [date],
			'Loan Repayment' as [Description],
			lt.loanTypeName,
			0 as principal,
			0 as interest,
			0 as addInt,
			0 as procFee,
			lp.principalPaid,
			0 as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			4 as sortCode,
			l.staffID
		from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
		where lp.repaymentDate <= @ed	and principalPaid>0
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			lp.repaymentDate as [date],
			'Loan Interest Repayment' as [Description],
			lt.loanTypeName,
			0 as principal,
			0 as interest,
			0 as addInt,
			0 as procFee,
			0 as paidPrinc,
			lp.interestPaid as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			4 as sortCode,
			l.staffID
		from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
		where  (lp.repaymentDate <= @ed  ) and (interestPaid<>0)
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			lp.repaymentDate as [date],
			'Processing Fee Payment' as [Description],
			lt.loanTypeName,
			0 as principal,
			0 as interest,
			0 as addInt,
			0 as procFee,
			0 as paidPrinc,
			0 as paidInt,
			0 as paidAddInt,
			 feePaid as paidProcFee,
			0 as writtenOff,
			4 as sortCode,
			l.staffID
		from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
		where (repaymentDate<=@ed) and repaymentTypeID=6
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			lp.repaymentDate as [date],
			'Additional Interest Payment' as [Description],	
			lt.loanTypeName,	
			0 as principal,
			0 as interest,
			0 as addInt,
			0 as procFee,
			0 as paidPrinc,
			0 as paidInt,
			lp.penaltyPaid as paidAddInt,
			0 as paidProcFee,
			0 as writtenOff,
			4 as sortCode,
			l.staffID
		from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
		where  (penaltyPaid>0 or lp.repaymentTypeID in (7))
		and repaymentDate<=@ed
		union all
		select
			c.clientID,
			case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
			c.accountNumber,
			l.loanID,
			l.loanNo,
			writeOffDate as [date],
			'Interest Write-off on Loan' as [Description],
			lt.loanTypeName,
			0 as principal,
			0 as interest,
			0 as addInt,
			0 as procFee,
			0 as paidPrinc,
			0 as paidInt,
			0 as paidAddInt,
			0 as paidProcFee,
			writeOffAmount as writtenOff,
			7 as sortCode,
			l.staffID
		from ln.loan l  
			inner join ln.client c on l.clientID = c.clientID
			inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			inner join ln.loanIterestWriteOff w on l.loanID=w.loanID
		where writeOffDate<=@ed 
	) t 
		inner join ln.loan l on t.loanID=l.loanID
		left outer join ln.GetvwLoans62('2000-01-01', @ed, 0, 1) l6 on l.loanId = l6.loanId
	group by 
		t.clientID,
		t.clientName,
		t.accountNumber,
		t.loanID,
		t.loanNo,
		t.loanTypeName,
		t.staffID,
		isnull(l.loanTenure, 0),
		l.disbursementDate,
		loanTenure
	having sum(principal)>0
	order by t.clientName, date
end
go


create view vwLoanBalanceReport
as
select
		0 clientID,
		'' clientName,
		'' accountNumber,
		0 loanID,
		'' loanNo,
		'' loanTypeName,
		getDate() as [date],
		0.0 principal,
		0.0  interest,
		0.0 addInt,
		0.0 procFee,
		0.0 paidPrinc,
		0.0 paidInt,
		0.0 paidAddInt,
		0.0 paidProcFee,
		0.0 as writtenOff,
		0 as staffID,
		0.0 loanTenure,
		getDate() as disbursementDate ,
		0.0  as amountPayable,
		0.0 amountPaid,
		0.0 cumPayable,
		0.0 cumPaid,
		getDate() as expiryDate,
		0.0 paid,
		0.0 payable
go
