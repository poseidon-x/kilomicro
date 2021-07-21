use coreDB
go

create table fa.staffCategory
(
	staffCategoryID int identity(1,1) primary key,
	staffCategoryName nvarchar(100) not null unique
)
go

create table fa.jobTitle
(
	jobTitleID int identity(1,1) primary key,
	jobTitleName nvarchar(100) not null unique
)
go

create table fa.assetCategory
(
	assetCategoryID int identity(1,1) primary key,
	assetCategoryName nvarchar(100) not null unique,
	depreciationMethod int not null
)
go

create table fa.assetSubCategory
(
	assetSubCategoryID int identity(1,1) primary key,
	assetSubCategoryName nvarchar(100) not null unique,
	assetCategoryID int not null
)
go


alter table fa.assetCategory add
	depreciationAccountID int,
	accumulatedDepreciationAccountID int
go


alter table fa.assetCategory add
	tagPrefix nvarchar(4) not null
go