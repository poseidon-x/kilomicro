use coreDB
go

create table iv.product
(
	productId int not null identity(1,1) primary key,
	productSubCategoryId int not null,
	productCode nvarchar(10) not null unique,
	productName nvarchar(250),
	productDescription nvarchar(400) not null,
	inventoryMethodId int not null,
	unitOfMeasurementId int not null,
	productMakeId int not null,
	productStatusId int not null,
	creator nvarchar(50) not null,
	created datetime not null,
	modifier nvarchar(50) not null,
	modified datetime not null, 
)
go
