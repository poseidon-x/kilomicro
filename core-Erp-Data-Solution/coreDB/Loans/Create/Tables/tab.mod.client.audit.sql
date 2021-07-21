use coreDB
go

alter table ln.client add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go
 
alter table ln.client add
	secondSurName nvarchar(50)  null,
	secondOtherNames nvarchar(50)  null,
	thirdSurName nvarchar(50)  null,
	thrifOtherNames nvarchar(50)  null,
	accountName nvarchar(250) null
go