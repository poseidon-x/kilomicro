use coreDB
go

alter view ln.vwLoansByStaff
with encryption as 
SELECT        c.clientID, c.clientName, c.accountNumber, c.addressLine1, c.addressLine2,isnull( c.directions, '')directions,
ISNULL( c.cityTown,'') cityTown, isnull(c.workPhone, '') workPhone,isnull( c.mobilePhone, '') mobilePhone, 
isnull(c.homePhone, '') homePhone, isnull(c.officeEmail, '') officeEmail, isnull(c.personalEmail, '') personalEmail, 
cast (0x0 as image) [image], l.amountDisbursed,
			 isnull(fa.staff.companyId,0)as companyID , 
		 isnull(company.comp_name,'')as comp_Name, 
                         l.loanTypeName, l.loanID, l.loanNo, l.disbursementDate, l.collateralType, l.collateralValue, l.interestBalance, l.principalBalance, l.expiryDate, l.daysDue, ISNULL(fa.staff.staffNo, '') AS Expr1, 
                         ISNULL(fa.staff.surName + ', ' + fa.staff.otherNames, '') AS staffName,
	isnull(totalBalance, 0) as totalBalance, isnull(amountDue, 0) as amountDue
FROM            ln.vwClients AS c INNER JOIN
                         ln.vwLoans2 AS l ON c.clientID = l.clientID INNER JOIN
                         ln.loan ON l.loanID = ln.loan.loanID LEFT OUTER JOIN
                         fa.staff ON ln.loan.staffID = fa.staff.staffID
						left join dbo.comp_prof AS company on fa.staff.companyId = company.comp_prof_id

go

