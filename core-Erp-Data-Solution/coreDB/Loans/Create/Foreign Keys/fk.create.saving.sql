use coreDB
go


alter table ln.saving
	add constraint fk_saving_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.saving
	add constraint fk_saving_savingType foreign key(savingTypeID)
	references ln.savingType(savingTypeID)
go

alter table ln.savingInterest
	add constraint fk_savingInterest_saving foreign key(savingID)
	references ln.saving(savingID)
go

alter table ln.savingWithdrawal
	add constraint fk_savingWithdrawal_saving foreign key(savingID)
	references ln.saving(savingID)
go

alter table ln.savingWithdrawal
	add constraint fk_savingWithdrawal_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.savingAdditional
	add constraint fk_savingAdditional_saving foreign key(savingID)
	references ln.saving(savingID)
go

alter table ln.savingAdditional
	add constraint fk_savingAdditional_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.savingSchedule
	add constraint fk_savingSchedule_saving foreign key(savingID)
	references ln.saving(savingID)
go

alter table ln.savingRollover
	add constraint fk_savingRollover_saving foreign key(savingID)
	references ln.saving(savingID)
go

alter table ln.savingSignatory
	add constraint fk_savingSignatory_saving foreign key(savingID)
	references ln.saving(savingID)
go

alter table ln.savingSignatory
	add constraint fk_savingSignatory_image foreign key(signatureImageID)
	references ln.[image](imageID)
go

alter table ln.savingCharge add
	constraint fk_savingCharge_chargeType foreign key (chargeTypeID)
	references ln.chargeType (chargeTypeID)
go

alter table ln.savingCharge add
	constraint fk_savingCharge_deposit foreign key (savingID)
	references ln.saving (savingID)
go

alter table ln.savingPlan add
	constraint fk_savingPlan_saving foreign key (savingID)
	references ln.saving (savingID)
go

alter table ln.savingDailyInterest add
	constraint fk_savingDailyInterest_saving foreign key (savingID)
	references ln.saving(savingID)
go

alter table ln.savingPlanFlag add
	constraint fk_savingPlanFlag_saving foreign key (savingID)
	references ln.saving (savingID)
go

alter table ln.savingNextOfKin add
	constraint fk_savingNextOfKin_saving foreign key (savingId)
	references ln.saving(savingId)
go

alter table ln.savingNextOfKin add
	constraint fk_savingNextOfKin_idType foreign key (idTypeId)
	references ln.idNoType(idNoTypeId)
go
