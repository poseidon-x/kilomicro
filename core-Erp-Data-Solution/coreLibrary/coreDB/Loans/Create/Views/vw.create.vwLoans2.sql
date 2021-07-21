use coreDB
go
															 
alter view ln.vwLoans2 
with encryption as 
select
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	t.loanTypeName as categoryName,
	l.amountDisbursed,
	lt.loanTypeName, 
	l.loanID,
	isnull(l.loanNo, '') loanNo,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select sum(lc.fairValue) from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull(t.interest-t.paidInt-t.writtenOff,0) as interestBalance,
	isnull(t.principal-t.paidPrinc,0) as principalBalance,
	isnull(t.interest,0) as interestPayment,
	isnull(t.principal,0) as principalPayment,
	case when daysDue<0 then 0 else daysDue end daysDue,
	isnull(t.procFee,0) as processingFee,
	isnull(dateadd(MM,loanTenure, l.disbursementDate), getdate()) as expiryDate,
	isnull(cast(0 as float),0) as provisionAmount,
	isnull(s.sectorName, '') as sectorName,
	isnull(ln.getAmountDue(l.loanID, '2100-01-01'), 0) as amountDue,
	isnull(t.principal-t.paidPrinc+t.interest-t.paidInt-t.writtenOff, 0) as totalBalance,
	isnull(cumPayable, 0.0) as amountPayable,
	isnull(amountPaid, 0.0) as amountPaid,
	isnull(cumPaid, 0.0) as cumPayable,
	isnull(cumPaid, 0.0) as cumPaid,
	isnull(c.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name 
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left join dbo.comp_prof AS company on c.companyId = company.companyId

	inner join  
		(
			select
				l.loanID,
				ln.getDaysDue(loanID, getdate()) as daysDue,
				isnull((select sum(principalPaid+interestPaid) from ln.loanRepayment lr where lr.loanID=l.loanID and repaymentDate<= getdate()), 0) as paid,
				isnull((select sum(principalPayment) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as principal,
				case when disbursementDate > dateadd(MM,-6,getdate()) then 1 else 0 end as inc
			from ln.loan l  
		) q on l.loanID = q.loanID
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
				isnull(lr.amountDisbursed, l.amountDisbursed) as principal,
				0 as interest,
				0 as addInt,
				0 as procFee,
				0 as paidPrinc,
				0 as paidInt,
				0 as paidAddInt,
				0 as paidProcFee,
				0 as writtenOff,
				1 as sortCode,
				l.staffID,
				isnull(lt.companyId,0)as CompanyId,
	            isnull(company.comp_Name, '') AS comp_Name
			from  ln.loan l 
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
				left join ln.loanTranch lr on l.loanID=lr.loanID
				left join dbo.comp_prof AS company on lt.companyId = company.companyId

			where isnull(lr.disbursementDate, l.disbursementDate)<=getdate()
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
			where feeDate<=getdate() and feeTypeID = 1
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
			where penaltyDate <= getdate()
			union all
			select
				c.clientID,
				case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
				c.accountNumber,
				l.loanID,
				l.loanNo,
				lp.repaymentDate as [date],
				'Interest on Loan' as [Description],
				lt.loanTypeName,
				0 as principal,
				lp.interestPayment as interest,
				0 as addInt,
				0 as procFee,
				0 as paidPrinc,
				0 as paidInt,
				0 as paidAddInt,
				0 as paidProcFee,
				0 as writtenOff,
				3 as sortCode,
				l.staffID
			from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
				left join 
				(
					select
						loanID, 
						max(disbursementDate) as disbursementDate
					from ln.loanTranch
					where disbursementDate <= getdate()
					group by loanID
				) lr on l.loanID=lr.loanID 
			where (lr.disbursementDate is null or repaymentDate >= lr.disbursementDate) 
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
			where  (interestPaid>0)
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
				case when feePaid=0 then amountPaid else feePaid end as paidProcFee,
				0 as writtenOff,
				4 as sortCode,
				l.staffID
			from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			where  disbursementDate<=getdate() and (repaymentTypeID=6 or lp.feePaid>0)
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
	left outer join ln.sector s on s.sectorID=c.sectorID
	left outer join ln.GetvwLoans62('2000-01-01', '2100-01-01', 0, 1) l6 on l.loanId = l6.loanId
where l.disbursementDate is not null
		and l.disbursementDate <= getdate() 

go


alter view ln.vwApprovedUndisbured 
with encryption as 
select
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	cat.categoryName,
	l.amountDisbursed,
	lt.loanTypeName, 
	isnull(l.amountApproved,0) amountApproved,
	isnull(l.finalApprovalDate,getdate()) finalApprovalDate,
	isnull(l.applicationDate,getdate()) applicationDate,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select top 1 lc.fairValue from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interestBalance,
	isnull((select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principalBalance,
	isnull((select sum(interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interestPayment,
	isnull((select sum(principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principalPayment,
		isnull(datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= getdate()), 
			isnull((select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID),getdate())),0) as daysDue,
	isnull(l.processingFee,0) as processingFee
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left outer join ln.sector s on s.sectorID=c.sectorID
where l.loanStatusID = 3

go

alter proc ln.GetvwLoans2 
(	
	@endDate datetime
)
with encryption as 
	declare @ed datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
select
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	t.loanTypeName as categoryName,
	l.amountDisbursed,
	lt.loanTypeName, 
	l.loanID,
	isnull(l.loanNo, '') loanNo,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select sum(lc.fairValue) from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull(t.interest-t.paidInt-t.writtenOff,0) as interestBalance,
	isnull(t.principal-t.paidPrinc,0) as principalBalance,
	isnull(t.interest,0) as interestPayment,
	isnull(t.principal,0) as principalPayment,
	case when daysDue<0 then 0 else daysDue end daysDue,
	isnull(t.procFee,0) as processingFee,
	isnull(dateadd(MM,loanTenure, disbursementDate), getdate()) as expiryDate,
	isnull(cast(0 as float),0) as provisionAmount,
	isnull(s.sectorName, '') as sectorName,
	isnull(ln.getAmountDue(l.loanID, getDate()), 0) as amountDue,
	isnull(t.principal-t.paidPrinc+t.interest-t.paidInt-t.writtenOff, 0) as totalBalance,
	isnull(cast(0.0 as float), 0.0) as amountPayable,
	isnull(cast(0.0 as float), 0.0) as amountPaid,
	isnull(cast(0.0 as float), 0.0) as cumPayable,
	isnull(cast(0.0 as float), 0.0) as cumPaid
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	inner join  
		(
			select
				l.loanID,
				ln.getDaysDue(loanID, @endDate) as daysDue,
				isnull((select sum(principalPaid+interestPaid) from ln.loanRepayment lr where lr.loanID=l.loanID and repaymentDate<= @endDate), 0) as paid,
				isnull((select sum(principalPayment) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as principal,
				case when disbursementDate > dateadd(MM,-6,@endDate) then 1 else 0 end as inc
			from ln.loan l  
		) q on l.loanID = q.loanID
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
				isnull(lr.amountDisbursed, l.amountDisbursed) as principal,
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
				left join ln.loanTranch lr on l.loanID=lr.loanID
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
			where feeDate<=@ed and feeTypeID = 1
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
				lp.repaymentDate as [date],
				'Interest on Loan' as [Description],
				lt.loanTypeName,
				0 as principal,
				lp.interestPayment as interest,
				0 as addInt,
				0 as procFee,
				0 as paidPrinc,
				0 as paidInt,
				0 as paidAddInt,
				0 as paidProcFee,
				0 as writtenOff,
				3 as sortCode,
				l.staffID
			from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
				left join 
				(
					select
						loanID, 
						max(disbursementDate) as disbursementDate
					from ln.loanTranch
					where disbursementDate <= @ed
					group by loanID
				) lr on l.loanID=lr.loanID 
			where (lr.disbursementDate is null or repaymentDate >= lr.disbursementDate)
				and ((l.loanTypeID<> 5 and l.disbursementDate <= @ed   ) or (l.loanTypeID=5 and lp.repaymentDate <= @ed))
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
				case when feePaid=0 then amountPaid else feePaid end as paidProcFee,
				0 as writtenOff,
				4 as sortCode,
				l.staffID
			from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			where  disbursementDate<=@ed and (repaymentTypeID=6 or lp.feePaid>0)
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
	left outer join ln.sector s on s.sectorID=c.sectorID
where l.disbursementDate is not null
		and disbursementDate <= @endDate
		and isnull(t.interest-t.paidInt-t.writtenOff,0) + isnull(t.principal-t.paidPrinc,0)>0

go

alter function ln.getDaysDue(
	@loanID int,
	@endDate datetime
)
returns int
with encryption
as
begin
	declare @daysDue int
	declare @sd datetime, @ed datetime, @disbDate datetime, @matDate  datetime, @rpID int, @tenure float
	
	select @rpID = max(repaymentModeID), @tenure=max(loanTenure), @disbDate=max(disbursementDate)
	from ln.loan lr where lr.loanID=@loanID
	 
	select @matDate = dateadd(MM, @tenure, @disbDate) 

	select @ed = max(repaymentDate)
	from ln.loanRepayment lr where lr.loanID=@loanID and repaymentDate <= @endDate  AND (principalPaid > 0 or interestPaid>0)
	if /*@matDate< @endDate and*/ @ed is not null
	begin
		select @sd=@ed
		select @ed=@endDate		
	end
	else if @ed is not null 
	begin
		select @sd=min(repaymentDate)
		from ln.repaymentSchedule lr where lr.loanID=@loanID and repaymentDate <= @ed 
		if @sd is null
		begin
			select @sd=@ed
			select @ed=@endDate
		end
	end
	else
	begin
		select @ed=@endDate
		select @sd=min(repaymentDate)
		from ln.repaymentSchedule lr where lr.loanID=@loanID and repaymentDate <= @endDate
		if @sd is null
		begin
			select @sd=@ed
			select @ed=@endDate
		end 
	end 

	select @daysDue = datediff(DD, @sd, @ed)

	return @daysDue
end
go

alter function ln.getAmountDue(
	@loanID int,
	@endDate datetime
)
returns int
with encryption
as
begin
	declare @due float, @proc float, @pen float
	  
	select @due = sum(principalBalance+interestBalance)
	from ln.repaymentSchedule
	where loanID=@loanID and repaymentDate<=@endDate

	set @due=isnull(@due,0)

	select @proc = sum(processingFeeBalance)
	from ln.loan
	where loanID = @loanID

	set @proc=isnull(@proc,0)

	select @pen=sum(penaltyBalance)
	from ln.loanPenalty
	where loanID = @loanID

	set @pen=isnull(@pen,0)

	return @due+@proc+@pen
end
go

alter view ln.vwUndisbured 
with encryption as 
select
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	cat.categoryName,
	l.amountDisbursed,
	l.loanID,
	lt.loanTypeName, 
	isnull(amountRequested,0) as amountRequested,
	isnull(l.amountApproved,0) amountApproved,
	isnull(l.finalApprovalDate,getdate()) finalApprovalDate,
	isnull(l.applicationDate,getdate()) applicationDate,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select top 1 lc.fairValue from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull((select sum(interestBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interestBalance,
	isnull((select sum(principalBalance) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principalBalance,
	isnull((select sum(interestPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as interestPayment,
	isnull((select sum(principalPayment) from ln.repaymentSchedule rs where rs.loanID=l.loanID),0) as principalPayment,
		isnull(datediff(DAY, (select max(repaymentDate) from ln.repaymentSchedule rs where rs.loanID=l.loanID and repaymentDate <= getdate()), 
			isnull((select max(repaymentDate) from ln.loanRepayment rs where rs.loanID=l.loanID),getdate())),0) as daysDue,
	isnull(l.processingFee,0) as processingFee,
	isnull(datediff(dd, applicationDate,getdate()),0) as applicationDaysDelta,
	isnull(l.staffID,0) as staffID,
	isnull(s.sectorName, '') as sectorName
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	left outer join ln.sector s on s.sectorID=c.sectorID
where disbursementDate is null


go



alter proc ln.GetvwLoans21 
(	
	@endDate datetime
)
with encryption as 
	declare @ed datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
select
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName as categoryName,
	l.amountDisbursed,
	lt.loanTypeName, 
	l.loanID,
	isnull(l.loanNo, '') loanNo,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull(typeOfSecurity,'') as collateralType,
	isnull(securityValue,0) as collateralValue,
	isnull(interestBalance,0) interestBalance,
	isnull(principalBalance,0) principalBalance,
	cast(isnull(0.0,0) as float) as interestPayment,
	cast(isnull(0.0,0) as float) as principalPayment,
	isnull(daysDue,0) daysDue,
	cast(isnull(0.0,0) as float) as processingFee,
	dateadd(MM,loanTenure, disbursementDate) as expiryDate,
	isnull(case when proposedAmount=0 then provisionAmount else proposedAmount end,0) provisionAmount,
	isnull(s.sectorName, '') as sectorName,
	isnull(ln.getAmountDue(l.loanID, @ed), 0) as amountDue,
	isnull(principalBalance+interestBalance, 0) as totalBalance,
	isnull(cast(0.0 as float), 0.0) as amountPayable,
	isnull(cast(0.0 as float), 0.0) as amountPaid,
	isnull(cast(0.0 as float), 0.0) as cumPayable,
	isnull(cast(0.0 as float), 0.0) as cumPaid
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID 
	left join ln.loanProvision t on t.loanID=l.loanID
	left outer join ln.sector s on s.sectorID=c.sectorID
where l.disbursementDate is not null 
		and disbursementDate <= @ed
		and provisionDate = @ed 

go

alter proc ln.GetvwLoans22 
(	
	@endDate datetime
)
with encryption as 
	declare @ed datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
select
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName as categoryName,
	l.amountDisbursed,
	lt.loanTypeName, 
	l.loanID,
	isnull(l.loanNo, '') loanNo,
	isnull(l.disbursementDate, getdate()) as disbursementDate, 
	isnull(ln.getDaysDue(l.loanID, dateadd(MM,-1,@ed)),0) daysDue, 
	isnull(dateadd(MM,loanTenure, disbursementDate),getdate()) as expiryDate, 
	isnull(s.sectorName, '') as sectorName,
	isnull(sum(case when t.repaymentDate between dateadd(MM,-1,@ed) and @ed then principalPaid else 0 end), 0) as princPaid,
	isnull(max(case when l.disbursementDate between dateadd(MM,-1,@ed) and @ed then l.amountDisbursed else 0 end), 0) as disbursed,
	isnull(cast(0.0 as float), 0.0) as amountPayable,
	isnull(cast(0.0 as float), 0.0) as amountPaid,
	isnull(cast(0.0 as float), 0.0) as cumPayable,
	isnull(cast(0.0 as float), 0.0) as cumPaid
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID 
	left join ln.loanRepayment t on t.loanID=l.loanID
	left outer join ln.sector s on s.sectorID=c.sectorID
where l.disbursementDate is not null
		and ((l.disbursementDate between dateadd(MM,-1,@ed) and @ed )
			or (t.repaymentDate between dateadd(MM,-1,@ed) and @ed))
group by 
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,
	c.accountNumber,
	c.clientID,
	lt.loanTypeName,
	l.amountDisbursed,
	lt.loanTypeName, 
	l.loanID,
	l.loanNo,
	isnull(l.disbursementDate, getdate()), 
	isnull(ln.getDaysDue(l.loanID, dateadd(MM,-1,@ed)),0), 
	dateadd(MM,loanTenure, disbursementDate), 
	isnull(s.sectorName, '')

go

alter proc ln.GetvwLoans23
(	
	@endDate datetime,
	@n int
)
with encryption as 
	declare @ed datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
select 
	isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,'')  as clientName,
	isnull(c.accountNumber, '') accountNumber,
	isnull(c.clientID, 0) clientID,
	isnull(t.loanTypeName, '') as categoryName,
	isnull(l.amountDisbursed,0) amountDisbursed,
	isnull(lt.loanTypeName,'') loanTypeName, 
	isnull(l.loanID,0) loanID,
	isnull(l.loanNo, '') loanNo,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select sum(lc.fairValue) from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull(t.interest-t.paidInt-t.writtenOff,0) as interestBalance,
	isnull(t.principalBalance,0) + case when isnull(t.interest-t.paidInt-t.writtenOff,0)<0 then isnull(t.interest-t.paidInt-t.writtenOff,0) else 0 end as principalBalance,
	isnull(t.interest,0) as interestPayment,
	isnull(t.principal,0) as principalPayment,
	isnull(case when q.daysDue is null then ln.getDaysDue(l.loanID, @endDate) when q.daysDue<0 then 0 else q.daysDue end,0) daysDue,
	isnull(t.procFee,0) as processingFee,
	isnull(dateadd(MM,loanTenure, l.disbursementDate), getdate()) as expiryDate,
	isnull(cast(0 as float),0) as provisionAmount,
	isnull(s.sectorName, '') as sectorName,
	isnull(amountDue, 0) as amountDue, 
	isnull(amountPayable, 0.0) as amountPayable,
	isnull(amountPaid, 0.0) as amountPaid,
	isnull(cumPayable, 0.0) as cumPayable,
	isnull(cumPaid, 0.0) as cumPaid
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	inner join  
		(
			select
				l.loanID,
				ln.getDaysDue(loanID, @endDate) as daysDue,
				isnull((select sum(principalPaid+interestPaid) from ln.loanRepayment lr where lr.loanID=l.loanID and repaymentDate<= @endDate), 0) as paid,
				isnull((select sum(principalPayment) from ln.repaymentSchedule lr where lr.loanID=l.loanID), 0) as principal,
				case when disbursementDate > dateadd(MM,-6,@endDate) then 1 else 0 end as inc,
				isnull(ln.getAmountDue(l.loanID, @ed),0) as amountDue
			from ln.loan l  
		) q on l.loanID = q.loanID
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
			isnull(sum(principal)-isnull(sum(paidPrinc), 0),0) principalBalance,
			isnull(sum(interest)-isnull(sum(paidInt), 0)-isnull(sum(writtenOff), 0),0) interestBalance,
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
				isnull(lr.amountDisbursed, l.amountDisbursed) as principal,
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
				left join ln.loanTranch lr on l.loanID=lr.loanID
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
			where feeDate<=@ed and feeTypeID = 1
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
				lp.repaymentDate as [date],
				'Interest on Loan' as [Description],
				lt.loanTypeName,
				0 as principal,
				lp.interestPayment as interest,
				0 as addInt,
				0 as procFee,
				0 as paidPrinc,
				0 as paidInt,
				0 as paidAddInt,
				0 as paidProcFee,
				0 as writtenOff,
				3 as sortCode,
				l.staffID
			from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
				left join 
				(
					select
						loanID, 
						max(disbursementDate) as disbursementDate
					from ln.loanTranch
					where disbursementDate <= @ed
					group by loanID
				) lr on l.loanID=lr.loanID 
			where (lr.disbursementDate is null or repaymentDate >= lr.disbursementDate)
				and ((l.loanTypeID<> 5 and l.disbursementDate <= @ed   ) or (l.loanTypeID=5 and lp.repaymentDate <= @ed))
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
				case when feePaid=0 then amountPaid else feePaid end as paidProcFee,
				0 as writtenOff,
				4 as sortCode,
				l.staffID
			from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			where  disbursementDate<=@ed and (repaymentTypeID=6 or lp.feePaid>0)
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
	left outer join ln.sector s on s.sectorID=c.sectorID  
	left outer join ln.GetvwLoans62('2000-01-01', @ed, 0, 1) l6 on l.loanId = l6.loanId
where l.disbursementDate is not null 

go


alter proc ln.GetvwLoans24
(	
	@endDate datetime,
	@n int
)
with encryption as 
	declare @ed datetime
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
select 
	isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,'')  as clientName,
	isnull(c.accountNumber, '') accountNumber,
	isnull(c.clientID, 0) clientID,
	isnull(t.loanTypeName, '') as categoryName,
	isnull(l.amountDisbursed,0) amountDisbursed,
	isnull(lt.loanTypeName,'') loanTypeName, 
	isnull(l.loanID,0) loanID,
	isnull(l.loanNo, '') loanNo,
	isnull(l.disbursementDate, getdate()) as disbursementDate,
	isnull((select top 1 ct.collateralTypeName from ln.loanCollateral lc inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
		 where lc.loanID=l.loanID),0) as collateralType,
	isnull((select sum(lc.fairValue) from ln.loanCollateral lc 
		 where lc.loanID=l.loanID),0) as collateralValue,
	isnull(t.interest-t.paidInt-t.writtenOff,0) as interestBalance,
	isnull(t.principal-t.paidPrinc,0)  as principalBalance,
	isnull(t.interest,0) as interestPayment,
	isnull(t.principal,0) as principalPayment,
	isnull(datediff(day, dateadd(MM, l.loantenure, disbursementDate), @endDate),0) daysDue,
	isnull(t.procFee,0) as processingFee,
	isnull(dateadd(MM,loanTenure, disbursementDate), getdate()) as expiryDate,
	isnull(cast(0 as float),0) as provisionAmount,
	isnull(s.sectorName, '') as sectorName,
	isnull(isnull(t.interest-t.paidInt-t.writtenOff,0) + isnull(t.principal-t.paidPrinc,0), 0) as amountDue,
	isnull(cast(0.0 as float), 0.0) as amountPayable,
	isnull(cast(0.0 as float), 0.0) as amountPaid,
	isnull(cast(0.0 as float), 0.0) as cumPayable,
	isnull(cast(0.0 as float), 0.0) as cumPaid
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID 
	left outer join ln.sector s on s.sectorID=c.sectorID
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
				isnull(lr.amountDisbursed, l.amountDisbursed) as principal,
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
				left join ln.loanTranch lr on l.loanID=lr.loanID
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
			where feeDate<=@ed and feeTypeID = 1
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
				lp.repaymentDate as [date],
				'Interest on Loan' as [Description],
				lt.loanTypeName,
				0 as principal,
				lp.interestPayment as interest,
				0 as addInt,
				0 as procFee,
				0 as paidPrinc,
				0 as paidInt,
				0 as paidAddInt,
				0 as paidProcFee,
				0 as writtenOff,
				3 as sortCode,
				l.staffID
			from ln.repaymentSchedule lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
				left join 
				(
					select
						loanID, 
						max(disbursementDate) as disbursementDate
					from ln.loanTranch
					where disbursementDate <= @ed
					group by loanID
				) lr on l.loanID=lr.loanID 
			where (lr.disbursementDate is null or repaymentDate >= lr.disbursementDate)
				and ((l.loanTypeID<> 5 and l.disbursementDate <= @ed   ) or (l.loanTypeID=5 and lp.repaymentDate <= @ed))
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
				case when feePaid=0 then amountPaid else feePaid end as paidProcFee,
				0 as writtenOff,
				4 as sortCode,
				l.staffID
			from ln.loanRepayment lp inner join ln.loan l on lp.loanID=l.loanID
				inner join ln.client c on l.clientID = c.clientID
				inner join ln.loanType lt on l.loanTypeID=lt.loanTypeID
			where  disbursementDate<=@ed and (repaymentTypeID=6 or lp.feePaid>0)
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
where l.disbursementDate is not null
go
