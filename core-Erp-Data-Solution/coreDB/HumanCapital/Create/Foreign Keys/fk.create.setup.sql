use coreDB
go

alter table hc.bonusType 
	add constraint fk_bonusType_bonusCalculationType foreign key(bonusCalculationTypeID)
	references hc.bonusCalculationType(bonusCalculationTypeID)
go

alter table hc.levelNotch 
	add constraint fk_levelNotch_level foreign key(levelID)
	references hc.[level](levelID)
go

alter table hc.levelAllowance 
	add constraint fk_levelAllowance_level foreign key(levelID)
	references hc.[level](levelID)
go

alter table hc.levelDeduction 
	add constraint fk_levelDeduction_level foreign key(levelID)
	references hc.[level](levelID)
go

alter table hc.levelAllowance 
	add constraint fk_levelAllowance_allowanceType foreign key(allowanceTypeID)
	references hc.allowanceType(allowanceTypeID)
go

alter table hc.levelDeduction 
	add constraint fk_levelDeduction_deductionType foreign key(deductionTypeID)
	references hc.deductionType(deductionTypeID)
go

alter table hc.levelBenefitsInKind 
	add constraint fk_levelBenefitsInKind_level foreign key(levelID)
	references hc.[level](levelID)
go

alter table hc.levelLeave 
	add constraint fk_levelLeave_level foreign key(levelID)
	references hc.[level](levelID)
go

alter table hc.levelLeave 
	add constraint fk_levelLeave_leaveType foreign key(leaveTypeID)
	references hc.[leaveType](leaveTypeID)
go
