use coreDB
go

create table hc.[shift]
(
	shiftID int identity (1,1) not null primary key,
	shiftName nvarchar(250) not null unique
)
go

create table hc.shiftAllowance
(
	shiftAllowanceID int identity (1,1) not null primary key,
	allowanceTypeID int not null,
	hourlyRate float not null
)
go

create table hc.staffShift
(
	staffShiftID int identity(1,1) not null primary key,
	staffID int not null,
	yearID int not null,
	shiftID int not null,
	startDate datetime not null,
	endDate datetime not null
)
go

create table hc.staffAttendance
(
	staffAttendanceID int identity (1,1) not null primary key,
	staffID int not null,
	yearID int not null,
	monthID int not null,
	clockInTime datetime,
	clockOutTime datetime
)
go

alter table hc.shiftAllowance add
	shiftID int not null

create table hc.staffDaysWorked
(
	staffDaysWorkedID int identity(1,1) not null primary key,
	staffID int not null,
	payCalendarID int not null,
	daysWorked float not null default(0)
)
go
