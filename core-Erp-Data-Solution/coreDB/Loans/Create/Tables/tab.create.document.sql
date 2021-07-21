use coreDB
go

create table ln.document
(
	documentID int identity(1,1) primary key,
	documentImage image,
	[description] nvarchar(1000),
	contentType nvarchar(100),
	fileName nvarchar(100)
)
go

create table ln.loanDocument
(
	loanDocumentID int identity(1,1) primary key,
	documentID int not null,
	loanID int not null
)
go


create table ln.clientDocument
(
	clientDocumentID int identity(1,1) primary key,
	documentID int not null,
	clientID int not null
)
go