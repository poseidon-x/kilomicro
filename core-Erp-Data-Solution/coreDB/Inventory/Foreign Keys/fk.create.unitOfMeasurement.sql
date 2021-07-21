use coreDB
go

alter table iv.unitOfMeasurement add
	constraint fk_unitOfMeasurement_unitOfMeasurement foreign key (complexDetailUnitOfMeasurementID)
	references iv.unitOfMeasurement (unitOfMeasurementID)
go