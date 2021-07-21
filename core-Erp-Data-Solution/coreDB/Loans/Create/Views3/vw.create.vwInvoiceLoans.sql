use coreDB
go

alter view ln.vwInvoiceLoanMaster
as
	select 
		isnull(c.clientID, 0) as clientID,
		isnull(c.clientName, '') as clientName,
		isnull(c.accountNumber, '') as accountNumber,
		isnull(addressLine1,'') addressLine1,
		isnull(workPhone,'') workPhone,
		isnull(mobilePhone,'') mobilePhone,
		isnull(homePhone,'') homePhone,
		isnull(addressLine2,'') addressLine2,
		isnull(cityTown,'') cityTown,
		isnull(ivm.supplierID, 0) as supplierID,
		isnull(s.supplierName, '') as supplierName,
		isnull(ivm.ceilRate, 0) as ceilRate,
		isnull(ivm.[approved], 0) as [approved],
		isnull(ivm.[disbursed], 0) as [disbursed],
		isnull(iv.[checkAmount], 0) as [checkAmount],
		isnull(ivm.[approvedBy], '') as [approvedBy],
		isnull(ivm.[approvalDate], '2000-01-01') as [approvalDate],
		isnull(ivm.[invoiceDate], '2000-01-01') as [invoiceDate],
		isnull(l.disbursementDate, '2000-01-01') as disbursementDate,
		isnull(iv.[invoiceAmount], 0) as [invoiceAmount],
		isnull(iv.[withHoldingTax], 0) as [withHoldingTax],
		isnull(iv.[amountDisbursed], 0) as [amountDisbursed],
		isnull(iv.[interestAmount], 0) as [interestAmount],
		isnull(iv.[processingFee], 0) as [processingFee],
		isnull(iv.[proposedAmount], 0) as [proposedAmount],
		isnull(iv.[rate], 0) as [rate],
		isnull(iv.[invoiceDescription], '') as [invoiceDescription],
		isnull(iv.[invoiceNo], '') as [invoiceNo],
		isnull(iv.[invoiceLoanMasterID], 0) as [invoiceLoanMasterID],
		isnull(iv.[invoiceLoanID], 0) as [invoiceLoanID],
		isnull(l.loanNo, '') as loanNo,

		isnull(
			(select sum(amountPaid) from ln.loanRepayment lr where lr.loanID=l.loanID)
			,0) as amountPaid,
		isnull(
			(select sum(feePaid) from ln.loanRepayment lr where lr.loanID=l.loanID)
			,0) as feePaid,
		isnull(
			(select sum(penaltyPaid) from ln.loanRepayment lr where lr.loanID=l.loanID)
			,0) as penaltyPaid,
		isnull(
			(select sum(penaltyFee) from ln.loanPenalty lr where lr.loanID=l.loanID)
			,0) as penaltyAmount,
		isnull(
			(select case when max(repaymentDate)<>min(repaymentDate) then 'Multi'
				else convert(nvarchar(15), max(repaymentDate), 106) end
				from ln.loanRepayment lr where lr.loanID=l.loanID)
			,0) as paymentDate
	from ln.vwClients c 
		inner join ln.invoiceLoanMaster ivm on c.clientID=ivm.clientID
		inner join ln.invoiceLoan iv on ivm.invoiceLoanMasterID = iv.invoiceLoanMasterID
		inner join ln.supplier s on ivm.supplierID = s.supplierID
		left join ln.loan l on ltrim(rtrim(iv.invoiceNo)) = ltrim(rtrim(l.invoiceNo))
			and l.clientId=iv.clientID and (l.finalApprovalDate = iv.approvalDate)

	where iv.invoiceAmount>0
go


