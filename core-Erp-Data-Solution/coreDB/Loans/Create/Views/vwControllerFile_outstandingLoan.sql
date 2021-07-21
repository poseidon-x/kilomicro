USE coreDB
GO

create view [vw_controllerFile_outstandingLoan]
with encryption 
as
Select *
From
(
Select c.clientID, ln.loanID, cfd.staffID, cfd.fileDetailID, cfd.fileID, cfd.managementUnit, cfd.oldID,
		cfd.balBF,cfd.employeeName ,cfd.monthlyDeduction, cfd.origAmt, cfd.repaymentScheduleID, cfd.authorized, cfd.duplicate, cfd.refunded, cfd.notFound,
		cfd.overage, cfd.remarks, ln.loanNo, ln.amountDisbursed, ln.disbursementDate, 
		ROW_NUMBER() Over(Partition by ln.clientID, cfd.fileID Order by ln.disbursementDate) RecordNumber,
		Count(*) Over(Partition by ln.clientID, cfd.fileID) as LoanCount 
From ln.staffCategory sc
Inner Join ln.controllerFileDetail cfd on sc.employeeNumber=cfd.staffID
Inner Join ln.client c on c.clientID=sc.clientID
Inner JOin ln.loan ln on ln.clientID=c.clientID
where ln.disbursementDate is not null and ln.balance > 10
) Sub

where loanCount > 1
GO