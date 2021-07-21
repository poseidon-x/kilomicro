use coreDB
go

set xact_abort on

begin tran

declare cur cursor for
select loanID, disbursementDate, processingFee 
from ln.loan
where loanNo in ('1011010148/2','001013010020/1','1011010050/3','1011010200/2','1011010143/2',
	'001011010008/1','001011010138/1','1011010244/1','1011010033/4','1011010095/3','001011010012/3','1011010227/1')
declare @loanID int, @disbursementDate datetime, @processingFee float

open cur
fetch next from cur into @loanID, @disbursementDate, @processingFee

while @@FETCH_STATUS=0
begin
	declare curReceipts cursor for
	select loanRepaymentID,
		amountPaid,
		repaymentDate
	from ln.loanRepayment
	where loanID = @loanID and repaymentTypeID = 1
	order by repaymentDate asc, loanRepaymentID asc

	declare @loanRepaymentID int, @amountPaid float, @repaymentDate datetime
	open curReceipts
	fetch curReceipts into @loanRepaymentID, @amountPaid, @repaymentDate
	while @@FETCH_STATUS=0
	begin
		declare @int float, @princ float, @balAsAt float, @intPaid float,
			@intAsAt float, @intExpected float

		select @balAsAt=isnull(isnull(max(amountDisbursed),0)-isnull(sum(principalPaid),0),0),
			@intPaid = isnull(sum(interestPaid), 0)
		from ln.vwLoanActualSchedule
		where loanID = @loanID and [date]<@repaymentDate
		
		select @intExpected = isnull(sum(interest), 0)
		from ln.vwLoanActualSchedule
		where loanID = @loanID

		select @intAsAt = @intExpected - @intPaid
		select @int = (@amountPaid * @intAsAt)/(@intAsAt+@balAsAt),
			@princ = (@amountPaid * @balAsAt)/(@intAsAt+@balAsAt)
		
		update ln.loanRepayment
		set principalPaid=@princ,
			interestPaid = @int
		where loanRepaymentID = @loanRepaymentID
		 
		fetch curReceipts into @loanRepaymentID, @amountPaid, @repaymentDate
	end
	close curReceipts
	deallocate curReceipts
	
	if( not Exists(select loanRepaymentID from ln.loanRepayment
					where loanID=@loanID and repaymentTypeID=6))
	begin
		INSERT INTO [ln].[loanRepayment]
			([loanID]
			,[modeOfPaymentID]
			,[repaymentTypeID]
			,[repaymentDate]
			,[amountPaid]
			,[interestPaid]
			,[principalPaid]
			,[feePaid]
			,[penaltyPaid]
			,[creation_date]
			,[creator]
			,[modification_date]
			,[last_modifier]
			,[commission_paid]
			,[bankID]
			,[bankName]
			,[checkNo])
		VALUES
			(@loanID
			,1
			,6
			,@disbursementDate
			,@processingFee
			,0
			,0
			,@processingFee
			,0
			,getDate()
			,'SYSTEM'
			,getDate()
			,'SYSTEM'
			,0
			,NULL
			,NULL
			,NULL)
	end
	fetch next from cur into @loanID, @disbursementDate, @processingFee
end

close cur
deallocate cur

commit tran
