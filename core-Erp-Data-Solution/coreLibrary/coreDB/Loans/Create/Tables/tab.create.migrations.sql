use coreDB
go

  create table dbo.susuOpeningBalance
  (  
	accountNumber nvarchar(50),
	firstName nvarchar(50),
	lastName nvarchar(50),
	sex nchar(1),
	dateofBirth nvarchar(50) not null default(getDate()),
	idNumber nvarchar(20),
	startDate datetime not null,
	rate float not null,
	position int not null,
	amountContributed float not null,
	posted bit not null default(0),
	disbursed bit not null default(0)
  )
  go
  
  create table dbo.loanOpeningBalance
  (  
	accountNumber nvarchar(50),
	loanNo int,
	firstName nvarchar(50),
	lastName nvarchar(50),
	sex nchar(1),
	dateofBirth nvarchar(50) not null default(getDate()),
	idNumber nvarchar(20),
	startDate datetime not null,
	rate float not null,
	repayment int not null,
	tenure float not null,
	loanAmount float not null,
	fees float not null,
	amountPaid float not null,
	posted bit not null default(0),
	disbursed bit not null default(0),
	loanType int not null
  )
  go
   
 create table dbo.loanOpeningSchedule
 (  
	accountNumber nvarchar(50),
	loanNo int,
	repaymenDate datetime,
	principal float,
	interest float,
	penalty float
)
go
 
 create table dbo.loanOpeningRepayment
 (  
	accountNumber nvarchar(50),
	loanNo int,
	repaymenDate datetime,
	principalPaid float,
	interestPaid float,
	penaltyPaid float
)
go
