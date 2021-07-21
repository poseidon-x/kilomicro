use coreDB
go

create table iv.productMake
(
	productMakeId int not null primary key,
	productMakeName nvarchar(100) not null
)
go

insert into iv.productMake (productMakeId, productMakeName)
values (1, 'Purchased for Sale')
insert into iv.productMake (productMakeId, productMakeName)
values (2, 'Manufactured for Sale')
insert into iv.productMake (productMakeId, productMakeName)
values (3, 'Manufacturing Parts') 


create table iv.productStatus
(
	productStatusId int not null primary key,
	productStatusName nvarchar(250) not null unique
)
go

insert into iv.productStatus (productStatusId, productStatusName)
values (1, 'Active')
insert into iv.productStatus (productStatusId, productStatusName)
values (2, 'Discontinued')
