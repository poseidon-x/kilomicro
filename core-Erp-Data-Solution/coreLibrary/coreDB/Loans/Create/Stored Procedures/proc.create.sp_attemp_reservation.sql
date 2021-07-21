USE [coreDB]
GO
/****** Object:  StoredProcedure [ln].[sp_attempt_reservation]    Script Date: 8/9/2017 8:26:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




ALTER proc [ln].[sp_attempt_reservation]
	@savingId int, 
	@amount float,
	@reservationTypeId int,
	@reservedBy nvarchar(30),
	@naration nvarchar(255),
	@transactionId nvarchar(60) out
as
	declare @currentBalance float
	declare @reservedAmount float
	declare @isAccountLocked bit = 0
	declare @lockedDateTime datetime = getDate()

begin try
	set transaction isolation level repeatable read
	begin transaction

		
		select @isAccountLocked = accountLocked from ln.saving where savingID = @savingId
		--Lock Account and reserve funds if not locked
		if(@isAccountLocked = 0)
			begin

			--Lock Account
			update ln.saving set accountLocked = 1,lockedBy=@reservedBy,lockDate=@lockedDateTime where savingID = @savingId
				and accountLocked=0
			
			--Check if lock succeed
			declare @lockSucceeded bit = 0
			select @lockSucceeded = accountLocked from ln.saving where savingID = @savingId and lockedBy = @reservedBy
			and DATEDIFF(MINUTE, lockDate, @lockedDateTime)<1

			--Proceed to reserve funds if lock succeed
			if(@lockSucceeded = 1)
			begin
				select @currentBalance = sum(interestBalance+principalBalance) from ln.saving where savingID = @savingId
				if(@reservationTypeId = 1) 
				select @reservedAmount = withdrawalReservation from ln.saving where savingID = @savingId
				select @transactionId = NEWID()
				print @transactionId

				--Check if client has enough balance
				if((@reservationTypeId = 1 and @reservedAmount+@amount <= @currentBalance) or @reservationTypeId = 2)
				begin
					select 'Enough balance'
					
					--For savings withdrawal
					if(@reservationTypeId = 1)
					begin
						--Reserve Withdrawal fund
						update ln.saving set withdrawalReservation += @amount where savingID = @savingId and accountLocked = 1 
							and lockedBy = @reservedBy and lockDate = @lockedDateTime
					end
					--For savings deposit
					else if(@reservationTypeId = 2)
					begin
						--Reserve Deposit fund
						update ln.saving set depositReservation += @amount where savingID = @savingId and accountLocked = 1 
							and lockedBy = @reservedBy and lockDate = @lockedDateTime
					end

					insert into ln.savingReservationTransc(savingId,reservationAmount,reservationTypeId,reservationDate,reservedBy,naration,transactionId,[committed])
					values(@savingId,@amount,@reservationTypeId,@lockedDateTime,@reservedBy,@naration,@transactionId,0)

					--Release lock
					update ln.saving set accountLocked = 0,lockedBy=null,lockDate=null where savingID = @savingId and accountLocked = 1 
						and lockedBy = @reservedBy and DATEDIFF(MINUTE, lockDate, @lockedDateTime)<1
				end
				--If balance is less than amount to reserve, release lock and raise error
				else
					
					begin
						update ln.saving set accountLocked = 0, lockedBy=null,lockDate=null where savingID = @savingId and accountLocked = 1 
						and lockedBy = @reservedBy and DATEDIFF(MINUTE, lockDate, @lockedDateTime)<1

					raiserror ('Not enough balance',16,16)
				end
			end
		end
		
	commit transaction
end try
begin catch
	rollback
	raiserror ('An Error occured',16,16)
end catch



