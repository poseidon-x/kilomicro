use coreDB
go

--create schema man
--go
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

