use coreDB
go

create view ln.vwInvoiceLoan
with encryption
as
	SELECT  c.clientID, c.clientName, c.accountNumber, c.addressLine1, c.addressLine2, c.directions, c.cityTown, c.workPhone, c.mobilePhone, c.homePhone, 
		c.officeEmail, c.personalEmail, c.image, c.categoryName, isnull(company.companyId,0)as CompanyId,isnull(company.comp_Name, '') AS comp_Name,
		isnull(il.invoiceDescription, '') invoiceDescription, isnull(il.invoiceAmount, 0) invoiceAmount, isnull(il.withHoldingTax, 0) withHoldingTax, 
		isnull(il.ceilRate, 0) ceilRate, isnull(il.proposedAmount, 0) proposedAmount, isnull(il.amountDisbursed, 0) amountDisbursed, 
		isnull(il.invoiceDate, getDate()) invoiceDate, isnull(il.approved, 0) approved, isnull(il.disbursed, 0) disbursed, 
		isnull(il.processingFee, 0) processingFee, isnull(il.interestAmount, 0) interestAmount, isnull(il.invoiceNo, '') invoiceNo, 
		isnull(il.amountApproved, 0) amountApproved, isnull(il.approvalDate, getDate()) approvalDate, isnull(il.approvedBy, '') approvedBy,
		isnull(il.invoiceLoanMasterID, 0) invoiceLoanMasterID

	FROM ln.invoiceLoanMaster AS ilm RIGHT OUTER JOIN
		ln.invoiceLoan AS il ON ilm.invoiceLoanMasterID = il.invoiceLoanMasterID INNER JOIN
		ln.vwClients AS c ON il.clientID = c.clientID
		left join dbo.comp_prof AS company on company.comp_prof_id = il.clientID

go