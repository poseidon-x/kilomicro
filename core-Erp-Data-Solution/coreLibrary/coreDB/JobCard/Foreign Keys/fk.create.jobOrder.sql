use coreDB
go

alter table jc.jobCard add
	constraint fk_jobCard_customer foreign key (customerId)
	references crm.customer (customerId)
go

alter table jc.jobCardLabourDetail add
	constraint fk_jobCardLabourDetail_jobCard foreign key (jobCardId)
	references jc.jobCard (jobCardId)
go

alter table jc.jobCardMaterialDetail add
	constraint fk_jobCardMaterialDetail_jobCard foreign key (jobCardId)
	references jc.jobCard (jobCardId)
go

alter table jc.workOrder add
	constraint fk_workOrder_customer foreign key (customerId)
	references crm.customer (customerId)
go

alter table jc.workOrderActivity add
	constraint fk_workOrderActivity_speciality foreign key (specialityId)
	references jc.speciality (specialityId)
go

alter table jc.speciality add
	constraint fk_speciality_specialityCategory foreign key (specialityCategoryId)
	references jc.specialityCategory (specialityCategoryId)
go

alter table jc.jobCardMaterialDetail add
	constraint fk_jobCardMaterialDetail_unitOfMeasurement foreign key (unitOfMeasurementId)
	references iv.unitOfMeasurement (unitOfMeasurementId)
go

alter table jc.workOrderActivity add
	constraint fk_workOrderActivity_workOrder foreign key (workOrderId)
	references jc.workOrder (workOrderId)
go


