use coreDB
go

create view hc.vwPaymasterDeduction
with encryption 
as
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.amount, 1 AS typeID, 'Tax' AS EarningType, 
						 s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterTax AS pma ON pm.payMasterID = pma.payMasterID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.employeeAmount AS amount, pma.pensionTypeID AS typeID, 
                         'Pension' AS EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterPension AS pma ON pm.payMasterID = pma.payMasterID INNER JOIN
                         hc.pensionType AS at ON pma.pensionTypeID = at.pensionTypeID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.amount, pma.deductionTypeID AS typeID, 
                         'Deduction' AS EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterDeduction AS pma ON pm.payMasterID = pma.payMasterID INNER JOIN
                         hc.deductionType AS at ON pma.deductionTypeID = at.deductionTypeID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.amount, pma.oneTimeDeductionTypeID AS typeID, 
                         'Onetime Deduction' AS EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterOneTimeDeduction AS pma ON pm.payMasterID = pma.payMasterID INNER JOIN
                         hc.oneTimeDeductionType AS at ON pma.oneTimeDeductionTypeID = at.oneTimeDeductionTypeID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, isnull(pma.amountDeducted,0) as amount, 
						 isnull(pma.staffLoanID,0) AS typeID, 
                         'Staff Loan Deduction' AS EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterLoan AS pma ON pm.payMasterID = pma.payMasterID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
go