use coreDB
go

alter view ln.vwAlert
with encryption
as
SELECT        l.loanID AS ID, applicationDate AS Date, c.surName + ', ' + c.otherNames + 
		': Unverified(Checklist) Loan Application of ¢' + 
          replace(CONVERT(varchar(50), CAST(ROUND(amountRequested, 2) AS money), 1),'.00','')
		  +   case when isnull(enteredBy, l.creator) is not null 
						 then ' entered by: ' + isnull(enteredBy, l.creator)
						 else '' end AS alert, isnull(enteredBy, l.creator) AS staffName, 
						 '' as userName, 'No Checklist' AS AlertType,
						 '/ln/loans/loanCheckList.aspx?id=' + cast(l.loanID as varchar(10))+'&catID=' + 
						 cast(c.categoryID as varchar(10)) as url,
						 isnull(c.companyId,0)as companyID , isnull(company.comp_name,'')as comp_Name,
		l.clientID as clientID
FROM            ln.loan AS l INNER JOIN
                         ln.client AS c ON l.clientID = c.clientID 
				left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
WHERE  loanStatusID=1
union all
SELECT        l.loanID AS ID, applicationDate AS Date, c.surName + ', ' + c.otherNames + ': Undisbursed Loan Application of ¢' + 
          replace(CONVERT(varchar(50), CAST(ROUND(amountApproved, 2) AS money), 1),'.00','')
		  +   case when isnull(enteredBy, l.creator) is not null 
						 then ' approved by: ' + isnull(approvedBy, l.creator)
						 else '' end AS alert, isnull(enteredBy, l.creator) AS staffName, '' as userName,
						  'Undisbursed Loan' AS AlertType,
						 '/ln/cashier/disburse.aspx?id=' + cast(l.loanID as varchar(10))+'&catID=' + 
						 cast(c.categoryID as varchar(10)) as url,
						 isnull(c.companyId,0)as companyID , isnull(company.comp_name,'')as comp_Name,

		l.clientID as clientID
FROM            ln.loan AS l INNER JOIN
                         ln.client AS c ON l.clientID = c.clientID 
						left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
WHERE  loanStatusID=3
union all
SELECT        l.loanID AS ID, applicationDate AS Date, c.surName + ', ' + c.otherNames + ': Unapproved Loan Application of ¢' + 
          replace(CONVERT(varchar(50), CAST(ROUND(amountRequested, 2) AS money), 1),'.00','')
		  +   case when isnull(enteredBy, l.creator) is not null 
						 then ' checked by: ' + isnull(checkedBy, l.creator)
						 else '' end AS alert, isnull(checkedBy, l.creator) AS staffName, '' as userName, 
						 'Unapproved Loan' AS AlertType,
						 '/ln/loans/approve.aspx?id=' + cast(l.loanID as varchar(10))+'&catID=' + 
						 cast(c.categoryID as varchar(10)) as url,
		l.clientID as clientID,
		 isnull(c.companyId,0)as companyID , isnull(company.comp_name,'')as comp_Name

FROM            ln.loan AS l INNER JOIN
                         ln.client AS c ON l.clientID = c.clientID  
						left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
WHERE  loanStatusID=2
union all
SELECT      distinct    al.clientActivityLogID AS ID, al.nextActionDate AS Date, 'Action on ' + 
           c.surName + ', ' + c.otherNames + ': "' + CAST(al.nextAction AS nvarchar(MAX))+'" assigned to: '
	        + s.surName + ', ' + s.otherNames AS alert, 
                         s.surName + ', ' + s.otherNames AS staffName, s.userName, 'Activity Log' AS AlertType,
						 '/ln/loans/clientActivity.aspx?id=' + cast(c.clientID as varchar(10)) as url,
		c.clientID as clientID,
				 isnull(c.companyId,0)as companyID , isnull(company.comp_name,'')as comp_Name

FROM            ln.clientActivityLog AS al INNER JOIN
                         ln.client AS c ON al.clientID = c.clientID INNER JOIN
                         fa.staff AS s ON al.responsibleStaffID = s.staffID
						left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

WHERE        (al.nextAction IS NOT NULL) AND (al.nextActionDate BETWEEN 
	CAST(CAST(DATEPART(year, GETDATE()) AS char(4)) + '-' + CAST(DATEPART(month, GETDATE()) AS varchar(2)) 
                         + '-' + CAST(DATEPART(day, GETDATE()) AS varchar(2)) AS datetime) AND DATEADD(second, - 1, DATEADD(day, 1, CAST(CAST(DATEPART(year, GETDATE()) AS char(4)) 
                         + '-' + CAST(DATEPART(month, GETDATE()) AS varchar(2)) + '-' + CAST(DATEPART(day, GETDATE()) AS varchar(2)) AS datetime))))
union all
SELECT        l.loanID AS ID, rs.repaymentDate AS Date, c.surName + ', ' + c.otherNames + ': Expected to pay ¢' + 
          replace(CONVERT(varchar(50), CAST(ROUND(rs.principalPayment + rs.interestPayment, 2) AS money), 1),'.00','')
		  + ' today.'
						 + case when s.surName + ', ' + s.otherNames is not null 
						 then ' Loan assigned to: ' + s.surName + ', ' + s.otherNames
						 else '' end AS alert, s.surName + ', ' + s.otherNames AS staffName, s.userName, 'Due Payment' AS AlertType,
						 '/ln/loans/loan.aspx?id=' + cast(l.loanID as varchar(10))+'&catID=' + 
						 cast(c.categoryID as varchar(10)) as url,
		l.clientID as clientID,
		isnull(c.companyId,0)as companyID , isnull(company.comp_name,'')as comp_Name

FROM            ln.repaymentSchedule AS rs INNER JOIN
                         ln.loan AS l ON rs.loanID = l.loanID INNER JOIN
                         ln.client AS c ON l.clientID = c.clientID INNER JOIN
                         fa.staff AS s ON l.staffID = s.staffID
						left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
WHERE        (rs.repaymentDate BETWEEN CAST(CAST(DATEPART(year, GETDATE()) AS char(4)) + '-' + CAST(DATEPART(month, GETDATE()) AS varchar(2)) 
                         + '-' + CAST(DATEPART(day, GETDATE()) AS varchar(2)) AS datetime) AND DATEADD(second, - 1, DATEADD(day, 1, CAST(CAST(DATEPART(year, GETDATE()) AS char(4)) 
                         + '-' + CAST(DATEPART(month, GETDATE()) AS varchar(2)) + '-' + CAST(DATEPART(day, GETDATE()) AS varchar(2)) AS datetime))))
			and loanStatusID=4
union all
SELECT        l.depositID AS ID, l.maturityDate AS Date, c.surName + ', ' + c.otherNames + ': matured deposit with balance ¢' +
          replace(CONVERT(varchar(50), CAST(ROUND(l.principalBalance + l.interestBalance, 2) AS money), 1),'.00','') AS alert, 
			'' AS staffName, '' AS userName, 'Matured Deposit' AS AlertType,
						 '/ln/deposit/deposit.aspx?id=' + cast(l.depositID as varchar(10)) as url,
		c.clientID as clientID,
		isnull(c.companyId,0)as companyID , isnull(company.comp_name,'')as comp_Name

FROM            ln.deposit AS l INNER JOIN
                         ln.client AS c ON l.clientID = c.clientID
						 left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
WHERE        (l.maturityDate <= DATEADD(second, - 1, DATEADD(day, 1, CAST(CAST(DATEPART(year, GETDATE()) AS char(4)) + '-' + CAST(DATEPART(month, GETDATE()) 
                         AS varchar(2)) + '-' + CAST(DATEPART(day, GETDATE()) AS varchar(2)) AS datetime)))) AND (l.principalBalance + l.interestBalance > 10)
go