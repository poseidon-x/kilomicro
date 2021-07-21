use coreDB
go

create table hc.payCalendar
(
	payCalendarID int identity(1,1) not null primary key,
	[year] int not null check ([year] > 2000 and [year]<= 2020),
	[month] int not null check ([month] between 1 and 14),
	isProcessed bit not null default(0),
	isPosted bit not null default(0),
	daysInMonth int not null
)
go

create table hc.payMaster
(
	payMasterID int identity(1,1) not null primary key,
	payCalendarID int not null,
	staffID int not null,
	basicSalary float not null,
	netSalary float not null
)
go

create table hc.payMasterAllowance
(
	payMasterAllowanceID int identity(1,1) not null primary key,
	payMasterID int not null, 
	allowanceTypeID int not null,
	amount float not null,
	percentValue float not null default(0),
	isPercent bit not null default(0),
	description nvarchar(250) not null
)
go

create table hc.payMasterDeduction
(
	payMasterDeductionID int identity(1,1) not null primary key,
	payMasterID int not null, 
	deductionTypeID int not null,
	amount float not null,
	percentValue float not null default(0),
	isPercent bit not null default(0),
	description nvarchar(250) not null
)
go

create table hc.payMasterPension
(
	payMasterPensionID int identity(1,1) not null primary key,
	payMasterID int not null, 
	pensionTypeID int not null,
	employeeAmount float not null ,
	employerAmount float not null default(0),
	isPercent bit not null default(1),
	isBeforeTax bit not null default(0),
	description nvarchar(250) not null
)
go

create table hc.payMasterLoan
(
	payMasterLoanID int identity(1,1) not null primary key,
	payMasterID int not null, 
	staffLoanID int not null,
	amountDeducted float not null,
	principalDeducted float not null,
	interestDeducted float not null,
	description nvarchar(250) not null
)
go

create table hc.staffCalendar
(
	staffCalendarID int identity(1,1) not null primary key,
	payCalendarID int not null,
	staffID int not null,
	daysWorked float not null default(0)
)
go

create table hc.payMasterBenefitsInKind
(
	payMasterBenefitsInKindID int identity(1,1) not null primary key,
	payMasterID int not null, 
	benefitsInKindID int not null,
	amount float not null,
	percentValue float not null default(0),
	isPercent bit not null default(0),
	description nvarchar(250) not null
)
go

create table hc.payMasterOneTimeDeduction
(
	payMasterOneTimeDeductionID int identity(1,1) not null primary key,
	payMasterID int not null, 
	oneTimeDeductionTypeID int not null,
	amount float not null,
	percentValue float not null default(0),
	isPercent bit not null default(0),
	description nvarchar(250) not null
)
go

create table hc.payMasterTax
(
	payMasterTaxID int identity(1,1) not null primary key,
	payMasterID int not null,  
	amount float not null, 
	description nvarchar(250) not null
)
go

create table hc.taxTable
(
	taxTableID int identity (1,1) not null primary key,
	amount float not null default(0),
	taxPercent float not null default(0),
	sortOrder int not null default(1)
)
go

create table hc.payMasterTaxRelief
(
	payMasterTaxReliefID int identity(1,1) not null primary key,
	payMasterID int not null, 
	taxReliefTypeID int not null,
	amount float not null,
	description nvarchar(250) not null
)
go


create table hc.payrollPostingAccounts
(
	payrollPostingAccountID int identity(1,1) not null primary key,
	netSalaryAccountID int not null,
	loansReceivableAccountID int not null,
	pensionsPayableAccountID int not null,
	taxPayableAccountID int not null,
	voluntaryDeductionsAccountID int not null
)
go

alter table hc.payrollPostingAccounts add
	loansRepaymentsAccountID int not null,
	payrollExpenseAccountID int not null
go

alter table hc.payMaster add
	posted bit not null default(0)
go

create table hc.overTime
(
	overTimeID int identity(1,1) not null primary key,
	staffID int not null,
	payCalendarID int not null,
	saturdayHours float not null Default(0),
	sundayHours float not null Default(0),
	holidayHours float not null Default(0),
	weekdayAfterWorkHours float not null Default(0)
)
go

create table hc.overTimeConfig
(
	overTimeConfigID int identity(1,1) not null primary key,
	levelID int not null,
	saturdayHoursRate float not null Default(0),
	sundayHoursRate float not null Default(0),
	holidayHoursRate float not null Default(0),
	weekdayAfterWorkHoursRate float not null Default(0),
	overTime5PerTax float not null Default(5),
	overTime10PerTax float not null Default(10),
)
go


create table hc.payMasterOverTime
(
	payMasterOverTimeID int identity(1,1) not null primary key,
	payMasterID int not null,
	saturdayHoursAmount float not null Default(0),
	sundayHoursAmount float not null Default(0),
	holidayHoursAmount float not null Default(0),
	weekdayAfterWorkHoursAmount float not null Default(0),
	overTimeTaxAmount float not null Default(0)
)
go

alter table hc.payrollPostingAccounts add
	overtimeWithholdingPayable int not null default(0)
go


