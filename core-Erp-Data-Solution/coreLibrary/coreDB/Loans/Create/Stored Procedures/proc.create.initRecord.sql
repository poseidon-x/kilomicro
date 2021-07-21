use coreDB
go


create procedure initRecord
(
	@tbl nvarchar (1000),
	@pkColumn nvarchar(50),
	@pkId bigint
)
with encryption
as
begin
	declare @stmt nvarchar(4000) 

	select @stmt = 'delete from ' + @tbl + 
		' where ' + @pkColumn + ' = ' + cast(@pkId as nvarchar(20)) + ' ';
	 
	exec (@stmt) 

end
go

alter procedure initLoanRecord
(
	@loanNo nvarchar(20)
)
with encryption 
as
begin
	set xact_abort on

	begin transaction

	declare @loanId bigint
	select @loanId = loanId 
	from ln.loan
	where loanNo=@loanNo

	declare curSched cursor
	for select repaymentScheduleId
	from ln.repaymentSchedule
	where loanId = @loanId
	declare @schId bigint

	open curSched
	fetch next from curSched into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.repaymentSchedule','repaymentScheduleId', @schId
		fetch next from curSched into @schId
	end
	close curSched
	deallocate curSched
	
	declare curRep cursor
	for select loanRepaymentId
	from ln.loanRepayment
	where loanId = @loanId 

	open curRep
	fetch next from curRep into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanRepayment','loanRepaymentId', @schId
		fetch next from curRep into @schId
	end
	close curRep
	deallocate curRep
	
	declare curTrc cursor
	for select loanTranchId
	from ln.loanTranch
	where loanId = @loanId 

	open curTrc
	fetch next from curTrc into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanTranch','loanTranchId', @schId
		fetch next from curTrc into @schId
	end
	close curTrc
	deallocate curTrc
	
	declare curPen cursor
	for select loanPenaltyId
	from ln.loanPenalty
	where loanId = @loanId 

	open curPen
	fetch next from curPen into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanPenalty','loanPenaltyId', @schId
		fetch next from curPen into @schId
	end
	close curPen
	deallocate curPen
	
	declare curRec cursor
	for select cashierReceiptId
	from ln.cashierReceipt
	where loanId = @loanId 

	open curRec
	fetch next from curRec into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.cashierReceipt','cashierReceiptId', @schId
		fetch next from curRec into @schId
	end
	close curRec
	deallocate curRec
	
	declare curDisb cursor
	for select cashierDisbursementId
	from ln.cashierDisbursement
	where loanId = @loanId 

	open curDisb
	fetch next from curDisb into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.cashierDisbursement','cashierDisbursementId', @schId
		fetch next from curDisb into @schId
	end
	close curDisb
	deallocate curDisb
	
	declare curFee cursor
	for select loanFeeId
	from ln.loanFee
	where loanId = @loanId 

	open curFee
	fetch next from curFee into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanFee','loanFeeId', @schId
		fetch next from curFee into @schId
	end
	close curFee
	deallocate curFee
	
	declare curCL cursor
	for select loanChecklistId
	from ln.loanChecklist
	where loanId = @loanId 

	open curCL
	fetch next from curCL into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanChecklist','loanChecklistId', @schId
		fetch next from curCL into @schId
	end
	close curCL
	deallocate curCL
	
	declare curProv cursor
	for select loanProvisionId
	from ln.loanProvision
	where loanId = @loanId 

	open curProv
	fetch next from curProv into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanProvision','loanProvisionId', @schId
		fetch next from curProv into @schId
	end
	close curProv
	deallocate curProv
	
	declare curGua cursor
	for select loanGurantorId
	from ln.loanGurantor
	where loanId = @loanId 

	open curGua
	fetch next from curGua into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanGurantor','loanGurantorId', @schId
		fetch next from curGua into @schId
	end
	close curGua
	deallocate curGua
	
	declare curDoc cursor
	for select loanDocumentId
	from ln.loanDocument
	where loanId = @loanId 

	open curDoc
	fetch next from curGua into @schId
	while @@FETCH_STATUS=0
	begin
		exec initRecord 'ln.loanDocument','loanDocumentId', @schId
		fetch next from curDoc into @schId
	end
	close curDoc
	deallocate curDoc
	
	declare curCol cursor
	for select loanCollateralId
	from ln.loanCollateral
	where loanId = @loanId 
	 
	open curCol
	fetch next from curCol into @schId
	while @@FETCH_STATUS=0
	begin
		
		declare curColImg cursor
		for select collateralImageId
		from ln.collateralImage
		where loanCollateralId = @schId 
		declare @ci int
		open curColImg
		fetch next from curCol into @ci
		while @@FETCH_STATUS=0
		begin	
			exec initRecord 'ln.collateralImage','collateralImageId', @ci
			fetch next from curColImg into @ci
		end
		close curColImg
		deallocate curColImg
	

		exec initRecord 'ln.loanCollateral','loanCollateralId', @schId
		fetch next from curCol into @schId
	end
	close curCol
	deallocate curCol

	exec initRecord 'ln.loan','loanId', @loanId

	commit transaction
end
go
