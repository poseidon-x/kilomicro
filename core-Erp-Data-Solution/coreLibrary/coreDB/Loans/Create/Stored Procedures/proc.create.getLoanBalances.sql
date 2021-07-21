use coreDB
go

alter function ln.calcInterest 
(
	@loanID int,
	@closingDate datetime
) returns float
with encryption 
as
begin
	declare @int1 float, @int2 float

	select @int1 = isnull((datediff(day,  l.disbursementDate, @closingDate)/
		30.0) *(interestRate/100.0) *l.amountDisbursed,0)
	from ln.loan l
	where loanID=@loanID

	select @int2 = isnull(sum(interestPaid),0)
	from ln.loanRepayment
	where loanID=@loanID

	return round(@int1-@int2,2)
end
go


use coreDB
go

alter procedure ln.getLoanBalances
( 
	@clientID int,
	@closingDate datetime
)
as
begin
	SELECT * FROM 
	(
		select
			l.loanID,
			l.loanNo,
			isnull(l.invoiceNo,'') as invoiceNo,
			isnull(l.disbursementDate,getDate()) as disbursementDate,
			l.balance as principalOutstanding,
			isnull(case when isnull((select sum(feePaid) from ln.loanRepayment lr where lr.loanID=l.loanID and lr.repaymentTypeID=6),0)=0
				then isnull(max(l.processingFee),0) 
				else isnull(max(l.processingFeeBalance),0) end,0) as processingFee,
			isnull(case when isnull((select sum(feePaid) from ln.loanRepayment lr where lr.loanID=l.loanID),0)=0
				then isnull(max(l.processingFee),0) 
				else isnull(max(l.processingFeeBalance),0) end,0) as processingFeeBalance,
			isnull(ln.calcInterest(l.loanID, '2016-05-17'),0)
				+ ISNULL((SELECT SUM(pen.penaltybalance) FROM ln.loanPenalty pen WHERE lp.loanId=l.loanId),0) as interestOutstanding,
			isnull(ln.calcInterest(l.loanID, '2016-05-17'),0) + l.balance		
				+ ISNULL((SELECT SUM(pen.penaltybalance) FROM ln.loanPenalty pen WHERE lp.loanId=l.loanId),0)
			+isnull(case when isnull((select sum(feePaid) from ln.loanRepayment lr where lr.loanID=l.loanID and lr.repaymentTypeID=6),0)=0
				then isnull(max(l.processingFee),0) 
				else isnull(max(l.processingFeeBalance),0) end,0) as totalOutstanding,
			--0.0 as amountPaid,
			'' as [description], 
			s.supplierName,
			isnull(amountPayable, 0.0) as amountPayable,
			isnull(l6.amountPaid, 0.0) as amountPaid,
			isnull(cumPayable, 0.0) as cumPayable,
			isnull(cumPaid, 0.0) as cumPaid
			--isnull(dateadd(MM, loanTenure, l.disbursementDate), getDate()) as expiryDate
		from ln.loan l left outer join ln.loanRepayment rs on l.loanID=rs.loanID
			left outer join ln.loanPenalty lp on l.loanID=lp.loanID
			left outer join ln.invoiceLoan il on l.invoiceNo = il.invoiceNo
			left outer join ln.supplier s on il.supplierID=s.supplierID
			left outer join ln.getvwLoans62('2000-01-01', @closingDate, 0, 1) l6 on l.loanId = l6.loanId
		where l.clientID = @clientID
			and l.disbursementDate is not null
			and balance>0.05 
			and isnull(ln.calcInterest(l.loanID, @closingDate),0) >0
			--and ( (@supplierID > 0 and il.supplierID = @supplierID ) or @supplierID is null or @supplierID <=0)
		group by 
			l.loanID,
			l.loanNo,
			isnull(l.invoiceNo,''),
			isnull(l.disbursementDate,getDate()),
			l.balance, 
			s.supplierName,
			l6.amountPaid,
			lp.loanID,
			isnull(amountPayable, 0.0), 
			isnull(cumPayable, 0.0),
			isnull(cumPaid, 0.0)
			--isnull(dateadd(MM, loanTenure, l.disbursementDate), getDate())
	) T
	WHERE  totalOutstanding > 5
end
go


alter view ln.vwLoanBalances 
with encryption
as 
select
		l.loanID,
		l.loanNo,
		invoiceNo,
		isnull(l.disbursementDate,getDate()) as disbursementDate,
		l.balance as principalOutstanding,
		l.processingFee,
		isnull(round(datediff(day, isnull(max(repaymentDate), l.disbursementDate), getdate())/
		30.0 *interestRate/100.0 *l.balance,2),0) as interestOutstanding,
		isnull(round(datediff(day, isnull(max(repaymentDate), l.disbursementDate), getdate())/
		30.0 *interestRate/100.0 *l.balance + l.balance,2),0) as totalOutstanding,
		0.0 as amountPaid,
		'' as [description],
		isnull(0, 0.0) as amountPayable,
		isnull(0, 0.0) as amountPaid2,
		isnull(0, 0.0) as cumPayable,
		isnull(0, 0.0) as cumPaid,
		isnull(dateadd(MM, loanTenure, l.disbursementDate), getDate()) as expiryDate
	from ln.loan l left outer join ln.loanRepayment rs on l.loanID=rs.loanID
	where   l.disbursementDate is not null
	group by 
		l.loanID,
		l.loanNo,
		l.invoiceNo,
		l.disbursementDate,
		l.processingFee,
		l.balance,
		l.interestRate,
		l.repaymentModeID,
		l.loanTenure
	having round(datediff(day, isnull(max(repaymentDate), l.disbursementDate), getdate())/
		cast(repaymentModeID as float) *interestRate/100.0 *l.balance + l.balance,2) >0

go
