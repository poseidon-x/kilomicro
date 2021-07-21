use coreDB
go

create table ln.loanConfig
(
	loanConfigID int identity(1,1) not null primary key,
	penaltyIsAdditionalInterest bit not null default(0),
	automaticInterestCalculation bit not null default(0),
	applicationFeeAmount float not null
)
go

alter table ln.loanConfig add
	penaltyScheme tinyint not null default(0)
go
