use coreDB
go

create procedure initTable
(
	@tbl nvarchar (1000)
)
with encryption
as
begin
	declare @stmt nvarchar(4000)
	declare @stmt2 nvarchar(4000)

	select @stmt = 'delete from ' + @tbl
	select @stmt2 = 'dbcc checkident (''' + @tbl +''', RESEED, 0) '

	exec (@stmt)
	exec (@stmt2)

end
go

alter procedure initAsset
with encryption as
begin
	exec dbo.initTable 'fa.depreciationSchedule'
	exec dbo.initTable 'fa.assetDepreciation'
	exec dbo.initTable 'fa.assetDocument'
	exec dbo.initTable 'fa.assetImage'
	exec dbo.initTable 'fa.asset'
end
go

alter procedure initLoan
with encryption
as
begin
	exec dbo.initTable 'ln.savingDailyInterest'
	exec dbo.initTable 'ln.loanInsurance'
	exec dbo.initTable 'ln.susuContribution'
	exec dbo.initTable 'ln.susuContributionSchedule'
	exec dbo.initTable 'ln.susuAccount'
	exec dbo.initTable 'ln.regularSusuContribution'
	exec dbo.initTable 'ln.regularSusuContributionSchedule'
	exec dbo.initTable 'ln.regularSusuAccount'
	exec dbo.initTable 'ln.invoiceLoanMaster'
	exec dbo.initTable 'ln.invoiceLoanConfig'
	exec dbo.initTable 'ln.multiPaymentClient'
	exec dbo.initTable 'ln.multiPayment'
	exec dbo.initTable 'ln.cashierReceipt'
	exec dbo.initTable 'ln.cashierDisbursement'
	exec dbo.initTable 'ln.cashiersTillDay'
	exec dbo.initTable 'ln.cashiersTill'
	exec dbo.initTable 'ln.loanRepayment'
	exec dbo.initTable 'ln.loanPenalty'
	exec dbo.initTable 'ln.repaymentSchedule'
	exec dbo.initTable 'ln.collateralImage'
	exec dbo.initTable 'ln.loanCollateral'
	exec dbo.initTable 'ln.loanGurantor'
	exec dbo.initTable 'ln.loanTranch'
	exec dbo.initTable 'ln.loanFinancial '
	exec dbo.initTable 'ln.loanDocument'
	exec dbo.initTable 'ln.loanCheck'
	exec dbo.initTable 'ln.loanCheckList'
	exec dbo.initTable 'ln.invoiceLoan'
	exec dbo.initTable 'ln.loanFee'
	exec dbo.initTable 'ln.loanIterestWriteOff'
	exec dbo.initTable 'ln.loanProvision'
	exec dbo.initTable 'ln.prAllowance'
	exec dbo.initTable 'ln.prLoanDetail'
	exec dbo.initTable 'ln.loanIncentive'
	exec dbo.initTable 'ln.clientActivityLog'
	exec dbo.initTable 'ln.loan'
	exec dbo.initTable 'ln.depositWithdrawal'
	exec dbo.initTable 'ln.depositInterest'
	exec dbo.initTable 'ln.depositAdditional'
	exec dbo.initTable 'ln.depositSchedule'
	exec dbo.initTable 'ln.depositSignatory'
	exec dbo.initTable 'ln.deposit'
	exec dbo.initTable 'ln.savingWithdrawal'
	exec dbo.initTable 'ln.savingInterest'
	exec dbo.initTable 'ln.savingAdditional'
	exec dbo.initTable 'ln.savingSchedule'
	exec dbo.initTable 'ln.savingSignatory'
	exec dbo.initTable 'ln.saving'
end
go

alter procedure initStaff
with encryption
as
begin  
	exec dbo.initTable 'hc.staffPromotion'
	exec dbo.initTable 'hc.overtime'
	exec dbo.initTable 'hc.staffDaysWorked'
	exec dbo.initTable 'fa.staffAddress'
	exec dbo.initTable 'fa.staffEmail'
	exec dbo.initTable 'fa.staffImage'
	exec dbo.initTable 'fa.staffPhone'
	exec dbo.initTable 'fa.staffDocument'
	exec dbo.initTable 'hc.staffLeave'
	exec dbo.initTable 'hc.staffAllowance'
	exec dbo.initTable 'hc.staffAttendance'
	exec dbo.initTable 'hc.staffBenefit'
	exec dbo.initTable 'hc.staffBenefitsInKind'
	exec dbo.initTable 'hc.staffCalendar'
	exec dbo.initTable 'hc.staffDeduction'
	exec dbo.initTable 'hc.staffLoanRepayment'
	exec dbo.initTable 'hc.staffLoanSchedule'
	exec dbo.initTable 'hc.staffLoan'
	exec dbo.initTable 'hc.staffLeaveBalance'
	exec dbo.initTable 'hc.staffManager'
	exec dbo.initTable 'hc.staffOneTimeDeduction'
	exec dbo.initTable 'hc.staffPension'
	exec dbo.initTable 'hc.staffQualification'
	exec dbo.initTable 'hc.staffRelation'
	exec dbo.initTable 'hc.payMasterAllowance'
	exec dbo.initTable 'hc.payMasterBenefitsInKind'
	exec dbo.initTable 'hc.payMasterDeduction'
	exec dbo.initTable 'hc.payMasterOvertime'
	exec dbo.initTable 'hc.payMasterLoan'
	exec dbo.initTable 'hc.payMasterOneTimeDeduction'
	exec dbo.initTable 'hc.payMasterPension'
	exec dbo.initTable 'hc.payMasterTax'
	exec dbo.initTable 'hc.payMasterTaxRelief'
	exec dbo.initTable 'hc.staffLoan'
	exec dbo.initTable 'hc.payMaster'
	exec dbo.initTable 'fa.staff'
end
go

alter procedure initClient
with encryption
as
begin
	exec dbo.initTable 'ln.depositSignatory'
	exec dbo.initTable 'ln.savingSignatory'
	exec dbo.initTable 'ln.Clientservicecharge'
	--exec dbo.initTable 'ln.clientCompany'
	exec dbo.initTable 'ln.staffCategory'
	exec dbo.initTable 'ln.smeCategory'
	exec dbo.initTable 'ln.agentPhone'
	exec dbo.initTable 'ln.agentAddress'
	exec dbo.initTable 'ln.agentDocument'
	exec dbo.initTable 'ln.agentImage'
	exec dbo.initTable 'ln.agentNextOfKin'
	exec dbo.initTable 'ln.nextOfKin'
	exec dbo.initTable 'ln.clientBankAccount'
	exec dbo.initTable 'ln.addressImage'
	exec dbo.initTable 'ln.clientAddress'
	exec dbo.initTable 'ln.clientEmail'
	exec dbo.initTable 'ln.clientImage'
	exec dbo.initTable 'ln.clientPhone'
	exec dbo.initTable 'ln.clientLiability'
	exec dbo.initTable 'ln.clientBusinessActivity'
	exec dbo.initTable 'ln.employeeCategory'
	exec dbo.initTable 'ln.groupExec'
	exec dbo.initTable 'ln.groupCategory'
	exec dbo.initTable 'ln.employerDirector'
	exec dbo.initTable 'ln.employerDepartment'
	exec dbo.initTable 'ln.employer'
	exec dbo.initTable 'ln.[group]'
	exec dbo.initTable 'ln.phone'
	exec dbo.initTable 'ln.microBusinessCategory'
	exec dbo.initTable 'ln.smeDirector'
	exec dbo.initTable 'ln.smeCategory'
	exec dbo.initTable 'ln.[image]'
	exec dbo.initTable 'ln.email'
	exec dbo.initTable 'ln.[address]'
	exec dbo.initTable 'ln.clientDocument'
	exec dbo.initTable 'ln.deposit'
	exec dbo.initTable 'ln.[document]'
	exec dbo.initTable 'ln.client'
	exec dbo.initTable 'ln.loanGroup'
	exec dbo.initTable 'ln.loanGroupClient'
end
go


alter proc initJournal
with encryption
as
begin
	exec dbo.initTable 'jnl_stg'
	exec dbo.initTable 'jnl_batch_stg'
	exec dbo.initTable 'jnl_tmp'
	exec dbo.initTable 'jnl_batch_tmp'
	exec dbo.initTable 'jnl'
	exec dbo.initTable 'jnl_batch'
	exec dbo.initTable 'gl.pc_dtl'
	exec dbo.initTable 'gl.pc_head'
	exec dbo.initTable 'gl.v_ftr'
	exec dbo.initTable 'gl.v_dtl'
	exec dbo.initTable 'gl.v_head'
	exec dbo.initTable 'gl.budget'
	update dbo.sys_no set [value]=10000000 where [name]='FND_TX'
end
go

alter proc initAccounts
with encryption
as
begin 
	exec dbo.initTable 'def_accts'
	exec dbo.initTable 'std_accts'
	exec dbo.initTable 't_acc_bals'
	exec dbo.initTable 'acct_bals'
	exec dbo.initTable 'accts'
	exec dbo.initTable 'acct_heads'
end
go
