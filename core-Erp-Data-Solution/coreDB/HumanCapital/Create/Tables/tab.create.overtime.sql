use coreDB
go

create table hc.overtimeConfig
(
	overtimeConfigID int identity(1,1) not null primary key,
	levelID int not null,
	saturdayRate float not null,
	sundayRate float not null,
	holidayRate float not null,
	weekdayRate float not null,
	overtimeTaxRate float not null
)
go

create table hc.staffOvertime
(
	staffOvertimeID int identity(1,1) not null primary key,
	staffID int not null,
	payCalendarID int not null,
	saturdayHours float not null,
	sundayHours float not null,
	holidayHours float not null,
	weekDayHours float not null
)
go
