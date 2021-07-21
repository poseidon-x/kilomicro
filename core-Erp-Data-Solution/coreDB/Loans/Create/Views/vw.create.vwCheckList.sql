use coreDB
go

alter  view ln.vwCheckList
with encryption as
select	distinct
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	isnull(rs.[description], isnull(ccl.[description],'')) as [description],
	isnull(passed,0) as passed,
	cast(isnull(comments,'No Comments') as nvarchar(4000)) as comments,
	isnull(c.companyId,0)as CompanyId,
	isnull(company.comp_Name, '') AS comp_Name
from ln.client c 
	inner join ln.loan l on c.clientID=l.clientID
	left outer join ln.loanCheckList rs on l.loanID=rs.loanID
	left outer join ln.categoryCheckList ccl on c.categoryID = ccl.categoryID
	left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id

go