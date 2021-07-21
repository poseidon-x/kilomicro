use coreDB
go

create table gl.budget
(
	budgetID int identity (1,1) not null primary key,
	acct_id int not null,
	monthEndDate datetime not null,
	budgetAmount float not null,
	creator nvarchar(30) not null,
	creationDate datetime not null,
	modifier nvarchar(30) null,
	modificationDate datetime null,
	cost_center_id int null
)
go

alter table gl.budget add constraint
	uk_budget unique (acct_id, monthEndDate, cost_center_id)
go