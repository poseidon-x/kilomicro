use coreDB
go

alter table man.assemblyLine add
	constraint fk_assemblyLine_assemblyLineType foreign key (assemblyLineTypeId)
	references man.assemblyLineType (assemblyLineTypeId)
go

alter table man.assemblyLine add
	constraint fk_assemblyLine_productId foreign key (endProductId)
	references iv.product (productId)
go

alter table man.assemblyLine add
	constraint fk_assemblyLine_factory foreign key (factoryId)
	references man.factory (factoryId)
go

alter table man.assemblyLine add
	constraint fk_assemblyLine_supervisorStaffId foreign key (supervisorStaffId)
	references fa.staff (staffID)
go

alter table man.assemblyLine add
	constraint fk_assemblyLine_assemblyLineType foreign key (assemblyLineTypeId)
	references man.assemblyLineType (assemblyLineTypeId)
go

alter table man.assemblyWorkStage add
	constraint fk_assemblyWorkStage_assemblyLine foreign key (assemblyLineId)
	references man.assemblyLine (assemblyLineId)
go

alter table man.assemblyWorkStage add
	constraint fk_assemblyWorkStage_workStageType foreign key (workStageTypeId)
	references man.workStageType (workStageTypeId)
go

alter table man.assemblyLineStaff add
	constraint fk_assemblyLineStaff_assemblyLine foreign key (assemblyLineId)
	references man.assemblyLine (assemblyLineId)
go

alter table man.assemblyLineStaff add
	constraint fk_assemblyLineStaff_employeeStaff foreign key (employeeStaffId)
	references fa.staff (staffID)
go

alter table man.assemblyLineStaff add
	constraint fk_assemblyLineStaff_assemblyWorkStage foreign key (assemblyWorkStageId)
	references man.assemblyWorkStage (assemblyWorkStageId)
go

alter table man.factory add
	constraint fk_factory_factoryType foreign key (factoryTypeId)
	references man.factoryType (factoryTypeId)
go




alter table man.durationType add
	constraint fk_durationType_durationType foreign key(detailDurationTypeId) 
	references man.durationType(durationTypeId)
go

alter table man.manufacturingCalender add
	constraint fk_manufacturingCalender_assemblyLine foreign key(assemblyLineId) 
	references man.assemblyLine(assemblyLineId)
go
	
alter table man.manufacturingCalender add
	constraint fk_manufacturingCalender_product foreign key(productId) 
	references iv.product(productId)
go
	
alter table man.manufacturingCalender add
	constraint fk_manufacturingCalender_durationType foreign key(durationTypeId) 
	references man.durationType(durationTypeId)
go

alter table man.manufacturingCalender add
	constraint fk_manufacturingCalender_unitOfMeasurement foreign key(unitOfMeasurementId) 
	references iv.unitOfMeasurement(unitOfMeasurementId)
go
	
alter table man.actualPerfomance add
	constraint fk_actualPermance_manufacturingCalender foreign key(manufacturingCalenderId) 
	references man.manufacturingCalender(manufacturingCalenderId)
go
	
alter table man.actualPerfomance add
	constraint fk_actualPermance_durationType foreign key(durationTypeId) 
	references man.durationType(durationTypeId)
go

alter table man.actualPerfomance add
	constraint fk_actualPerfomance_unitOfMeasurement foreign key(unitOfMeasurementId) 
	references iv.unitOfMeasurement(unitOfMeasurementId)
go

alter table man.manufacturingScrap add
	constraint fk_manufacturingScrap_scrapReason foreign key(scrapReasonId) 
	references man.scrapReason(scrapReasonId)
go

alter table man.manufacturingScrap add
	constraint fk_manufacturingScrap_actualPerfomance foreign key(actualPerfomanceId) 
	references man.actualPerfomance(actualPerfomanceId)
go

alter table man.manufacturingScrap add
	constraint fk_manufacturingScrap_unitOfMeasurement foreign key(unitOfMeasurementId) 
	references iv.unitOfMeasurement(unitOfMeasurementId)
go
	
alter table man.manufacturingCalenderStaff add
	constraint fk_manufacturingCalenderStaff_manufacturingCalender foreign key(manufacturingCalenderId) 
	references man.manufacturingCalender(manufacturingCalenderId)
go

alter table man.manufacturingCalenderStaff add
	constraint fk_manufacturingCalenderStaff_staff foreign key(employeeStaffId) 
	references fa.staff (staffID)
go

alter table man.billOfMaterial add
	constraint fk_billOfMaterial_product foreign key (productId) 
	references iv.product(productId)
go

alter table man.billOfMaterialDetail add
	constraint fk_billOfMaterialDetail_billOfMaterial foreign key(billOfMaterialId) 
	references man.billOfMaterial(billOfMaterialId)
go
	
alter table man.billOfMaterialDetail add
	constraint fk_billOfMaterialDetail_product foreign key(productId) 
	references iv.product(productId)
go
	
alter table man.billOfMaterialDetail add
	constraint fk_billOfMaterialDetail_unitOfMeasurement foreign key(unitOfMeasurementId) 
	references iv.unitOfMeasurement(unitOfMeasurementId)
go
	
