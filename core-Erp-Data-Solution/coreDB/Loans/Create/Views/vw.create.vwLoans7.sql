use coreDB
go

alter view ln.vwLoans7
with encryption as 
select
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	cat.categoryName,
	l.amountDisbursed,
	l.processingFee,
	l.processingFeeBalance,
	l.loanNo,
	isnull(l.disbursementDate, l.applicationDate) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select top 1 lc.fairValue from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull((select sum(interestBalance+principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as balance,
	isnull((select sum(principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principal,
	isnull((select sum(interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interest,
	isnull((select sum(interestPaid+principalPaid) from ln.loanRepayment rs where rs.loanID=l.loanID),0) as paid,
	isnull((select sum(principalPaid) from ln.loanRepayment rs where rs.loanID=l.loanID),0) as paidPrinc,
	isnull((select sum(interestPaid) from ln.loanRepayment rs where rs.loanID=l.loanID),0) as paidInt,
	isnull((select sum(penaltyPaid) from ln.loanRepayment rs where rs.loanID=l.loanID ),0)  as paidPenalty,
	isnull((select sum(feePaid) from ln.loanRepayment rs where rs.loanID=l.loanID),0)  as paidFee,
		isnull((select sum(penaltyFee) from ln.loanPenalty rs where rs.loanID=l.loanID ),0) as penalty,
	isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interestBalance,
	isnull((select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principalBalance,
	isnull((select sum(interestWritenOff) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as writtenOff,
	isnull(datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()),0) as daysDue,
	isnull((select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID),applicationDate) as loanEndDate,
	isnull((select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID),applicationDate) as lastPaymentDate,
	isnull(
			(select avg(datediff(day, t1.repaymentDate, t2.repaymentDate)) from
			(select repaymentDate, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t1,			
			(select repaymentDate, row_number() over (order by repaymentDate) as rn from ln.loanRepayment rs where rs.loanID=l.loanID) t2
			where t1.rn=t2.rn)
		,9999) as repaymentDateDelta,
	isnull(
			(select avg(((t1.payment- t2.payment)/t1.payment)*100.0) from
			(select principalPayment+interestPayment as payment, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t1,			
			(select principalPayment+interestPayment-principalBalance-interestBalance as payment, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t2
			where t1.rn=t2.rn)
		,9999) as repaymentAmountDelta
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
where l.disbursementDate is not null

go

alter proc ln.getvwLoans7
(@endDate DATETIME)
with encryption as 
	declare @ed datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
select
	l.loanID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	t.loanTypeName categoryName,
	l.amountDisbursed,
	isnull(t.procFee,0) as processingFee,
	isnull(t.procFee-t.paidProcFee,0) processingFeeBalance,
	l.loanNo,
	isnull(l.disbursementDate, l.applicationDate) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select top 1 lc.fairValue from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull((select sum(interestBalance+principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID ),0) as balance,
	isnull(t.principal,0) as principal,
	isnull(t.interest,0) as interest,
	isnull(t.paidAddInt+t.paidInt+t.paidPrinc+t.paidProcFee,0) as paid,
	isnull(t.paidPrinc,0) as paidPrinc,
	isnull(t.paidInt,0) as paidInt,
	isnull(t.paidAddInt,0)  as paidPenalty,
	isnull(t.paidProcFee,0)  as paidFee,
		isnull(t.addInt,0) as penalty,
	isnull(t.interest-t.paidInt,0) as interestBalance,
	isnull(t.writtenOff,0) as writtenOff,
	isnull(t.principal-t.paidPrinc,0) as principalBalance,
	isnull(datediff(DAY, (select min(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and (interestBalance>0 or principalBalance>0)), getdate()),0) as daysDue,
	isnull((select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID),applicationDate) as loanEndDate,
	isnull((select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID and repaymentDate <= @ed),applicationDate) as lastPaymentDate,
	isnull(
			(select avg(datediff(day, t1.repaymentDate, t2.repaymentDate)) from
			(select repaymentDate, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t1,			
			(select repaymentDate, row_number() over (order by repaymentDate) as rn from ln.loanRepayment rs where rs.loanID=l.loanID) t2
			where t1.rn=t2.rn)
		,9999) as repaymentDateDelta,
	isnull(
			(select avg(((t1.payment- t2.payment)/t1.payment)*100.0) from
			(select principalPayment+interestPayment as payment, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t1,			
			(select principalPayment+interestPayment-principalBalance-interestBalance as payment, row_number() over (order by repaymentDate) as rn from ln.repaymentSchedule rs where rs.loanID=l.loanID) t2
			where t1.rn=t2.rn)
		,9999) as repaymentAmountDelta
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join
	(
		select
			clientID,
			clientName,
			accountNumber,
			loanID,
			loanNo,
			loanTypeName,
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
			isnull(staffID, 0) as staffID	
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
						where  ((l.loanTypeID<> 5 and l.disbursementDate <= @ed   ) or (l.loanTypeID=5 and lp.repaymentDate <= @ed))
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
				where  (lp.repaymentDate <= @ed  ) and (interestPaid>0)
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
				where (repaymentDate<=@ed) 
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
		group by 
			clientID,
			clientName,
			accountNumber,
			loanID,
			loanNo,
			loanTypeName,
			staffID 
	) t on t.loanID=l.loanID
where l.disbursementDate <= @ed

go
 