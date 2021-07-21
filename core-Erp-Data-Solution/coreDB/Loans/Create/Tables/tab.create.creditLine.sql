use coreDB
go

create table ln.creditLine
(
	creditLineId int identity(1,1) not null primary key,
	clientId int not null,
	creditLineNumber nvarchar(30) not null,
	loanId int null,
	tenure int not null,
	amountRequested float not null,
	expiryDate datetime null,
	amountApproved float not null default(0),
	amountDisbursed float not null default(0),
	applicationDate datetime not null,
	isApproved bit not null,
	approvalDate datetime null,
	approvedBy nvarchar(30) null,
	closed bit not null,
	creator nvarchar(30) not null,
	creationDate datetime not null,
	modifier nvarchar(30) null,
	modified datetime null
)
	 