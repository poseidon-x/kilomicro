use coreDB
go

create table ln.[collection]
(
	collectionID int identity(1,1) not null primary key,
	loanProductID int not null,
	[month] int not null,
	[collection] float not null default(0)
)
go