use	coreDB
go

create table fa.asset
(
	assetID int identity(1,1) primary key,
	assetSubCategoryID int not null,
	assetDescription nvarchar(1000),
	assetNotes ntext not null default(''),
	staffID int,
	ouID int,
	assetPurchaseDate datetime not null,
	lastDepreciationDate datetime,
	assetLifetime int not null,
	assetTag nvarchar(100),
	assetPrice float not null default(0),
	assetCurrentValue float not null default(0)
)
go

create table fa.assetDocument
(
	assetDocumentID int identity(1,1) primary key,
	documentID int not null,
	assetID int not null
)
go

create table fa.assetDepreciation
(
	assetDepreciationID int identity(1,1) primary key,
	assetID int not null,
	drepciationDate datetime,
	period int not null,
	startDate datetime,
	assetValue float not null default(0),
	depreciationAmount float not null default(0)
)
go

create  table fa.assetImage
(
	assetImageID int identity(1,1) primary key,
	assetID int , 
	imageID int 
)
go

alter table fa.asset add
	depreciationRate float not null default(0)
go

create table fa.depreciationSchedule
(
	depreciationScheduleID int identity(1,1) primary key,
	assetID int not null,
	drepciationDate datetime,
	period int not null,
	startDate datetime,
	assetValue float not null default(0),
	depreciationAmount float not null default(0)
)
go

alter table fa.assetDepreciation add
	proposedAmount float not null default(0);
go

alter table fa.asset add
	posted bit not null default(0)
go

alter table fa.asset add
	depreciationAccountID int,
	fixedAssetsAccountID int,
	accumulatedDepreciationAccountID int
go
