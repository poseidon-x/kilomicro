use coreDB
go

alter view ln.vwWriteOff
with encryption as
select
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end  as clientName,
	c.accountNumber, 
	a.addressLine1,
	a.addressLine2,
	a.cityTown,
	(select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=1 and cp.clientID=c.clientID) as workPhone,
	(select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=2 and cp.clientID=c.clientID) as mobilePhone,
	(select p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=3 and cp.clientID=c.clientID) as homePhone,
	(select p.emailAddress from ln.clientEmail cp inner join ln.email p on cp.emailID = p.emailID where cp.emailTypeID=1 and cp.clientID=c.clientID) as officeEmail,
	(select p.emailAddress from ln.clientEmail cp inner join ln.email p on cp.emailID = p.emailID where cp.emailTypeID=2 and cp.clientID=c.clientID) as personalEmail,
	(select top 1 [image] from ln.clientImage ai inner join ln.[image] i on ai.imageID = i.imageID and ai.clientID = c.clientID) as [image],
	isnull(DoB, getDate()) as DoB,
	cat.categoryName,
	(select max(rp.repaymentDate) from ln.loanRepayment rp where rp.loanID=l.loanID) as repaymentDate,
	sum(rs.proposedInterestWriteOff) as proposedInterestWriteOff,
	 isnull(c.companyId,0)as companyID , 
	 isnull(company.comp_name,'')as comp_Name
from ln.client c 
	left outer join ln.clientAddress ca on c.clientID=ca.clientID
	left outer join ln.address a on ca.addressID = a.addressID
	left outer join ln.category cat on c.categoryID=cat.categoryID
	inner join ln.loan l on c.clientID=l.clientID
	inner join ln.repaymentSchedule rs on l.loanID=rs.loanID
	left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
where (ca.addressID is null or ca.addressTypeID = 1) and rs.proposedInterestWriteOff>0
group by 
	c.clientID,
	case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) then c.companyName when (c.clientTypeID = 6) then c.accountName else c.surName + ', ' + c.otherNames end,
	c.accountNumber, 
	a.addressLine1,
	a.addressLine2,
	a.cityTown,
	isnull(DoB, getDate()),
	cat.categoryName,
	l.loanID,
	 isnull(c.companyId,0), 
	 isnull(company.comp_name,'')
go