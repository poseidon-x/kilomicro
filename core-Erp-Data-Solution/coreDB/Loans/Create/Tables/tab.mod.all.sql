use coreDB
go
 
	alter table fa.depreciationSchedule add [version] rowversion
go
	alter table fa.assetDepreciation add [version] rowversion
go
	alter table fa.assetDocument add [version] rowversion
go
	alter table fa.assetImage add [version] rowversion
go
	alter table fa.asset add [version] rowversion 
go
	alter table ln.loanInsurance add [version] rowversion
go
	alter table ln.susuContribution add [version] rowversion
go
	alter table ln.susuContributionSchedule add [version] rowversion
go
	alter table ln.susuAccount add [version] rowversion
go
	alter table ln.regularSusuContribution add [version] rowversion
go
	alter table ln.regularSusuContributionSchedule add [version] rowversion
go
	alter table ln.regularSusuAccount add [version] rowversion
go
	alter table ln.invoiceLoanMaster add [version] rowversion
go
	alter table ln.invoiceLoanConfig add [version] rowversion
go
	alter table ln.multiPaymentClient add [version] rowversion
go
	alter table ln.multiPayment add [version] rowversion
go
	alter table ln.cashierReceipt add [version] rowversion
go
	alter table ln.cashierDisbursement add [version] rowversion
go
	alter table ln.cashiersTillDay add [version] rowversion
go
	alter table ln.cashiersTill add [version] rowversion
go
	alter table ln.loanRepayment add [version] rowversion
go
	alter table ln.loanPenalty add [version] rowversion
go
	alter table ln.repaymentSchedule add [version] rowversion
go
	alter table ln.collateralImage add [version] rowversion
go
	alter table ln.loanCollateral add [version] rowversion
go
	alter table ln.loanGurantor add [version] rowversion
go
	alter table ln.loanTranch add [version] rowversion
go
	alter table ln.loanFinancial  add [version] rowversion
go
	alter table ln.loanDocument add [version] rowversion
go
	alter table ln.loanCheck add [version] rowversion
go
	alter table ln.loanCheckList add [version] rowversion
go
	alter table ln.invoiceLoan add [version] rowversion
go
	alter table ln.loanFee add [version] rowversion
go
	alter table ln.loanIterestWriteOff add [version] rowversion
go
	alter table ln.loanProvision add [version] rowversion
go
	alter table ln.prAllowance add [version] rowversion
go
	alter table ln.prLoanDetail add [version] rowversion
go
	alter table ln.loanIncentive add [version] rowversion
go
	alter table ln.clientActivityLog add [version] rowversion
go
	alter table ln.loan add [version] rowversion
go
	alter table ln.depositWithdrawal add [version] rowversion
go
	alter table ln.depositInterest add [version] rowversion
go
	alter table ln.depositAdditional add [version] rowversion
go
	alter table ln.depositSchedule add [version] rowversion
go
	alter table ln.depositSignatory add [version] rowversion
go
	alter table ln.deposit add [version] rowversion
go
	alter table ln.savingWithdrawal add [version] rowversion
go
	alter table ln.savingInterest add [version] rowversion
go
	alter table ln.savingAdditional add [version] rowversion
go
	alter table ln.savingSchedule add [version] rowversion
go
	alter table ln.savingSignatory add [version] rowversion
go
go
	alter table ln.saving add [version] rowversion 
go
	alter table hc.staffPromotion add [version] rowversion
go
	alter table hc.overtime add [version] rowversion
go
	alter table hc.staffDaysWorked add [version] rowversion
go
	alter table fa.staffAddress add [version] rowversion
go
	alter table fa.staffEmail add [version] rowversion
go
	alter table fa.staffImage add [version] rowversion
go
	alter table fa.staffPhone add [version] rowversion
go
	alter table fa.staffDocument add [version] rowversion
go
	alter table hc.staffLeave add [version] rowversion
go
	alter table hc.staffAllowance add [version] rowversion
go
	alter table hc.staffAttendance add [version] rowversion
go
	alter table hc.staffBenefit add [version] rowversion
go
	alter table hc.staffBenefitsInKind add [version] rowversion
go
	alter table hc.staffCalendar add [version] rowversion
go
	alter table hc.staffDeduction add [version] rowversion
go
	alter table hc.staffLoanRepayment add [version] rowversion
go
	alter table hc.staffLoanSchedule add [version] rowversion
go
	alter table hc.staffLoan add [version] rowversion
go
	alter table hc.staffLeaveBalance add [version] rowversion
go
	alter table hc.staffManager add [version] rowversion
go
	alter table hc.staffOneTimeDeduction add [version] rowversion
go
	alter table hc.staffPension add [version] rowversion
go
	alter table hc.staffQualification add [version] rowversion
go
	alter table hc.staffRelation add [version] rowversion
go
	alter table hc.payMasterAllowance add [version] rowversion
go
	alter table hc.payMasterBenefitsInKind add [version] rowversion
go
	alter table hc.payMasterDeduction add [version] rowversion
go
	alter table hc.payMasterOvertime add [version] rowversion
go
	alter table hc.payMasterLoan add [version] rowversion
go
	alter table hc.payMasterOneTimeDeduction add [version] rowversion
go
	alter table hc.payMasterPension add [version] rowversion
go
	alter table hc.payMasterTax add [version] rowversion
go
	alter table hc.payMasterTaxRelief add [version] rowversion
go
	alter table hc.staffLoan add [version] rowversion
go
	alter table hc.payMaster add [version] rowversion
go
	alter table fa.staff add [version] rowversion 
go
	alter table ln.depositSignatory add [version] rowversion
go
	alter table ln.savingSignatory add [version] rowversion
go
	alter table ln.clientCompany add [version] rowversion
go
	alter table ln.staffCategory add [version] rowversion
go
	alter table ln.smeCategory add [version] rowversion
go
	alter table ln.agentPhone add [version] rowversion
go
	alter table ln.agentAddress add [version] rowversion
go
	alter table ln.agentDocument add [version] rowversion
go
	alter table ln.agentImage add [version] rowversion
go
	alter table ln.agentNextOfKin add [version] rowversion
go
	alter table ln.nextOfKin add [version] rowversion
go
	alter table ln.clientBankAccount add [version] rowversion
go
	alter table ln.addressImage add [version] rowversion
go
	alter table ln.clientAddress add [version] rowversion
go
	alter table ln.clientEmail add [version] rowversion
go
	alter table ln.clientImage add [version] rowversion
go
	alter table ln.clientPhone add [version] rowversion
go
	alter table ln.clientLiability add [version] rowversion
go
	alter table ln.clientBusinessActivity add [version] rowversion
go
	alter table ln.employeeCategory add [version] rowversion
go
	alter table ln.groupExec add [version] rowversion
go
	alter table ln.groupCategory add [version] rowversion
go
	alter table ln.employerDirector add [version] rowversion
go
	alter table ln.employerDepartment add [version] rowversion
go
	alter table ln.employer add [version] rowversion
go
	alter table ln.[group] add [version] rowversion
go
	alter table ln.phone add [version] rowversion
go
	alter table ln.microBusinessCategory add [version] rowversion
go
	alter table ln.smeDirector add [version] rowversion
go
	alter table ln.smeCategory add [version] rowversion
go
	alter table ln.[image] add [version] rowversion
go
	alter table ln.email add [version] rowversion
go
	alter table ln.[address] add [version] rowversion
go
	alter table ln.clientDocument add [version] rowversion
go
	alter table ln.deposit add [version] rowversion
go
	alter table ln.[document] add [version] rowversion
go
	alter table ln.client add [version] rowversion 
go
	alter table jnl_stg add [version] rowversion
go
	alter table jnl_batch_stg add [version] rowversion
go
	alter table jnl_tmp add [version] rowversion
go
	alter table jnl_batch_tmp add [version] rowversion
go
	alter table jnl add [version] rowversion
go
	alter table jnl_batch add [version] rowversion
go
	alter table gl.pc_dtl add [version] rowversion
go
	alter table gl.pc_head add [version] rowversion
go
	alter table gl.v_ftr add [version] rowversion
go
	alter table gl.v_dtl add [version] rowversion
go
	alter table gl.v_head add [version] rowversion
go
	alter table gl.budget add [version] rowversion 
go
	alter table def_accts add [version] rowversion
go
	alter table std_accts add [version] rowversion
go
	alter table t_acc_bals add [version] rowversion
go
	alter table acct_bals add [version] rowversion
go
	alter table accts add [version] rowversion
go
	alter table acct_heads add [version] rowversion 
go