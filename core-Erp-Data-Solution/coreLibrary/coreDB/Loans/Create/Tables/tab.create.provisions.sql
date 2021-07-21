use coreDB
go

create table ln.provisionClass
(
	provisionClassID int not null primary key,
	provisionClassName nvarchar(100) not null,
	minDays int not null,
	maxDays int not null,
	provisionPercent float not null
)
go

create table ln.loanProvision
(
	loanProvisionID int identity(1,1) not null primary key,
	loanID int not null,
	provisionDate datetime not null,
	principalBalance float not null,
	interestBalance float not null,
	daysDue int not null,
	proposedAmount float not null default(0),
	provisionAmount float not null default(0),
	provisionClassID int null,
	posted bit not null default(0),
	typeOfSecurity nvarchar(100) null,
	securityValue float not null default(0)
)
go

alter table ln.loanProvision add
	reversed bit not null default(0)
go
