use coreDB
go

create table ln.systemDate
(
	systemDateID tinyint not null  primary key check(systemDateID=1),
	loanSystemDate datetime null,
	savingSystemDate datetime null,
	depositSystemDate datetime null,
	investmentSystemDate datetime null,
	susuSystemDate datetime null,
	creditUnionSystemDate datetime null,
	accountsSystemDate datetime null,
	useSystemDates bit not null default(0)
)
go

insert into ln.systemDate (systemDateID, useSystemDates)
values (1, 0)
go
