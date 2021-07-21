use coreDB
go

alter view vwCollateral
with encryption as
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber,
	l.loanID,
	l.loanNo,
	lc.fairValue,
	ct.collateralTypeName,
	l.applicationDate,
	ct.collateralTypeID,
	lc.[collateraldescription],
	isnull(c.companyId,0)as CompanyId,
    isnull(company.comp_Name, '') AS comp_Name
from ln.loanCollateral lc inner join ln.loan l on lc.loanID=l.loanID
	inner join ln.client c on l.clientID=c.clientID
	inner join ln.collateralType ct on lc.collateralTypeID=ct.collateralTypeID
	left join dbo.comp_prof AS company on c.companyId = company.companyId

go