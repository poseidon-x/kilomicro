use coreDB
go


alter view hc.vwPaymaster
with encryption
as
SELECT        pm.payMasterID, pc.payCalendarID, pm.basicSalary, pm.netSalary, pc.year, pc.month, pc.isProcessed, pc.isPosted, s.staffNo, s.staffID, 
                         s.surName + ', ' + s.otherNames AS staffName, ISNULL(sb.bankAccountNo, '') AS bankAccountNo,ISNULL(sb.bankSortCode, '') AS bankSortCode, ISNULL(sb.bankName, '') AS bankName, 
                         ISNULL(sb.bankBranchName, '') AS bankBranchName, ISNULL(sb.ssn, '') AS ssn, fa.jobTitle.jobTitleName, ISNULL(fa.jobTitle.jobTitleID, 0) AS jobTitleID, 
                         ISNULL(s.DOB, GETDATE()) AS DOB, sc.staffCategoryID, sc.staffCategoryName,
						 isnull(l.levelName,'') as levelName, isnull(l.levelID, 0) as levelID,
						 isnull((
							select sum(amount) from hc.payMasterAllowance pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalAllowances,
						 isnull((
							select sum(amount) from hc.payMasterDeduction pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalDeductions,
						 isnull((
							select sum(amount) from hc.payMasterBenefitsInKind pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalBenefitsInKind,
						 isnull((
							select sum(amount) from hc.payMasterTax pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalTax,
						 isnull((
							select sum(amount) from hc.payMasterOneTimeDeduction pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalOneTimeDeductions,
						 isnull((
							select sum(employeeAmount) from hc.payMasterPension pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalEmployeePension,
						 isnull((
							select sum(employerAmount) from hc.payMasterPension pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalEmployerPension,
						 isnull((
							select sum(amount) from hc.payMasterTaxRelief pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalTaxRelief, isnull(staffManagerID, 0) staffManagerID,
						 isnull((
							select sum(amountDeducted) from hc.payMasterLoan pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalLoanDeductions,
						 isnull((
							select sum([saturdayHoursAmount]+[sundayHoursAmount]+[holidayHoursAmount]
							+[weekdayAfterWorkHoursAmount]) from hc.payMasterOvertime pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as totalOvertime, 
						 isnull((
							select sum(overtimeTaxAmount) from hc.payMasterOvertime pma where pma.payMasterID = pm.payMasterID
						 )
						 ,0) as overtimeTaxAmount, s.companyId, com.comp_name
FROM            hc.payMaster AS pm INNER JOIN
                         hc.payCalendar AS pc ON pm.payCalendarID = pc.payCalendarID INNER JOIN
                         fa.staff AS s ON pm.staffID = s.staffID INNER JOIN
                         hc.staffBenefit AS sb ON s.staffID = sb.staffID INNER JOIN
                         fa.jobTitle ON s.jobTitleID = fa.jobTitle.jobTitleID INNER JOIN
                         fa.staffCategory AS sc ON s.staffCategoryID = sc.staffCategoryID
						 left join hc.staffManager sm on sm.staffID = s.staffID
						 left Join hc.[level] l on sm.levelID = l.levelID
						 Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId 
go