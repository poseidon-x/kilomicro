use coreDB
go

create view hc.vwStaff
with encryption as
select
	staffID,
	surName + ', ' + otherNames + ' (' + staffNo + ')' as staffName,
	staffNo,
	surName,
	otherNames,
	s.companyId,
	com.comp_name
from fa.staff s
Left Outer Join dbo.comp_prof com ON com.companyId = s.companyId

go

