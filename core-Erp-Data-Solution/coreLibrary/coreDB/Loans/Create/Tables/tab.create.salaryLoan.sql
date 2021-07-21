use coreDb
go

create table ln.salaryLoanConfig
(
	salaryLoanConfigId int identity(1,1) primary key,
	employerId int not null,
	productName nvarchar(200) not null,
	tenure float not null,
	interestRate float not null,
	processingFeeRate float not null,
	penaltyRate float not null,
	isActive bit not null
)
go

create table ln.salaryLoan
(
	salaryLoanId int identity(1,1) primary key,
	clientId int not null,
	employerId int not null,
	salaryLoanConfigId int not null,
	loanId int null,
	applicationDate datetime not null,
	basicSalary float not null,
	totalAllowances float not null,
	nominalDeductions float not null,
	employmentStartDate datetime not null,
	applicationAmount float not null,
	approvedAmount float not null,
	disbursedAmount float not null,
	approvalDate float null,
	disbursementDate datetime null,
	[status] nchar(1) not null constraint df_salaryLoan_status default('N') constraint ck_salaryLoan_status check([status] in ('N', 'A', 'D', 'R', 'F', 'P')),
	approvingDirectorId int null,
	directorApprovalDate datetime null
)
go
