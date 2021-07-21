--use coreDB
--go

alter table hc.staffQualification
	add constraint fk_staffQualification_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffQualification
	add constraint fk_staffQualification_qualificationType foreign key (qualificationTypeID)
	references hc.qualificationType(qualificationTypeID)
go

alter table hc.staffQualification
	add constraint fk_staffQualification_qualificationSubject foreign key (qualificationSubjectID)
	references hc.qualificationSubject(qualificationSubjectID)
go

alter table hc.staffRelation
	add constraint fk_staffRelation_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffRelation
	add constraint fk_staffRelation_relationType foreign key (relationTypeID)
	references hc.relationType(relationTypeID)
go

alter table hc.staffAllowance
	add constraint fk_sstaffAllowance_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffAllowance
	add constraint fk_staffAllowance_allowanceType foreign key (allowanceTypeID)
	references hc.allowanceType(allowanceTypeID)
go

alter table hc.staffDeduction
	add constraint fk_sstaffDeduction_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffDeduction
	add constraint fk_staffDeduction_deductionType foreign key (deductionTypeID)
	references hc.deductionType(deductionTypeID)
go

alter table hc.staffManager
	add constraint fk_stafManager_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffManager
	add constraint fk_stafManager_managerStaff foreign key (managerStaffID)
	references [fa].staff(staffID)
go

alter table hc.staffPension
	add constraint fk_staffPension_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffPension
	add constraint fk_staffPension_pensionType foreign key (pensionTypeID)
	references hc.pensionType(pensionTypeID)
go

alter table hc.staffBenefit
	add constraint fk_staffBenefit_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffManager
	add constraint fk_staffManager_level foreign key (levelID)
	references hc.[level](levelID)
go

alter table hc.staffManager
	add constraint fk_staffManager_levelNotch foreign key (levelNotchID)
	references hc.[levelNotch](levelNotchID)
go

alter table hc.staffOneTimeDeduction
	add constraint fk_staffOneTimeDeduction_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffOneTimeDeduction
	add constraint fk_staffOneTimeDeduction_oneTimeDeductionType foreign key (oneTimeDeductionTypeID)
	references hc.oneTimeDeductionType(oneTimeDeductionTypeID)
go

alter table hc.staffOneTimeDeduction
	add constraint fk_staffOneTimeDeduction_payCalendar foreign key (payCalendarID)
	references hc.payCalendar(payCalendarID)
go

alter table hc.staffBenefitsInKind
	add constraint fk_BenefitsInKind_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffBenefitsInKind
	add constraint fk_BenefitsInKind_allowanceType foreign key (benefitsInKindID)
	references hc.benefitsInKind(benefitsInKindID)
go

alter table hc.staffTaxRelief
	add constraint fk_staffTaxRelief_staff foreign key (staffID)
	references [fa].staff(staffID)
go

alter table hc.staffTaxRelief
	add constraint fk_staffTaxRelief_taxReliefType foreign key (taxReliefTypeID)
	references hc.taxReliefType(taxReliefTypeID)
go
