use coreDB
go

alter table ln.nextOfKin
	add constraint fk_nextOfKin_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.employeeCategory
	add constraint fk_employeeCategory_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.employeeCategory
	add constraint fk_employeeCategory_employer foreign key(employerID)
	references ln.employer(employerID)
go

alter table ln.employeeCategory
	add constraint fk_employeeCategory_employerDirector foreign key(employerDirectorID)
	references ln.employerDirector(employerDirectorID)
go

alter table ln.employer
	add constraint fk_employer_employmentType foreign key(employmentTypeID)
	references ln.employmentType(employmentTypeID)
go
 
alter table ln.employer
	add constraint fk_employer_employerAddress foreign key(employerAddressID)
	references ln.[address](addressID)
go
 
alter table ln.employerDirector
	add constraint fk_employerDirector_employer foreign key(employerID)
	references ln.employer(employerID)
go
 
alter table ln.employerDirector
	add constraint fk_employerDirector_idNo foreign key(idNoID)
	references ln.idNo(idNoID)
go
   
alter table ln.nextOfKin
	add constraint fk_nextOfKin_idNo foreign key(idNoID)
	references ln.idNo(idNoID)
go
  
alter table ln.employerDirector
	add constraint fk_employerDirector_phone foreign key(phoneID)
	references ln.phone(phoneID)
go
 
alter table ln.employerDirector
	add constraint fk_employerDirector_email foreign key(emailID)
	references ln.email(emailID)
go
 
alter table ln.employerDirector
	add constraint fk_employerDirector_signatureImage foreign key(signatureImageID)
	references ln.[image](imageID)
go
 
alter table ln.nextOfKin
	add constraint fk_nextOfKin_image foreign key(imageID)
	references ln.[image](imageID)
go
 
alter table ln.nextOfKin
	add constraint fk_nextOfKin_phone foreign key(phoneID)
	references ln.phone(phoneID)
go
 
alter table ln.nextOfKin
	add constraint fk_snextOfKin_email foreign key(emailID)
	references ln.email(emailID)
go
  
alter table ln.smeDirector
	add constraint fk_smeDirector_idNo foreign key(idNoID)
	references ln.idNo(idNoID)
go
  
alter table ln.smeDirector
	add constraint fk_smeDirector_phone foreign key(phoneID)
	references ln.phone(phoneID)
go
 
alter table ln.smeDirector
	add constraint fk_smeDirector_email foreign key(emailID)
	references ln.email(emailID)
go
  
alter table ln.microBusinessCategory
	add constraint fk_microBusinessCategory_client foreign key(clientID)
	references ln.client(clientID)
go
   
alter table ln.microBusinessCategory
	add constraint fk_microBusinessCategory_lineOfBusiness foreign key(lineOfBusinessID)
	references ln.lineOfBusiness(lineOfBusinessID)
go
 
alter table ln.smeCategory
	add constraint fk_smeCategory_client foreign key(clientID)
	references ln.client(clientID)
go
 
alter table ln.smeDirector
	add constraint fk_smeDirector_smeCategory foreign key(smeCategoryID)
	references ln.smeCategory(smeCategoryID)
go
 
alter table ln.groupCategory
	add constraint fk_groupCategory_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.groupExec
	add constraint fk_groupExec_group foreign key(groupID)
	references ln.[group](groupID)
go
  
alter table ln.groupCategory
	add constraint fk_groupCategory_group foreign key(groupID)
	references ln.[group](groupID)
go
  
alter table ln.smeCategory
	add constraint fk_smeCategory_regAddress foreign key(registeredAddressID)
	references ln.[address](addressID)
go
  
alter table ln.smeCategory
	add constraint fk_smeCategory_phyAddress foreign key(physicalAddressID)
	references ln.[address](addressID)
go
 
alter table ln.employeeCategory
	add constraint fk_employeeCategory_employerAddress foreign key(employerAddressID)
	references ln.[address](addressID)
go
 
alter table ln.[group]
	add constraint fk_group_Address foreign key(addressID)
	references ln.[address](addressID)
go
 
alter table ln.[groupExec]
	add constraint fk_group_phone foreign key(phoneID)
	references ln.phone(phoneID)
go

alter table ln.[groupExec]
	add constraint fk_group_email foreign key(emailID)
	references ln.email(emailID)
go

alter table ln.staffCategory
	add constraint fk_staffCategory_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.staffCategory
	add constraint fk_staffCategory_employeeContractType foreign key(employeeContractTypeID)
	references ln.employeeContractType(employeeContractTypeID)
go

alter table ln.staffCategory
	add constraint fk_staffCategory_employer foreign key(employerID)
	references ln.employer(employerID)
go

alter table ln.staffCategory
	add constraint fk_staffCategory_employerDepartment foreign key(employerDepartmentID)
	references ln.employerDepartment(employerDepartmentID)
go

alter table ln.employerDepartment
	add constraint fk_employerDepartment_employer foreign key(employerID)
	references ln.employer(employerID)
go

alter table ln.clientBankAccount
	add constraint fk_clientBankAccount_accountType foreign key (accountTypeID)
	references dbo.bankAccountType(accountTypeID)
go

alter table ln.clientBankAccount
	add constraint fk_clientBankAccount_branch foreign key (branchID)
	references dbo.bank_branches(branch_id)
go

alter table ln.clientBankAccount
	add constraint fk_clientBankAccount_client foreign key (clientID)
	references ln.client(clientID)
go

alter table ln.staffCategoryDirector
	add constraint fk_staffCategoryDirector_employerDirector foreign key(employerDirectorID)
	references ln.employerDirector(employerDirectorID)
go

alter table ln.staffCategoryDirector
	add constraint fk_staffCategoryDirector_staffCategory foreign key(staffCategoryID)
	references ln.staffCategory(staffCategoryID)
go

alter table ln.staffCategory	
	add constraint fk_staffCategory_region foreign key (regionID)
	references ln.region(regionID)
go