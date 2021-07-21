use coreDB
go

create table ar.paymentTerm
(
	paymentTermID int not null identity(1,1) primary key,
	paymentTerms nvarchar(100) not null unique,
	netDays int not null default(0),
	discountIfBeforeDays int not null default(0),
	discountPercent float not null default(0)
)
go

