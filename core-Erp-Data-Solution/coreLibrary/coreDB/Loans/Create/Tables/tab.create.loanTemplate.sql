use coreDB
go

create table ln.loanTemplate
(
	loanTemplateID int identity(1,1) primary key,
	templateName nvarchar(250) not null,
	templateBody ntext not null
)
go


create table ln.depositTemplate
(
	depositTemplateID int identity(1,1) primary key,
	templateName nvarchar(250) not null,
	templateBody ntext not null
)
go