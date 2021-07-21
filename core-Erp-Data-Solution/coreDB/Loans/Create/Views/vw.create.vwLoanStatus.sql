use coreDB
go

alter view ln.vwLoanStatus
with encryption as
SELECT        loanID, 'Application Completed' AS status, 1 AS sortOrder, ISNULL(applicationDate, GETDATE()) AS date,
	isnull(staffID,0) as staffID, clientID,
			 isnull(l.clientID.companyId,0)as companyID , 
		 isnull(company.comp_name,'')as comp_Name
FROM            ln.loan AS l
		left join dbo.comp_prof AS company on l.clientID.companyId = company.comp_prof_id

WHERE        (applicationDate IS NOT NULL)
union all
SELECT        l.loanID, 'Check List Completed' AS status, 2 AS sortOrder, ISNULL(MAX(creationDate), GETDATE()) AS date,
	isnull(staffID,0) as staffID, clientID
FROM            ln.loan AS l INNER JOIN
                         ln.loanCheckList AS lc ON l.loanID = lc.loanID
WHERE        (lc.passed = 1)
GROUP BY l.loanID,
	isnull(staffID,0), clientID
union all
SELECT        loanID, 'Application Approved' AS status, 3 AS sortOrder, ISNULL(finalApprovalDate, GETDATE()) AS date,
	isnull(staffID,0) as staffID, clientID
FROM            ln.loan AS l
WHERE        (finalApprovalDate IS NOT NULL) AND (loanStatusID <> 7) AND (amountApproved > 0)
union all
SELECT        loanID, 'Application Denied' AS status, 3 AS sortOrder, ISNULL(finalApprovalDate, GETDATE()) AS date,
	isnull(staffID,0) as staffID, clientID
FROM            ln.loan AS l
WHERE        (loanStatusID = 7)
union all
SELECT        loanID, 'Disbursed' AS status, 4 AS sortOrder, ISNULL(disbursementDate, GETDATE()) AS date,
	isnull(staffID,0) as staffID, clientID
FROM            ln.loan AS l
WHERE        (disbursementDate IS NOT NULL) AND (amountDisbursed > 0)