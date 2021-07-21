use coreDB
go

alter table iv.productSubCategory 
	add constraint fk_productSubCategory_productCategory foreign key (productCategoryID)
	references iv.productCategory(productCategoryID)
go