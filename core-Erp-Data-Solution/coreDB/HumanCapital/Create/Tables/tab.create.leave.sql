use coreDB
go

create table hc.staffLeave
(
	staffLeaveID int identity(1,1) not null primary key,
	staffID int not null,
	leaveTypeID int not null,
	applicationDate datetime not null default(getdate()),
	managerApproved bit not null default(0),
	managerApprovalDate datetime,
	managerUserName nvarchar(30),
	daysApplied int not null,
	daysApproved int not null default(0),
	startDate datetime,
	endDate datetime
)
go

create table hc.holidayType
(
	holidayTypeID int identity(1,1) not null primary key,
	holidayTypeName nvarchar(250) not null unique
)
go

create table hc.publicHoliday
(
	publicHolidayID int identity(1,1) not null primary key,
	[date] datetime not null,
	yearID int not null,
	holidayTypeID int not null
)
go

create table hc.[year]
(
	yearID int identity(1,1) not null primary key,
	[year] smallint not null unique check ([year] between 2000 and 2050)
)
go

create table hc.[month]
(
	monthID int identity(1,1) not null primary key,
	[month] smallint not null unique check ([month] between 1 and 13),
	monthName nvarchar(50) not null unique
)
go

insert into hc.[year] ([year]) values (2010)
insert into hc.[year] ([year]) values (2011)
insert into hc.[year] ([year]) values (2012)
insert into hc.[year] ([year]) values (2013)
insert into hc.[year] ([year]) values (2014)
insert into hc.[year] ([year]) values (2015)
insert into hc.[year] ([year]) values (2016)
insert into hc.[year] ([year]) values (2017)
insert into hc.[year] ([year]) values (2018)
insert into hc.[year] ([year]) values (2019)
insert into hc.[year] ([year]) values (2020)
insert into hc.[year] ([year]) values (2021)
insert into hc.[year] ([year]) values (2022)
go

create table hc.staffLeaveBalance
(
	staffLeaveBalanceID int identity(1,1) primary key not null,
	staffID int not null,
	yearID int not null,
	leaveAccumulatedDays float not null default(0),
	leaveBalanceDays float not null default(0)
)
go