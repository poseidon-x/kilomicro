use coreDB
go

create view ln.vwBankDown
with encryption as
	SELECT 
		c.clientID, c.surName, c.otherNames, c.accountNumber,
		isnull(amountDisbursed,0) as initialAmount,
		isnull(principalPayment+interestPayment,0) as monthlyDeduction,
		isnull(isnull(cfd.employeeName, otherNames + ' ' + surName), '') as names,
		isnull(l.loanTenure,0) as tenure,
		isnull(isnull(cfd.staffID, employeeNumber), '') as staffID,
		isnull(cfd.balBF, 0) as balBF,
		isnull(cfd.monthlyDeduction, 0) as controllerDeduction,
		isnull(remarks, 'Not Deducted') as comments,
		isnull(repaymentDate, getDate()) as repaymentDate,
        isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name	FROM ln.client AS c
		INNER JOIN ln.staffCategory AS sc ON c.clientID = sc.clientID
		inner join ln.loan l on c.clientID=l.clientID
		inner join ln.repaymentSchedule rs on rs.loanID=l.loanID
		left join ln.controllerFileDetail cfd on cfd.repaymentScheduleID=rs.repaymentScheduleID 
		left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

	where l.loanTypeID=6 and repaymentDate<=getDate()
		and loanStatusID=4 and balance > 44

go
