use coreDB
go

create table ar.invoiceStatus
(
	invoiceStatusID int not null primary key,
	invoiceStatusName nvarchar(100) not null unique
)
go


insert into ar.invoiceStatus (invoiceStatusID, invoiceStatusName) values (1, 'Pending')
insert into ar.invoiceStatus (invoiceStatusID, invoiceStatusName) values (2, 'Paid')
insert into ar.invoiceStatus (invoiceStatusID, invoiceStatusName) values (3, 'Cancelled')
go
