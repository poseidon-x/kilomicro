use coreDB
go

alter table ln.susuAccount add
	lastEOD datetime,
	isDormant bit not null default(0),
	convertedToLoan bit not null default(0),
	appliedToLoan bit not null default(0),
	interestAmountApplied float not null default(0)
go

alter table ln.regularSusuAccount add
	lastEOD datetime,
	isDormant bit not null default(0),
	convertedToLoan bit not null default(0),
	appliedToLoan bit not null default(0),
	interestAmountApplied float not null default(0)
go

alter table ln.susuConfig add
	dormantInterestRate float not null default(8),
	dormantPenaltyRate float not null default(2)
go
