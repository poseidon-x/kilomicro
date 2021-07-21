use coreDB
go

alter table iv.product add
	constraint fk_product_productSubCategory foreign key (productSubCategoryID)
	references iv.productSubCategory (productSubCategoryID)
go

alter table iv.product add
	constraint fk_product_inventoryMethod foreign key (inventoryMethodID)
	references iv.inventoryMethod (inventoryMethodID)
go

alter table iv.product add
	constraint fk_product_unitOfMeasurement foreign key (unitOfMeasurementID)
	references iv.unitOfMeasurement (unitOfMeasurementID)
go

alter table iv.product add
	constraint fk_product_productMake foreign key (productMakeID)
	references iv.productMake (productMakeID)
go

alter table iv.product add
	constraint fk_product_productStatus foreign key (productStatusID)
	references iv.productStatus (productStatusID)
go
