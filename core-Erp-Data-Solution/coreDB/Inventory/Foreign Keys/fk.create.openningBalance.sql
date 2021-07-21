use coreDB
go

alter table iv.openningBalance add constraint fk_openningBalance_inventoryItem
	foreign key (inventoryItemID) references iv.inventoryItem (inventoryItemID)
go

alter table iv.openningBalance add constraint fk_openningBalance_unitOfMeasurement
	foreign key (unitOfMeasurementID) references iv.unitOfMeasurement (unitOfMeasurementID)
go

alter table iv.openningBalance add constraint fk_openningBalance_openningBalanceBatch
	foreign key (openningBalanceBatchId) references iv.openningBalanceBatch (openningBalanceBatchId)
go
