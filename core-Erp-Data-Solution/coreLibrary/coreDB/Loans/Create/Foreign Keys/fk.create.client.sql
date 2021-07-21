use coreDB
go

alter table ln.client
	add constraint fk_client_maritalStatus foreign key(maritalStatusID)
	references ln.maritalStatus(maritalStatusID)
go
alter table ln.client
	add constraint fk_client_industry foreign key(industryID)
	references ln.industry(industryID)
go
	
alter table ln.client
	add constraint fk_client_sector foreign key(sectorID)
	references ln.sector(sectorID)
go
	
alter table ln.clientImage
	add constraint fk_client_client foreign key(clientID)
	references ln.client(clientID)
go
	
alter table ln.clientImage
	add constraint fk_client_image foreign key(imageID)
	references ln.[image](imageID)
go
	
alter table ln.client
	add constraint fk_client_branch foreign key(branchID)
	references ln.branch(branchID)
go
		
alter table ln.client
	add constraint fk_client_category foreign key(categoryID)
	references ln.category(categoryID)
go
		
alter table ln.client
	add constraint fk_client_idNo foreign key(idNoID)
	references ln.idNo(idNoID)
go
		
alter table ln.client
	add constraint fk_client_idNo2 foreign key(idNoID2)
	references ln.idNo(idNoID)
go
		
alter table ln.clientAddress
	add constraint fk_clientAddress_client foreign key(clientID)
	references ln.client(clientID)
go
		
alter table ln.clientAddress
	add constraint fk_clientAddress_address foreign key(addressID)
	references ln.address(addressID)
go

	
alter table ln.clientPhone
	add constraint fk_clientPhone_client foreign key(clientID)
	references ln.client(clientID)
go
		
alter table ln.clientPhone
	add constraint fk_clientPhone_phone foreign key(phoneID)
	references ln.phone(phoneID)
go


alter table ln.clientEmail
	add constraint fk_clientEmail_client foreign key(clientID)
	references ln.client(clientID)
go
		
alter table ln.clientEmail
	add constraint fk_clientEmail_email foreign key(emailID)
	references ln.email(emailID)
go

alter table ln.clientBusinessActivity
	add constraint fk_clientBusinessActivity_client foreign key(clientID)
	references ln.client(clientID)
go
		
alter table ln.clientBusinessActivity
	add constraint fk_clientBusinessActivity_businessType foreign key(businessTypeID)
	references ln.businessType(businessTypeID)
go

alter table ln.clientLiability
	add constraint fk_clientLiability_client foreign key(clientID)
	references ln.client(clientID)
go
		
alter table ln.clientLiability
	add constraint fk_clientLiability_institution foreign key(institutionID)
	references ln.institution(institutionID)
go
 
 alter table ln.smeDirector
	add constraint fk_smeDirector_image foreign key (imageID)
	references ln.[image](imageID)
go

alter table ln.clientCompany add
	constraint fk_clientCompany_client foreign key (clientID)
	references ln.client (clientID)
go

alter table ln.clientCompany add
	constraint fk_clientCompany_address foreign key (businessAddressID)
	references ln.[address] (addressID)
go

alter table ln.clientCompany add
	constraint fk_clientCompany_phone foreign key (phoneID)
	references ln.phone (phoneID)
go

alter table ln.clientCompany add
	constraint fk_clientCompany_email foreign key (emailID)
	references ln.email (emailID)
go
