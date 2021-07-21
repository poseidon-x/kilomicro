--use coreDB
--go

alter table hc.payMaster
	add constraint fk_payMaster_payCalendar foreign key (payCalendarID)
	references hc.payCalendar(payCalendarID)
go

alter table hc.payMaster
	add constraint fk_payMaster_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table hc.payMasterAllowance
	add constraint fk_payMasterAllowance_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterAllowance
	add constraint fk_payMasterAllowance_allowanceType foreign key (allowanceTypeID)
	references hc.allowanceType(allowanceTypeID)
go

alter table hc.payMasterDeduction
	add constraint fk_payMasterDeduction_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterDeduction
	add constraint fk_payMasterDeduction_deductionType foreign key (deductionTypeID)
	references hc.deductionType(deductionTypeID)
go

alter table hc.payMasterLoan
	add constraint fk_payMasterLoan_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterLoan
	add constraint fk_payMasterLoan_staffLoan foreign key (staffLoanID)
	references hc.staffLoan(staffLoanID)
go

alter table hc.staffCalendar
	add constraint fk_staffCalendar_payCalendar foreign key (payCalendarID)
	references hc.payCalendar(payCalendarID)
go

alter table hc.staffCalendar
	add constraint fk_staffCalendar_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table hc.payMasterPension
	add constraint fk_payMasterPension_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterPension
	add constraint fk_payMasterPension_pensionType foreign key (pensionTypeID)
	references hc.pensionType(pensionTypeID)
go

alter table hc.payMasterBenefitsInKind
	add constraint fk_BenefitsInKind_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterBenefitsInKind
	add constraint fk_payMasterBenefitsInKind_benefitsInKind foreign key (benefitsInKindID)
	references hc.benefitsInKind(benefitsInKindID)
go

alter table hc.payMasterOneTimeDeduction
	add constraint fk_payMasterOneTimeDeduction_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterOneTimeDeduction
	add constraint fk_payMasterOneTimeDeduction_deductionType foreign key (oneTimeDeductionTypeID)
	references hc.oneTimeDeductionType(oneTimeDeductionTypeID)
go

alter table hc.payMasterTax
	add constraint fk_payMasterTax_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterTaxRelief
	add constraint fk_payMasterTaxRelief_payMaster foreign key (payMasterID)
	references hc.payMaster(payMasterID)
go

alter table hc.payMasterTaxRelief
	add constraint fk_payMasterTaxRelief_taxReliefType foreign key (taxReliefTypeID)
	references hc.taxReliefType(taxReliefTypeID)
go

alter table hc.overTime
	add constraint fk_overTime_staff foreign key (staffID)
	references fa.staff(staffID)
go

alter table hc.overTime
	add constraint fk_overTime_payCalendar foreign key (payCalendarID)
	references hc.payCalendar(payCalendarID)
go

alter table hc.overTimeConfig
	add constraint fk_overTimeConfig_level foreign key (levelID)
	references hc.level(levelID)
go

alter table hc.payMasterOverTime
	add constraint fk_payMasterOverTime_payMaster foreign Key (payMasterID)
	references hc.payMaster(payMasterID)
go