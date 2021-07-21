use coreDB
go

create table iv.productCategory
(
	productCategoryId int identity(1,1) primary key,
	productCategoryName nvarchar(250) not null unique,
	cogsAccountId int,
	inventoryAccountId int,
	apAccountId int,
	arAccountId int,
	incomeAccountId int,
	expenseAccountId int,
	createdBy nvarchar(30) not null,
	creationDate datetime not null default(getDate()),
	modifiedBy nvarchar(30),
	modifiedDate datetime
)
go

create table iv.productSubCategory
(
	productSubCategoryId int identity(1,1) primary key,
	productCategoryId int not null,	
	productSubCategoryName nvarchar(250) not null,
	createdBy nvarchar(30) not null,
	creationDate datetime not null default(getDate()),
	modifiedBy nvarchar(30),
	modifiedDate datetime
)
go