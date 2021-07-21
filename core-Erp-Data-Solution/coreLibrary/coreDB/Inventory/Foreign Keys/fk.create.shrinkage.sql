use coreDB
go

alter table iv.shrinkage add constraint fk_shrinkage_inventoryItem
	foreign key (inventoryItemID) references iv.inventoryItem (inventoryItemID)
go

alter table iv.shrinkage add constraint fk_shrinkage_unitOfMeasurement
	foreign key (unitOfMeasurementID) references iv.unitOfMeasurement (unitOfMeasurementID)
go

alter table iv.shrinkage add constraint fk_shrinkage_batch
	foreign key (shrinkageBatchId) references iv.shrinkageBatch (shrinkageBatchId)
go

alter table iv.shrinkage add constraint fk_shrinkage_reason
	foreign key (shrinkageReasonId) references iv.shrinkageReason (shrinkageReasonId)
go
