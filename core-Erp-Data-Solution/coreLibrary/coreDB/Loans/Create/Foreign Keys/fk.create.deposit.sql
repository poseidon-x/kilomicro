use coreDB
go


alter table ln.deposit
	add constraint fk_deposit_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.deposit
	add constraint fk_deposit_depositType foreign key(depositTypeID)
	references ln.depositType(depositTypeID)
go

alter table ln.depositInterest
	add constraint fk_depositInterest_deposit foreign key(depositID)
	references ln.deposit(depositID)
go

alter table ln.depositWithdrawal
	add constraint fk_depositWithdrawal_deposit foreign key(depositID)
	references ln.deposit(depositID)
go

alter table ln.depositWithdrawal
	add constraint fk_depositWithdrawal_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.depositAdditional
	add constraint fk_depositAdditional_deposit foreign key(depositID)
	references ln.deposit(depositID)
go

alter table ln.depositAdditional
	add constraint fk_depositAdditional_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.depositSchedule
	add constraint fk_depositSchedule_deposit foreign key(depositID)
	references ln.deposit(depositID)
go

alter table ln.depositSignatory
	add constraint fk_depositSignatory_deposit foreign key(depositID)
	references ln.deposit(depositID)
go

alter table ln.depositSignatory
	add constraint fk_depositSignatory_image foreign key(signatureImageID)
	references ln.[image](imageID)
go

alter table ln.depositCharge add
	constraint fk_depositCharge_chargeType foreign key (chargeTypeID)
	references ln.chargeType (chargeTypeID)
go

alter table ln.depositCharge add
	constraint fk_depositCharge_deposit foreign key (depositID)
	references ln.deposit (depositID)
go

alter table ln.depositTypeAllowedTenure add
	constraint fk_depositTypeAllowedTenure_depositType foreign key(depositTypeId)
	references ln.depositType(depositTypeID)

alter table ln.depositTypeAllowedTenure add
	constraint fk_depositTypeAllowedTenure_tenureType foreign key(tenureTypeId)
	references ln.tenureType(tenureTypeID)



alter table ln.clientCheck add
	constraint fk_clientCheck_client foreign key(clientId)
	references ln.client(clientID)

alter table ln.clientCheckDetail add
	constraint fk_clientCheckDetail_clientCheck foreign key(clientCheckId)
	references ln.clientCheck(clientCheckId)

alter table ln.clientCheckDetail add
	constraint fk_clientCheckDetail_bankId foreign key(bankId)
	references dbo.banks(bank_id)

alter table ln.clientCheckDetail add
	constraint fk_clientCheckDetail_bank_accts foreign key(sourceBankAccountId)
	references dbo.bank_accts(bank_acct_id)

alter table ln.checkApply add
	constraint fk_checkApply_clientCheckDetail foreign key(clientCheckDetailId)
	references ln.clientCheckDetail(clientCheckDetailId)

alter table ln.checkApply add
	constraint fk_checkApply_deposit foreign key(depositId)
	references ln.deposit(depositID)



--add by Manager 19-OCT-2015
alter table ln.clientInvestmentReceipt add
	constraint fk_clientInvestmentReceipt_client foreign key(clientId)
	references ln.client(clientID)

alter table ln.clientInvestmentReceiptDetail add
	constraint fk_clientInvestmentReceiptDetail_clientInvestmentReceipt foreign key(clientInvestmentReceiptId)
	references ln.clientInvestmentReceipt(clientInvestmentReceiptId)
