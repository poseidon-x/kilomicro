use coreDB
go


create table ln.loanDocumentTemplate
(
	loanDocumentTemplateId int identity(1,1) not null primary key,
	templateName nvarchar(30) not null unique,
	creator nvarchar(30),
	creationDate datetime,
	modifier nvarchar(30) not null,
	modified datetime not null
)

create table ln.loanDocumentTemplatePage
(
	loanDocumentTemplatePageId int identity(1,1) not null primary key,
	loanDocumentTemplateId int not null,
	pageNumber int not null,
	content ntext not null,
	isNew bit default(1)
)

create table ln.loanDocumentTemplatePagePlaceHolder
(
	loanDocumentTemplatePagePlaceHolderId int identity(1,1) not null primary key,
	loanDocumentTemplatePageId int not null,
	placeHolderTypeId int not null
)

create table ln.loanDocumentPlaceHolderType
(
	loanDocumentPlaceHolderTypeId int identity(1,1) not null primary key,
	placeHolderTypeCode nvarchar(20) not null,
	entityTypeCode nvarchar(20) not null
)
