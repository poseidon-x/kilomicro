use coreDB
go

alter view ln.vwClients
with encryption
as
select
	c.clientID,
	isnull(case when (c.clientTypeID=3 or c.clientTypeID=4 or c.clientTypeID=5) 
		then c.companyName when (c.clientTypeID = 6) 
		then c.accountName else c.surName + ' ' + c.otherNames end,'')  as clientName,
	c.accountNumber, 
	isnull((select top 1 p.addressLine1 from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=2 and cp.clientID=c.clientID), '') addressLine1,
	isnull((select top 1 p.addressLine2 from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=2 and cp.clientID=c.clientID), '') addressLine2,
	isnull((select top 1 p.addressLine1 from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=1 and cp.clientID=c.clientID),'') as directions,
	isnull((select top 1 p.cityTown from ln.clientAddress cp inner join ln.[address] p on cp.addressID = p.addressID where cp.addressTypeID=2 and cp.clientID=c.clientID), '') cityTown,
	isnull((select  top 1 p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=1 and cp.clientID=c.clientID),'') as workPhone,
	isnull((select  top 1 p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=2 and cp.clientID=c.clientID),'') as mobilePhone,
	isnull((select  top 1 p.phoneNo from ln.clientPhone cp inner join ln.phone p on cp.phoneID = p.phoneID where cp.phoneTypeID=3 and cp.clientID=c.clientID),'') as homePhone,
	isnull((select  top 1 p.emailAddress from ln.clientEmail cp inner join ln.email p on cp.emailID = p.emailID where cp.emailTypeID=1 and cp.clientID=c.clientID),'') as officeEmail,
	isnull((select  top 1 p.emailAddress from ln.clientEmail cp inner join ln.email p on cp.emailID = p.emailID where cp.emailTypeID=2 and cp.clientID=c.clientID), '') as personalEmail,
	isnull((select top 1 [image] from ln.clientImage ai inner join ln.[image] i on ai.imageID = i.imageID and ai.clientID = c.clientID), 0x0) as [image],
	isnull(DoB, getdate()) as DOB,
	isnull(cat.categoryName,'')categoryName,
	isnull(branchID,0) branchID,
	isnull(c.companyId,0)as CompanyId,
    isnull(company.comp_Name, '') AS comp_Name
from ln.client c 
	left outer join ln.category cat on c.categoryID=cat.categoryID
	left join dbo.comp_prof AS company on c.companyId = company.comp_prof_id
 
go