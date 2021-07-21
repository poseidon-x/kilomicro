use coreDB
go

alter view ln.vwControllerIn
with encryption as 
SELECT  
		C.accountNumber, 
		C.surName, 
		C.otherNames, 
		l.loanNo, 
		l.loanID, 
		C.clientID, 
		isnull(employeeNumber,'') as employeeNumber,
		ISNULL(SUM(rs.interestPayment + rs.principalPayment), 0) AS totalPayment, 
        ISNULL(MAX(rs.interestPayment + rs.principalPayment), 0) AS installment, 
		ISNULL(cast(ISNULL(l.loanTenure,COUNT(rs.repaymentScheduleID)) as int), 0) AS noOfInstallments, 
        ISNULL(ln.region.regionName, '') AS regionName,
		isnull(isnull(disbursementDate, dateadd(day,1, getdate())), getdate()) as disbursementDate,
		isnull(c.companyId,0)as CompanyId,
		isnull(company.comp_Name, '') AS comp_Name
		
FROM            ln.client AS C INNER JOIN
                         ln.loan AS l ON C.clientID = l.clientID INNER JOIN
                         ln.repaymentSchedule AS rs ON l.loanID = rs.loanID INNER JOIN
                         ln.staffCategory AS sc ON C.clientID = sc.clientID LEFT OUTER JOIN
                         ln.region ON sc.regionID = ln.region.regionID AND sc.regionID = ln.region.regionID
						 inner join ln.prLoanDetail p on p.loanID=l.loanID
						 inner join ln.employer e on sc.employerID = e.employerID
						 left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

where isSentToController=0 and loanStatusID=4 and e.employmentTypeID=3
GROUP BY C.accountNumber, C.surName, C.otherNames, l.loanNo, l.loanID, C.clientID, 
		employeeNumber,ISNULL(ln.region.regionName, ''),
		isnull(disbursementDate, dateadd(day,1, getdate())),
		l.loanTenure,
		isnull(c.companyId,0),
		isnull(company.comp_Name, '') 
