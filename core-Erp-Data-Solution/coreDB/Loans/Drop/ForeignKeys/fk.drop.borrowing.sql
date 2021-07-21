use coreDB
go

alter table ln.borrowing
	drop constraint fk_borrowing_client

alter table ln.borrowing
	drop constraint fk_borrowing_borrowingType 

alter table ln.borrowing
	drop constraint fk_borrowing_tenureType

alter table ln.borrowing
	drop constraint fk_borrowing_loanStatus

alter table ln.borrowing
	drop constraint fk_borrowing_interestType

alter table ln.borrowing
	drop constraint fk_borrowing_repaymentMode

alter table ln.borrowingRepaymentSchedule
	drop constraint fk_borrowingRepaymentSchedule_borrowing

alter table ln.borrowingFee
	drop constraint fk_borrowingFee_borrowing

alter table ln.borrowingFee
	drop constraint fk_borrowingFee_loanFeeType

alter table ln.borrowingPenalty
	drop constraint fk_borrowingPenalty_borrowing
 
alter table ln.borrowingDocument
	drop constraint fk_borrowingDocument_document

alter table ln.borrowingDocument
	drop constraint fk_borrowingDocument_borrowing

alter table ln.borrowingRepayment
	drop constraint fk_borrowingRepayment_borrowing

alter table ln.borrowingRepayment
	drop constraint fk_borrowingRepayment_repaymentType

alter table ln.borrowingDisbursement
	drop constraint fk_borrowingDisbursement_borrowing
 
alter table ln.borrowingDisbursement
	drop constraint fk_borrowingDisbursement_modeOfPayment

alter table ln.borrowingDisbursement
	drop constraint fk_borrowingDisbursement_bank

alter table ln.borrowingRepayment
	drop constraint fk_borrowingRepayment_modeOfPayment

	