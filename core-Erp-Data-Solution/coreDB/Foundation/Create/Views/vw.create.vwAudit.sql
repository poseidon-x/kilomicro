use coreDB
go

create view vwAudit
with encryption
as
select u2.full_name, module_name, actionDateTime, u.url, u.userName
from userAudit u inner join modules m on u.moduleID=m.module_id
	inner join users u2 on u2.user_name=u.userName 
go

alter view vwAudit
with encryption
as
select u2.full_name, module_name, actionDateTime, u.url, u.userName, allowed
from userAudit u left join modules m on u.moduleID=m.module_id
	inner join users u2 on u2.user_name=u.userName 
go