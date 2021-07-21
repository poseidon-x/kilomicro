use coreDB
go

create view hc.vwPaymasterEarning
with encryption
as
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName,'Basic Salary' as description, pm.basicSalary as amount,  isnull(cast(1 as bit),0) as isTaxable, 1 as typeID, 
						 'Basic Salary' as EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId   
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.amount, isnull(at.isTaxable, 0) isTaxable, pma.allowanceTypeID as typeID, 
						 'Allowance' as EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterAllowance AS pma ON pm.payMasterID = pma.payMasterID INNER JOIN
                         hc.allowanceType AS at ON pma.allowanceTypeID = at.allowanceTypeID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.amount, isnull(at.isTaxable, 0) isTaxable, pma.benefitsInKindID AS typeID, 
                         'Benefits In Kind' AS EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterBenefitsInKind AS pma ON pm.payMasterID = pma.payMasterID INNER JOIN
                         hc.benefitsInKind AS at ON pma.benefitsInKindID = at.benefitsInKindID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, pma.description, pma.amount,isnull(cast(0 as bit), 0) as isTaxable, pma.taxReliefTypeID AS typeID, 
                         'Tax Relief' AS EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterTaxRelief AS pma ON pm.payMasterID = pma.payMasterID INNER JOIN
                         hc.taxReliefType AS at ON pma.taxReliefTypeID = at.taxReliefTypeID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
union all
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName, 'OverTime' description, 
						 saturdayHoursAmount+sundayHoursAmount+holidayHoursAmount+weekdayAfterWorkHoursAmount AS amount, 
						 isnull(CAST(1 as bit), 0) isTaxable, 1 as typeID, 
						 'OverTime' as EarningType, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID INNER JOIN
                         hc.payMasterOverTime AS pma ON pm.payMasterID = pma.payMasterID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId
go

