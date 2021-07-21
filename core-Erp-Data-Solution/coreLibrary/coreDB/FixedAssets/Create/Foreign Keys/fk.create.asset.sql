use coreDB
go

alter table fa.asset
	add constraint fk_asset_assetSubCategory foreign key(assetSubCategoryID)
	references fa.assetSubCategory(assetSubCategoryID)
go 

alter table fa.asset
	add constraint fk_asset_staff foreign key(staffID)
	references fa.staff(staffID)
go 

alter table fa.assetImage
	add constraint fk_asset_asset foreign key(assetID)
	references fa.asset(assetID)
go
	
alter table fa.assetImage
	add constraint fk_asset_image foreign key(imageID)
	references ln.[image](imageID)
go
	

alter table fa.assetDocument add
	constraint fk_assetDocument_asset foreign key (assetID)
	references fa.asset(assetID)
go

alter table fa.assetDocument add
	constraint fk_assetDocument_document foreign key (documentID)
	references ln.document(documentID)
go


alter table fa.assetDepreciation add
	constraint fk_assetDepreciation_asset foreign key (assetID)
	references fa.asset(assetID)
go

alter table fa.depreciationSchedule add
	constraint fk_depreciationSchedule_asset foreign key (assetID)
	references fa.asset(assetID)
go
