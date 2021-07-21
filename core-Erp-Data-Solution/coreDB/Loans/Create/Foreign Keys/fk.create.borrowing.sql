use coreDB
go


alter table ln.borrowing
	add constraint fk_borrowing_client foreign key(clientId)
	references ln.client(clientID)
go

alter table ln.borrowing
	add constraint fk_borrowing_borrowingType foreign key(borrowingTypeId)
	references ln.borrowingType(borrowingTypeId)
go

alter table ln.borrowing
	add constraint fk_borrowing_tenureType foreign key(tenureTypeId)
	references ln.tenureType(tenureTypeID)
go

alter table ln.borrowing
	add constraint fk_borrowing_loanStatus foreign key(borrowingStatusId)
	references ln.loanStatus(loanStatusID)
go

alter table ln.borrowing
	add constraint fk_borrowing_interestType foreign key(interestTypeId)
	references ln.interestType(interestTypeID)
go

alter table ln.borrowing
	add constraint fk_borrowing_repaymentMode foreign key(repaymentModeId)
	references ln.repaymentMode(repaymentModeID)
go

alter table ln.borrowingRepaymentSchedule
	add constraint fk_borrowingRepaymentSchedule_borrowing foreign key(borrowingId)
	references ln.borrowing(borrowingId)
go

alter table ln.borrowingFee
	add constraint fk_borrowingFee_borrowing foreign key(borrowingId)
	references ln.borrowing(borrowingId)
go

alter table ln.borrowingFee
	add constraint fk_borrowingFee_loanFeeType foreign key(feeTypeId)
	references ln.loanFeeType(feeTypeID)
go
 
alter table ln.borrowingPenalty
	add constraint fk_borrowingPenalty_borrowing foreign key(borrowingId)
	references ln.borrowing(borrowingId)
go
 
alter table ln.borrowingDocument
	add constraint fk_borrowingDocument_document foreign key(documentId)
	references ln.document(documentID)
go

alter table ln.borrowingDocument
	add constraint fk_borrowingDocument_borrowing foreign key(borrowingId)
	references ln.borrowing(borrowingId)
go

alter table ln.borrowingRepayment
	add constraint fk_borrowingRepayment_borrowing foreign key(borrowingId)
	references ln.borrowing(borrowingId)
go
 
alter table ln.borrowingRepayment
	add constraint fk_borrowingRepayment_modeOfPayment foreign key(modeOfPaymentId)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.borrowingRepayment
	add constraint fk_borrowingRepayment_repaymentType foreign key(repaymentTypeId)
	references ln.repaymentType(repaymentTypeID)
go

alter table ln.borrowingDisbursement
	add constraint fk_borrowingDisbursement_borrowing foreign key(borrowingId)
	references ln.borrowing(borrowingId)
go
 
alter table ln.borrowingDisbursement
	add constraint fk_borrowingDisbursement_modeOfPayment foreign key(modeOfPaymentId)
	references ln.modeOfPayment(modeOfPaymentID)
go

alter table ln.borrowingDisbursement
	add constraint fk_borrowingDisbursement_bank foreign key(bankId)
	references dbo.banks(bank_id)
go
