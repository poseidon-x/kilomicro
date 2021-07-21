use coreDB
go

alter table iv.inventoryItem add
	constraint fk_inventoryItem_product foreign key (productID)
	references iv.product (productID)
go

alter table iv.inventoryItem add
	constraint fk_inventoryItem_location foreign key (locationID)
	references iv.location (locationID)
go

alter table iv.inventoryItem add
	constraint fk_inventoryItem_brand foreign key (brandID)
	references iv.brand (brandID)
go

alter table iv.inventoryItemDetail add
	constraint fk_inventoryItemDetail_inventoryItem foreign key (inventoryItemID)
	references iv.inventoryItem (inventoryItemID)
go
