use coreDB
go


alter table ln.investment
	add constraint fk_investment_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.investment
	add constraint fk_investment_investmentType foreign key(investmentTypeID)
	references ln.investmentType(investmentTypeID)
go

alter table ln.investmentInterest
	add constraint fk_investmentInterest_investment foreign key(investmentID)
	references ln.investment(investmentID)
go

alter table ln.investmentWithdrawal
	add constraint fk_investmentWithdrawal_investment foreign key(investmentID)
	references ln.investment(investmentID)
go

alter table ln.investmentWithdrawal
	add constraint fk_investmentWithdrawal_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.investmentAdditional
	add constraint fk_investmentAdditional_investment foreign key(investmentID)
	references ln.investment(investmentID)
go

alter table ln.investmentAdditional
	add constraint fk_investmentAdditional_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.investmentSchedule
	add constraint fk_investmentSchedule_investment foreign key(investmentID)
	references ln.investment(investmentID)
go

alter table ln.investmentSignatory
	add constraint fk_investmentSignatory_investment foreign key(investmentID)
	references ln.investment(investmentID)
go

alter table ln.investmentSignatory
	add constraint fk_investmentSignatory_image foreign key(signatureImageID)
	references ln.[image](imageID)
go

alter table ln.investmentCharge add
	constraint fk_investmentCharge_chargeType foreign key (chargeTypeID)
	references ln.chargeType (chargeTypeID)
go

alter table ln.investmentCharge add
	constraint fk_investmentCharge_investment foreign key (investmentID)
	references ln.investment (investmentID)
go
