use coreDB
go

create procedure ln.getPARnPIRData
(
	@clientID int,
	@staffID int,
	@branchID int,
	@loanTypeID int,
	@endDate datetime
)
with encryption as
begin
select 
		c.clientID,
		isnull(clientName, '')  as clientName,
		c.accountNumber,
		l.loanID,
		l.loanNo,
		isnull(l.disbursementDate, getDate()) disbursementDate, 
		lt.loanTypeName,
		isnull(l.amountDisbursed, 0 ) as principal,
		isnull(b.branchID, 0) as branchID,
		isnull(b.branchName, '') as branchName,
		isnull(s.staffID, 0) as staffID,
		isnull(s.surName + ', '+ s.otherNames, '') as staffName,
		isnull(
			(select min(repaymentDate) from ln.repaymentSchedule rs 
			where rs.loanID = l.loanID and (rs.principalBalance+rs.interestBalance)>5 and repaymentDate <@endDate
			), getDate()) as oldestOwedDate,
		isnull(
			(select sum(rs.principalBalance) from ln.repaymentSchedule rs 
			where rs.loanID = l.loanID and (rs.principalBalance+rs.interestBalance)>5 and repaymentDate <@endDate
			), 0) as principalOwed,
		isnull(
			(select sum(+rs.interestBalance) from ln.repaymentSchedule rs 
			where rs.loanID = l.loanID and (rs.principalBalance+rs.interestBalance)>5 and repaymentDate <@endDate
			), 0) as interestOwed,
		isnull(
			(select sum(rs.principalBalance) from ln.repaymentSchedule rs 
			where rs.loanID = l.loanID and (rs.principalBalance+rs.interestBalance)>5
			), 0) as totalPrincipalOwed,
		isnull(
			(select sum(rs.interestBalance) from ln.repaymentSchedule rs 
			where rs.loanID = l.loanID and (rs.principalBalance+rs.interestBalance)>5
			), 0) as totalInterestOwed,
		isnull(
			(select sum(rs.interestPayment) from ln.repaymentSchedule rs 
			where rs.loanID = l.loanID
			), 0) as interest,
		isnull(workPhone, '') as workPhone,
		isnull(mobilePhone, '') as mobilePhone,
		isnull(homePhone, '') as homePhone,
		isnull(officeEmail,'') as workEmail,
		isnull(personalEmail, '') as personalEmail,
		isnull(addressLine1, '') as addressLine1,
		isnull(addressLine2, '') as addressLine2,
		isnull(cityTown, '') as cityTown,
		isnull(directions, '') as directions
from ln.loan l
	inner join ln.vwClients c on l.clientID = c.clientID
	inner join ln.loanType lt on l.loanTypeID = lt.loanTypeID
	inner join ln.branch b on c.branchID = b.branchID
	left join hc.vwStaff s on l.staffID = s.staffID
where l.loanStatusID = 4
	and disbursementDate <= @endDate
	and (@clientID is null or c.clientID = @clientID)
	and (@staffID is null or s.staffID = @staffID)
	and (@branchID is null or c.branchID = @branchID)
	and (@loanTypeID is null or l.loanTypeID = @loanTypeID)
end
go
