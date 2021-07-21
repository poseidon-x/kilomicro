


use coreDB
go

alter function ln.calcLoanBalance
(
	@loanId int
)
returns float
with encryption
as 
begin
	declare @bal float 

	select @bal = sum((principal+interest-principalPaid-interestPaid+penaltyAmount-penaltyPaid))
	from ln.vwLoanActualSchedule
	where loanId = @loanId

	return isnull(@bal, 0)
end
go

alter view ln.vwPLSMSData
with encryption as
	select
		isnull(c.clientID, 0) as clientID,
		isnull(c.clientName, '') as clientName,
		isnull('233' + substring(isnull(mobilePhone, isnull(workPhone, isnull(homePhone, ''))) , 2, 9),'') as phone,
		isnull(monthlyDeduction, 0) as monthlyDeduction,
		isnull(ln.calcLoanBalance(l.loanId), 0) as balBF,
		isnull(fileMonth, 0) as fileMonth
	from ln.vwClients c inner join ln.loan l on c.clientID = l.clientID
		inner join ln.repaymentSchedule r on l.loanID = r.loanID
		inner join ln.controllerFileDetail cfd on cfd.repaymentScheduleID = r.repaymentScheduleID
		inner join ln.controllerFile cf on cfd.FileID = cf.FileID
	where l.loanTypeID = 6

go
