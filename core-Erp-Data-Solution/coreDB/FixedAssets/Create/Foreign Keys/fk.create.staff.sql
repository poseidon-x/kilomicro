use coreDB
go

alter table fa.staff
	add constraint fk_staff_maritalStatus foreign key(maritalStatusID)
	references ln.maritalStatus(maritalStatusID)
go 

	
alter table fa.staff
	add constraint fk_staff_users foreign key(userName)
	references dbo.users(user_name)
go 

alter table fa.staffImage
	add constraint fk_staff_staff foreign key(staffID)
	references fa.staff(staffID)
go
	
alter table fa.staffImage
	add constraint fk_staff_image foreign key(imageID)
	references ln.[image](imageID)
go
	
alter table fa.staff
	add constraint fk_staff_branch foreign key(branchID)
	references ln.branch(branchID)
go
		
alter table fa.staff
	add constraint fk_staff_staffCategory foreign key(staffCategoryID)
	references fa.staffCategory(staffCategoryID)
go
 	
alter table fa.staffAddress
	add constraint fk_staffAddress_staff foreign key(staffID)
	references fa.staff(staffID)
go
		
alter table fa.staffAddress
	add constraint fk_staffAddress_address foreign key(addressID)
	references ln.address(addressID)
go

	
alter table fa.staffPhone
	add constraint fk_staffPhone_staff foreign key(staffID)
	references fa.staff(staffID)
go
		
alter table fa.staffPhone
	add constraint fk_staffPhone_phone foreign key(phoneID)
	references ln.phone(phoneID)
go


alter table fa.staffEmail
	add constraint fk_staffEmail_staff foreign key(staffID)
	references fa.staff(staffID)
go
		
alter table fa.staffEmail
	add constraint fk_staffEmail_email foreign key(emailID)
	references ln.email(emailID)
go



alter table fa.staffDocument add
	constraint fk_staffDocument_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table fa.staffDocument add
	constraint fk_staffDocument_document foreign key (documentID)
	references ln.document(documentID)
go

alter table fa.assetSubCategory add
	constraint fk_assetSubCategory_assetCategory foreign key (assetCategoryID)
	references fa.assetCategory(assetCategoryID)
go

alter table fa.staff add
	constraint fk_staff_employmentStatus foreign key (employmentStatusID)
	references hc.employmentStatus(employmentStatusID)
go