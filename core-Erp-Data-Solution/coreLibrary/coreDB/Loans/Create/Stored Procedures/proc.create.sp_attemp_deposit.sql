USE [coreDB]
GO
/****** Object:  StoredProcedure [ln].[sp_attempt_deposit]    Script Date: 8/9/2017 8:25:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER proc [ln].[sp_attempt_deposit]
	@savingId int,
	@savingAmount float,
	@reservedBy nvarchar(30),
	@savingDate datetime,
	@bankId int null,
	@checkNo nvarchar(30) null,
	@modeOfPaymentId int,
	@naration nvarchar(100),
	@transactionId nvarchar(60),
	@savingAdditionalId nvarchar(15) out
as
	declare @isAccountLocked bit = 0
	declare @DepositReservation float
	declare @isFundsReserved bit = 1

begin try
begin
	set transaction isolation level repeatable read
	begin transaction
		
		--Check for reservation
		select @isFundsReserved = [committed] from ln.savingReservationTransc 
												where transactionId = @transactionId 
												and savingID = @savingId 
												and reservedBy = @reservedBy
												and reservationTypeId = 2
												and (DATEDIFF(MINUTE, reservationDate, GETDATE())<2)

		select @isAccountLocked = accountLocked from ln.saving where savingID = @savingId
		--Lock Account and withdraw funds if not locked
		if(@isFundsReserved = 0)
			begin

			declare @principalBal float
			declare @avaialablePrincipalBal float
			declare @amountInvested float

			select @principalBal = principalBalance from ln.saving where savingID = @savingId
			select @avaialablePrincipalBal = availablePrincipalBalance from ln.saving where savingID = @savingId
			select @amountInvested = amountInvested from ln.saving where savingID = @savingId

			--Proceed to deposit funds if lock succeed
			if(@savingAmount > 0)
			begin
					declare @interestBalance float = 0
					select @principalBal = principalBalance from ln.saving where savingID = @savingId	
					select @interestBalance = interestBalance from ln.saving where savingID = @savingId	
								
					--Reserve fund
					update ln.saving set principalBalance += @savingAmount,availablePrincipalBalance += @savingAmount,amountInvested += @savingAmount,
					depositReservation -= @savingAmount
					where savingID = @savingId 

					insert into ln.savingAdditional(savingId,savingDate,savingAmount,principalBalance,interestBalance,creation_date,creator,
							bankID,checkNo,modeOfPaymentID,fxRate,localAmount,lastPrincipalFxGainLoss,posted,naration,closed,balanceBD)
					values(@savingId,@savingDate,@savingAmount,(@principalBal+@savingAmount),@interestBalance,GETDATE(),@reservedBy,@bankId,
						@checkNo,@modeOfPaymentId,0,@savingAmount,0,0,@naration,0,(@principalBal+@interestBalance))

					 select @savingAdditionalId = cast(SCOPE_IDENTITY() as nvarchar(15))	
					 
					 update ln.savingReservationTransc set [committed] = 1, committedDate = GETDATE()
					 where transactionId = @transactionId and savingID = @savingId  and reservedBy = @reservedBy
					 and reservationTypeId = 2 and (DATEDIFF(MINUTE, reservationDate, GETDATE())<2)					 							
			end
			else 
			raiserror ('Deposit amount cannot be zero or less',16,16)
		end
		
	commit transaction
	select @savingAdditionalId
	end
end try
begin catch
	rollback
	select 'An Error occured' 
end catch





