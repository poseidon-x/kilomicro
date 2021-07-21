use coreDB
go

create table hc.staffLoanType
(
	loanTypeID int identity(1,1) not null primary key,
	loanTypeName nvarchar(250) not null unique,
	attractsInterest bit not null default(0),
	rate float not null default(0),
	timeToRepay int not null
)
go

create table hc.staffLoanTypeLevel
(
	loanTypeLevelID int identity(1,1) not null primary key,
	loanTypeID int not null,
	levelID int not null,
	minPercentOfBasic float not null default(0) check(minPercentOfBasic>=0 and minPercentOfBasic<=100),
	maxPercentOfBasic float not null default(0) check(maxPercentOfBasic>0 and maxPercentOfBasic<=100)
)
go

create table hc.staffLoan
(
	staffLoanID int identity(1,1) not null primary key,
	loanTypeID int not null,
	staffID int not null,
	principal float not null,
	principalBalance float not null,
	attractsInterest bit not null default(0),
	rate float not null default(0),
	approvedDate datetime,
	disbursementDate datetime,
	interestAccumulated float not null default(0),
	interestBalance float not null default(0)
)
go

create table hc.staffLoanSchedule
(
	staffLoanScheduleID int identity(1,1) not null primary key,
	staffLoanID int not null,
	principalDeduction float not null,
	interestDeduction float not null,
	deductionDate datetime not null,
	balanceAfter float not null
)
go

create table hc.staffLoanRepayment
(
	staffLoanScheduleID int identity(1,1) not null primary key,
	staffLoanID int not null,
	principalPaid float not null,
	interestPaid float not null,
	repaymentDate datetime not null,
	balanceAfter float not null
)
go

alter table hc.staffLoan add
	approvedBy nvarchar(50),
	enteredBy nvarchar(50),
	creationDate datetime not null default(getdate())
go

alter table hc.staffLoanRepayment add
	repaymentType nchar(1) not null default('D'),
	enteredBy nvarchar(50),
	creationDate datetime not null default(getdate()),
	checkedBy nvarchar(50)
go

alter table hc.staffLoan add
	deductionStartsDate datetime not null,
	memo ntext not null default('')
go

alter table hc.staffLoan add
	bankID int,
	checkNo nvarchar(50),
	posted bit not null default(0)
go

