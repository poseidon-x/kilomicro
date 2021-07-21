use coreDB
go

create table iv.inventoryMethod
(
	inventoryMethodId int not null primary key,
	inventoryMethodName nvarchar(100) not null unique
)
go

insert into iv.inventoryMethod (inventoryMethodId, inventoryMethodName)
values (1, 'FIFO')
insert into iv.inventoryMethod (inventoryMethodId, inventoryMethodName)
values (2, 'LIFO')
insert into iv.inventoryMethod (inventoryMethodId, inventoryMethodName)
values (3, 'Weighted Average')
