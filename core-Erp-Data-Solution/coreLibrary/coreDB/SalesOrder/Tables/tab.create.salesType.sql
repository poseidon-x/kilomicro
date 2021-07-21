use coreDB
go


create table so.salesType
(
	salesTypeID int not null primary key,
	salesTypeName nvarchar(250) not null constraint uk_salesType unique
)
go

insert into so.salesType (salesTypeID, salesTypeName)
values (1, 'Cash Sale')
insert into so.salesType (salesTypeID, salesTypeName)
values (2, 'Credit Sale') 
go

