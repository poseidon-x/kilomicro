CREATE VIEW [dbo].[loanNo_by_staffID]
AS
SELECT        TOP (100) PERCENT ln.staffCategory.employeeNumber,
            ln.staffCategory.clientID AS Expr1, 
			ln.loan.loanNo,
			 ln.client.clientID,
			  ln.controllerFileDetail.fileID,
			   ln.loan.loanID, ln.loan.balance,
			  isnull(ln.client.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name 
FROM            ln.client INNER JOIN
                         ln.staffCategory ON ln.client.clientID = ln.staffCategory.clientID INNER JOIN
                         ln.loan ON ln.client.clientID = ln.loan.clientID CROSS JOIN
                         ln.controllerFileDetail
					left join dbo.comp_prof AS company on ln.client.companyId = company.companyId

ORDER BY ln.controllerFileDetail.fileID

GO