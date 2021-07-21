use coreDB
go

alter table ln.loan add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go


alter table ln.loanGurantor add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go

alter table ln.loanCollateral add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go
 
alter table ln.loanTranch add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go
  
alter table ln.repaymentSchedule add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go 

alter table ln.loanPenalty add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go 
 
alter table ln.loanRepayment add
  creation_date datetime null default(getdate()) check(creation_date  <= dateadd(day,1,getdate())),
  creator nvarchar(50) not null  check(datalength(ltrim(rtrim(creator)))>0), 
  modification_date datetime null  check(modification_date is null or modification_date <= dateadd(day,1,getdate())),
  last_modifier nvarchar(50) null  check(last_modifier is null or datalength(ltrim(rtrim(last_modifier)))>0)
go 
  

alter table ln.loan add
	approvedBy nvarchar(100),
	checkedBy nvarchar(100),
	enteredBy nvarchar(100)
go

 

alter table ln.loan add
	disbursedBy nvarchar(100)
go