use coreDB
go

alter table ln.controllerFileDetail add
	constraint fk_controllerFileDetail_controllerFile foreign key (fileID)
	references ln.controllerFile (fileID)
go

alter table ln.controllerFileDetail add
	constraint fk_controllerFileDetail_repaymentSchedule foreign key (repaymentScheduleID)
	references ln.repaymentSchedule (repaymentScheduleID)
go
