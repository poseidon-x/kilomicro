use coreDB
go

alter view ln.vwCashierDetail
with encryption as
    SELECT isnull(full_name, '') as full_name, userName,amountPaid AS credit, 0.0 as debit, repaymentTypeName as naration, 
		modeOfPaymentName, loanNo, repaymentDate as txDate, cl.accountNumber, isnull(clientName, '') as clientName, 
		cl.clientID, 'Loan Repayment' as txType, isnull(principalPaid,0) principalPaid, isnull(interestPaid,0) interestPaid,
		cp.comp_name, cp.comp_prof_id
	FROM  dbo.vwCashierRepayments cv
	Left Join dbo.comp_prof	cp ON cp.companyId = cl.companyId

	union all
	SELECT isnull(full_name, '') as full_name, userName,0.0 as credit, amountDisbursed AS debit, 'Loan Disbursement', 
		modeOfPaymentName, loanNo, disbursementDate, accountNumber, isnull(clientName, '') as clientName, 
		clientID, 'Loan Disbursement', 0.0 as principalPaid, 0.0 as interestPaid,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	FROM ln.vwCashierDisb
	     left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	union all
	SELECT isnull(full_name, '') as full_name, a.creator,savingAmount as credit, 0.0 AS debit, naration, 
		modeOfPaymentName, savingNo, savingDate, accountNumber, isnull(clientName, '') as clientName, 
		clientID, 'Savings Account Deposit', 0.0 as principalPaid, 0.0 as interestPaid,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name

	FROM ln.vwSavingAdditional a left join users u on ltrim(rtrim(a.creator))=ltrim(rtrim(u.user_name))
		 left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	union all
	SELECT isnull(full_name, '') as full_name, a.creator,depositAmount as credit, 0.0 AS debit, naration, 
		modeOfPaymentName,  depositNo, depositDate, accountNumber, isnull(clientName, '') as clientName, 
		clientID, 'Fixed Deposit Account', 0.0 as principalPaid, 0.0 as interestPaid,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	FROM ln.vwdepositAdditional a left join users u on ltrim(rtrim(a.creator))=ltrim(rtrim(u.user_name))
	     left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	union all
	SELECT isnull(full_name, '') as full_name, a.creator,0.0 as credit, amountWithdrawn AS debit, naration, 
		modeOfPaymentName,  savingNo, withdrawalDate, accountNumber, isnull(clientName, '') as clientName, 
		clientID, 'Savings Account Withdrawal', 0.0 as principalPaid, 0.0 as interestPaid,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	FROM ln.vwSavingWithdrawal a left join users u on ltrim(rtrim(a.creator))=ltrim(rtrim(u.user_name))
		     left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	union all
	SELECT isnull(full_name, '') as full_name, a.creator,0.0 as credit, amountWithdrawn AS debit, naration, 
		modeOfPaymentName, depositNo, withdrawalDate, accountNumber, isnull(clientName, '') as clientName, 
		clientID, 'Fixed Deposit Withdrawal', 0.0 as principalPaid, 0.0 as interestPaid,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
	FROM ln.vwdepositWithdrawal a left join users u on ltrim(rtrim(a.creator))=ltrim(rtrim(u.user_name))
			     left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

go

