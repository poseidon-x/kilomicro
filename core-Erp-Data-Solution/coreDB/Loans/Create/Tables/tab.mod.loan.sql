use coreDB
go

alter table ln.repaymentSchedule add
	origInterestPayment float not null default(0),
	additionalInterest float not null default(0),
	origPrincipalCD float not null default(0),
	origPrincipalBF float not null default(0),
	penaltyAmount float not null default(0)
go

alter table ln.loan add
	lastEOD datetime null
go

alter table ln.repaymentSchedule add
	origPrincipalPayment float
go

alter table ln.repaymentSchedule add
	additionalInterestBalance float
go