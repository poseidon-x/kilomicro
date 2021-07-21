use coreDB
go

alter table hc.performanceContract
	add constraint fk_performanceContract_staff foreign key (staffID)
	references fa.staff (staffID)
go

alter table hc.performanceContract
	add constraint fk_performanceContract_performanceContractStatus foreign key (performanceContractStatusID)
	references hc.performanceContractStatus (performanceContractStatusID)
go

alter table hc.performanceContractItem
	add constraint fk_performanceContractItem_performanceContract foreign key (performanceContractID)
	references hc.performanceContract (performanceContractID)
go

alter table hc.performanceContractItem
	add constraint fk_performanceContractItem_performanceArea foreign key (performanceAreaID)
	references hc.performanceArea (performanceAreaID)
go

alter table hc.performanceContractTarget
	add constraint fk_performanceContractTarget_performanceContractItem foreign key (performanceContractItemID)
	references hc.performanceContractItem (performanceContractItemID)
go

alter table hc.performanceContractTarget
	add constraint fk_performanceContractTarget_performanceScore foreign key (performanceScoreID)
	references hc.performanceScore (performanceScoreID)
go

alter table hc.performanceAppraisal
	add constraint fk_performanceAppraisal_performanceContract foreign key (performanceContractID)
	references hc.performanceContract (performanceContractID)
go

alter table hc.performanceAppraisal
	add constraint fk_performanceAppraisal_performanceAppraisalType foreign key (performanceAppraisalTypeID)
	references hc.performanceAppraisalType (performanceAppraisalTypeID)
go

alter table hc.performanceAppraisal
	add constraint fk_performanceAppraisal_managerStaff foreign key (managerStaffID)
	references fa.staff (staffID)
go

alter table hc.performanceAppraisalScore
	add constraint fk_performanceAppraisalScore_performanceAppraisal foreign key (performanceAppraisalID)
	references hc.performanceAppraisal (performanceAppraisalID)
go

alter table hc.performanceAppraisalScore
	add constraint fk_performanceAppraisalScore_performanceScore foreign key (performanceScoreID)
	references hc.performanceScore (performanceScoreID)
go

alter table hc.performanceAppraisalScore
	add constraint fk_performanceAppraisalScore_performanceContractItem foreign key (performanceContractItemID)
	references hc.performanceContractItem (performanceContractItemID)
go
