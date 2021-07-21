use coreDB
go

create table ln.controllerFile
(
	fileID int identity(1,1) not null primary key,
	fileMonth int not null check(fileMonth>201301 and fileMonth < 202012),
	uploadDate datetime not null default(getdate()),
	[fileName] nvarchar(100) not null
)
go

create table ln.controllerFileDetail
(
	fileDetailID int identity(1,1) not null primary key,
	fileID int not null,
	managementUnit nvarchar(100) not null,
	staffID nvarchar(100) not null,
	oldID nvarchar(100) not null,
	employeeName nvarchar(100) not null,
	origAmt float null,
	balBF float not  null,
	monthlyDeduction float not null,
	repaymentScheduleID int null,
	authorized bit not null default(0),
	duplicate bit not null default(0),
	refunded bit not null default(0)
)
go

alter table ln.controllerFileDetail add 
	notFound bit not null default(1),
	overage float not null default(0),
	remarks nvarchar(400) not null default('')
go


alter table ln.controllerFileDetail add 
	transferred bit not null default(0)
go