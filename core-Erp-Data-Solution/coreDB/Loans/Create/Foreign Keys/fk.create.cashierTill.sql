use coreDB
go

alter table ln.multiPayment add constraint
	fk_multiPayment_multiPaymentClient foreign key(multiPaymentClientID)
	references ln.multiPaymentClient(multiPaymentClientID)
go

alter table ln.multiPaymentClient add constraint
	fk_multiPaymentClient_cashiersReceipt foreign key(cashierReceiptID)
	references ln.cashierReceipt(cashierReceiptID)
go

alter table ln.multiPayment add constraint
	fk_multiPayment_cashiersReceipt foreign key(cashierReceiptID)
	references ln.cashierReceipt(cashierReceiptID)
go

alter table ln.cashiersTill add constraint
	fk_cashiersTill_users foreign key(userName)
	references dbo.users(user_name)
go

alter table ln.cashiersTill add constraint
	fk_cashiersTill_accts foreign key(accountID)
	references dbo.accts(acct_id)
go

alter table ln.cashiersTillDay add constraint
	fk_cashiersTillDay_cashiersTill foreign key(cashiersTillID)
	references ln.cashiersTill(cashiersTillID)
go

alter table ln.cashierDisbursement add constraint
	fk_cashierDisbursement_cashiersTill foreign key(cashierTillID)
	references ln.cashiersTill(cashiersTillID)
go

alter table ln.cashierReceipt add constraint
	fk_cashierReceipt_cashiersTill foreign key(cashierTillID)
	references ln.cashiersTill(cashiersTillID)
go

alter table ln.cashierDisbursement add constraint
	fk_cashierDisbursement_loan foreign key(loanID)
	references ln.loan(loanID)
go

alter table ln.cashierReceipt add constraint
	fk_cashierReceipt_loan foreign key(loanID)
	references ln.loan(loanID)
go

alter table ln.cashierDisbursement add constraint
	fk_cashierDisbursement_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.cashierReceipt add constraint
	fk_cashierReceipt_client foreign key(clientID)
	references ln.client(clientID)
go
 
alter table ln.cashierDisbursement add constraint
	fk_cashierDisbursement_modeOfPayment foreign key(paymentModeID)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.cashierReceipt add constraint
	fk_ccashierReceipt_modeOfPayment foreign key(paymentModeID)
	references ln.modeOfPayment(modeOfPaymentID)
go
