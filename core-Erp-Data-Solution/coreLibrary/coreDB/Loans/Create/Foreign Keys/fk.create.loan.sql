use coreDB
go


alter table ln.loan
	add constraint fk_loan_client foreign key(clientID)
	references ln.client(clientID)
go

alter table ln.loan
	add constraint fk_loan_staff foreign key(staffID)
	references fa.staff(staffID)
go

alter table ln.loan
	add constraint fk_loan_agent foreign key(agentID)
	references ln.agent(agentID)
go

alter table ln.loan
	add constraint fk_loan_loanStatus foreign key(loanStatusID)
	references ln.loanStatus(loanStatusID)
go

alter table ln.loan
	add constraint fk_loan_interestType foreign key(interestTypeID)
	references ln.interestType(interestTypeID)
go

alter table ln.loan
	add constraint fk_loan_repaymentMode foreign key(repaymentModeID)
	references ln.repaymentMode(repaymentModeID)
go

alter table ln.loan
	add constraint fk_loan_loanType foreign key(loanTypeID)
	references ln.loanType(loanTypeID)
go
 
alter table ln.loanGurantor
	add constraint fk_loanGurantor_loan foreign key(loanID)
	references ln.loan(loanID)
go
 
alter table ln.loanGurantor
	add constraint fk_loanGurantor_address foreign key(addressID)
	references ln.[address](addressID)
go
 
alter table ln.loanGurantor
	add constraint fk_loanGurantor_phone foreign key(phoneID)
	references ln.phone(phoneID)
go
 
alter table ln.loanGurantor
	add constraint fk_loanGurantor_email foreign key(emailID)
	references ln.email(emailID)
go
 
alter table ln.loanGurantor
	add constraint fk_loanGurantor_image foreign key(imageID)
	references ln.[image](imageID)
go
 
alter table ln.loanCollateral
	add constraint fk_loanCollateral_loan foreign key(loanID)
	references ln.loan(loanID)
go
 
alter table ln.loanCollateral
	add constraint fk_loanCollateral_collateralType foreign key(collateralTypeID)
	references ln.collateralType(collateralTypeID)
go
  
alter table ln.collateralImage
	add constraint fk_collateralImage_loanCollateral foreign key(loanCollateralID)
	references ln.loanCollateral(loanCollateralID)
go
 
alter table ln.collateralImage
	add constraint fk_collateralImage_image foreign key(imageID)
	references ln.[image](imageID)
go

alter table ln.loanTranch
	add constraint fk_loanTranch_loan foreign key(loanID)
	references ln.loan(loanID)
go
     
alter table ln.loanTranch
	add constraint fk_loanTranch_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go
  
alter table ln.repaymentSchedule
	add constraint fk_repaymentSchedule_loan foreign key(loanID)
	references ln.loan(loanID)
go
   
alter table ln.loanPenalty
	add constraint fk_loanPenalty_loan foreign key(loanID)
	references ln.loan(loanID)
go
   
alter table ln.loanRepayment
	add constraint fk_loanRepayment_loan foreign key(loanID)
	references ln.loan(loanID)
go
  
alter table ln.loanRepayment
	add constraint fk_loanRepayment_modeOfPayment foreign key(modeOfPaymentID)
	references ln.modeOfPayment(modeOfPaymentID)
go
   
alter table ln.loanRepayment
	add constraint fk_loanRepayment_repaymentType foreign key(repaymentTypeID)
	references ln.repaymentType(repaymentTypeID)
go
   
alter table ln.loan 
	add constraint fk_loan_tenureType foreign key(tenureTypeID)
	references ln.tenureType(tenureTypeID)
go
   
alter table ln.loanGurantor
	add constraint fk_loanGurantor_idNo foreign key(idNoID)
	references ln.idNo(idNoID)
go
   
alter table ln.loanCheckList
	add constraint fk_loanCheckList_loan foreign key(loanID)
	references ln.loan(loanID)
go
   
alter table ln.loanCheckList
	add constraint fk_loanCheckList_categoryCheckList foreign key(categoryCheckListID)
	references ln.categoryCheckList(categoryCheckListID)
go
 
alter table ln.categoryCheckList
	add constraint fk_categoryCheckList_category foreign key(categoryID)
	references ln.category(categoryID)
go
 
 alter table ln.loanFinancial
	add constraint fk_loanFinancial_loan foreign key(loanID)
	references ln.loan(loanID)
go

 alter table ln.loanFinancial
	add constraint fk_loanFinancial_financialType foreign key(financialTypeID)
	references ln.financialType(financialTypeID)
go

 alter table ln.loanCheck
	add constraint fk_loanCheck_loan foreign key(loanID)
	references ln.loan(loanID)
go

 alter table ln.loanFinancial
	add constraint fk_loanFinancial_frequency foreign key(frequencyID)
	references ln.repaymentMode(repaymentModeID)
go

alter table ln.loanFee
	add constraint fk_loanFee_loan foreign key (loanID)
	references ln.loan (loanID)
go

alter table ln.loanIterestWriteOff
	add constraint fk_loaInterestWriteOff_loan foreign key (loanID)
	references ln.loan (loanID)
go

alter table ln.loanFee
	add constraint fk_loanFee_loanFeeType foreign key (feeTypeID)
	references ln.loanFeeType (feeTypeID)
go

alter table ln.loanProductHistory add
	constraint fk_loanProductHistory_loanProduct foreign key (loanProductID)
	references ln.loanProduct(loanProductID)
go

alter table ln.prLoanDetail
	add constraint fk_prLoanDetail_loan foreign key(loanID)
	references ln.loan(loanID)
go

alter table ln.prLoanDetail
	add constraint fk_prLoanDetail_loanProduct foreign key(loanProductID)
	references ln.loanProduct(loanProductID)
go

alter table ln.prAllowance
	add constraint fk_staffCategoryAllowance_loanDetail foreign key(loanDetailID)
	references ln.prLoanDetail(loanDetailID)
go

alter table ln.prAllowance
	add constraint fk_staffCategoryAllowance_prAllowanceType foreign key(allowanceTypeID)
	references ln.prAllowanceType(allowanceTypeID)
go

alter table ln.loanPurposeDetail
	add constraint fk_loanPurposeDetail_loanPurpose foreign key(loanPurposeID)
	references ln.loanPurpose(loanPurposeID)
go

alter table ln.prLoanDetail
	add constraint fk_prLoanDetail_loanPurpose foreign key(loanPurposeID)
	references ln.loanPurpose(loanPurposeID)
go

alter table ln.prLoanDetail
	add constraint fk_prLoanDetail_loanPurposeDetail foreign key(loanPurposeDetailID)
	references ln.loanPurposeDetail(loanPurposeDetailID)
go

alter table ln.prLoanDetail
	add constraint fk_prLoanDetail_modeOfEntry foreign key(modeOfEntryID)
	references ln.modeOfEntry(modeOfEntryID)
go

alter table ln.loanIncentive
	add constraint fk_loanIncentive_loan foreign key(loanID)
	references ln.loan(loanID)
go

alter table ln.loanIncentive
	add constraint fk_loanIncentive_agent foreign key(agentID)
	references ln.agent(agentID)
go

alter table ln.loanCheck add
	constraint fk_loanCheck_client foreign key (clientID)
	references ln.client (clientID)
go

 alter table ln.loanCheck
	drop constraint fk_loanCheck_loan 
go

alter table ln.insuranceSetup add
	constraint fk_insuranceSetup_loanType foreign key (loanTypeID)
	references ln.loanType (loanTypeID)
go

alter table ln.loanInsurance add
	constraint fk_loanInsurance_loan foreign key (loanID)
	references ln.loan (loanID)
go

alter table ln.loanCheck add
	constraint fk_loanCheck_checkType foreign key (checkTypeID)
	references ln.checkType (checkTypeID)
go

alter table ln.loanPenalty add
	constraint fk_loanPenalty_penaltyType foreign key (penaltyTypeId)
	references ln.penaltyType (penaltyTypeId)
go

alter table ln.loanClosure add
	constraint fk_loanClosure_loan foreign key (loanId)
	references ln.loan (loanId)
go

alter table ln.loanClosure add
	constraint fk_loanClosure_loanClosureReason foreign key (loanClosureReasonId)
	references ln.loanClosureReason (loanClosureReasonId)
go


alter table ln.loanGroupClient drop
	constraint fk_loanGroupClient_loanGroup

alter table ln.loanGroupClient drop
	constraint fk_loanGroupClient_client
    
alter table ln.loanGroup drop	
	constraint fk_loanGroup_loanGroupDay

alter table ln.loanGroup drop
	constraint fk_loanGroup_staff

alter table ln.loanGroup drop
	constraint fk_loanGroup_groupClient


alter table ln.loanGroupClient add
	constraint fk_loanGroupClient_loanGroup foreign key(loanGroupId)
	references ln.loanGroup(loanGroupId)

alter table ln.loanGroupClient add
	constraint fk_loanGroupClient_client foreign key(clientId)
	references ln.client(clientID)
    
alter table ln.loanGroup add	
	constraint fk_loanGroup_loanGroupDay foreign key(loanGroupDayId)
	references ln.loanGroupDay(loanGroupDayId)

alter table ln.loanGroup add
	constraint fk_loanGroup_staff foreign key(relationsOfficerStaffId)
	references fa.staff(staffID)

alter table ln.loanGroup add
	constraint fk_loanGroup_client foreign key(leaderClientId)
	references ln.client(clientID)



-------- Manager 28-OCT-2015 -----------
	alter table ln.loanApproval add
	constraint fk_loanApproval_loan foreign key(loanId)
	references ln.loan(loanID)

alter table ln.loanApproval add
	constraint fk_loanApproval_loanApprovalStage foreign key(approvalStageId)
	references ln.loanApprovalStage(loanApprovalStageId)

alter table ln.loanApprovalStage add
	constraint fk_loanApprovalStage_loanType foreign key(loanTypeId)
	references ln.loanType(loanTypeID)

alter table ln.loanApprovalStageOfficer add
	constraint fk_loanApprovalStageOfficer_loanApprovalStage foreign key(loanApprovalStageId)
	references ln.loanApprovalStage(loanApprovalStageId)


alter table ln.loan add
	securityDeposit float not null default(0)





