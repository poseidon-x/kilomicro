use coreDB
go

create table ln.supplier
(
	supplierID int identity(1,1) primary key,
	supplierName nvarchar(400) not null,
	directions nvarchar(4000),
	comments ntext
)
go

create table ln.supplierContact
(
	supplierContactID int identity(1,1) primary key,
	contactName nvarchar(50),
	workPhone nvarchar(50),
	mobilePhone nvarchar(50),
	email nvarchar(400),
	department nvarchar(100)
)
go


alter table ln.supplierContact add
	supplierID int not null
go

alter table ln.supplier add
	maximumExposure float not null default(0)
go