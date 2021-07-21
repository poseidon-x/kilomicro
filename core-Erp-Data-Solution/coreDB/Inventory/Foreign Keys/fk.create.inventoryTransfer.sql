use coreDB
go

alter table iv.inventoryTransfer add
	constraint fk_inventoryTransfer_fromLocation foreign key (fromLocationID)
	references iv.location (LocationID)
go

alter table iv.inventoryTransfer add
	constraint fk_inventoryTransfer_toLocation foreign key (toLocationID)
	references iv.location (LocationID)
go

alter table iv.inventoryTransferDetail add
	constraint fk_inventoryTransferDetail_inventoryTransfer foreign key (inventoryTransferID)
	references iv.inventoryTransfer (inventoryTransferID)
go

alter table iv.inventoryTransferDetail add
	constraint fk_inventoryTransferDetail_inventoryItem foreign key (inventoryItemID)
	references iv.inventoryItem (inventoryItemID)
go

alter table iv.inventoryTransferDetailLine add
	constraint fk_inventoryTransferDetailLine_inventoryTransferDetail foreign key (inventoryTransferDetailID)
	references iv.inventoryTransferDetail (inventoryTransferDetailID)
go
