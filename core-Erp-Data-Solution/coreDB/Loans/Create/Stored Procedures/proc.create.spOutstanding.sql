use coreDB
go


alter procedure spOutstanding
(
    @endDate datetime
)
with encryption as
select	
	isnull(c.clientID, 0) as clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	isnull(l.loanID, 0) as loanID,
	isnull(l.loanNo,'') as loanNo,
	isnull(isnull(l.disbursementDate, @endDate), getDate()) as disbursementDate,
	isnull(sum(lc.fairValue),0) as fairValue,
	isnull(sum(principalBalance),0) as principalBalance,
	isnull(sum(interestBalance),0) as interestBalance,
	isnull(max(repaymentDate), getDate()) as repaymentDate,
	isnull(max(amountDisbursed),0) as amountDisbursed,
	isnull((select sum(principalPaid+interestPaid+feePaid+penaltyPaid)  from ln.loanRepayment lr where  lr.loanID = l.loanID and lr.repaymentDate<=@endDate),0) as totalPaid,
	isnull((select sum(feePaid)  from ln.loanRepayment lr where  lr.loanID = l.loanID and lr.repaymentDate<=@endDate),0) as feePaid,
	isnull((select sum(penaltyPaid)  from ln.loanRepayment lr where  lr.loanID = l.loanID and lr.repaymentDate<=@endDate),0) as penaltyPaid,
	isnull((select sum(penaltyFee)  from ln.loanPenalty lr where  lr.loanID = l.loanID and lr.penaltyDate<=@endDate),0) as penalty,
	isnull(
            isnull((select sum(feeAmount)  from ln.loanFee lr where  lr.loanID = l.loanID and lr.feeDate<=@endDate),0)
            + isnull((select sum(amount)  from ln.loanInsurance lr where  lr.loanID = l.loanID and lr.insuranceDate<=@endDate),0)
        ,0) as fee,
	isnull((select sum(writeOffAmount)  from ln.loanIterestWriteOff lr where  lr.loanID = l.loanID),0) as writtenOff,
	isnull(isnull((select sum(interestPayment)  from ln.repaymentSchedule lr where  lr.loanID = l.loanID),0)
	+ isnull((select sum(penaltyFee)  from ln.loanPenalty lr where  lr.loanID = l.loanID and lr.penaltyDate<=@endDate),0),0) as interest,
	isnull(ln.getDaysDue(l.loanID, @endDate),0) as daysDue,
	isnull(isnull(max(repaymentDate), @endDate),getDate()) as expiryDate,
	isnull(lg.loangroupname, '') as LoanGroupName, 
	isnull(isnull(lg.relationsOfficerStaffId,l.staffID), -1) as StaffID
	--isnull(c.companyId,0)as CompanyId,
	--isnull(company.comp_Name, '') AS comp_Name 
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	left join
	(
		select
			lc.loanID,
			sum(lc.fairValue) as fairValue
		from ln.loanCollateral lc
		group by lc.loanID
	) lc on l.loanID=lc.loanID
	left join ln.repaymentSchedule rs on l.loanID=rs.loanID
	left join ln.loangroupclient lgc on c.clientid = lgc.clientid
	left join ln.loangroup lg on lgc.loangroupid = lg.loangroupid
	--left join dbo.comp_prof AS company on c.companyId = company.companyId

where l.loanStatusId=4 and l.disbursementDate <= @endDate
group by 
	isnull(l.loanNo,''),
	isnull(c.clientID, 0),
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,
	c.accountNumber,
	l.loanID,
    isnull(l.disbursementDate, @endDate),
	isnull(lg.loangroupname, ''),
	amountDisbursed, 
	isnull(isnull(lg.relationsOfficerStaffId,l.staffID), -1)
	--isnull(l.staffID, -1)
	--isnull(c.companyId,0),
	--isnull(company.comp_Name, '')
Having (amountDisbursed + isnull(isnull((select sum(interestPayment)  from ln.repaymentSchedule lr where  lr.loanID = l.loanID),0)
	+ isnull((select sum(penaltyFee)  from ln.loanPenalty lr where  lr.loanID = l.loanID and lr.penaltyDate<=@endDate),0),0) + isnull(
            isnull((select sum(feeAmount)  from ln.loanFee lr where  lr.loanID = l.loanID and lr.feeDate<=@endDate),0)
            + isnull((select sum(amount)  from ln.loanInsurance lr where  lr.loanID = l.loanID and lr.insuranceDate<=@endDate),0)
        ,0) - (isnull((select sum(principalPaid+interestPaid+feePaid+penaltyPaid)  from ln.loanRepayment lr where  lr.loanID = l.loanID and lr.repaymentDate<=@endDate),0))) > 4;