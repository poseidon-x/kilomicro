use coreDB
go

set xact_abort on

begin tran

declare @accountNumber nvarchar(50),
	@firstName nvarchar(60),
	@lastName nvarchar(50),
	@sex nchar(1),
	@dateofBirth datetime,
	@idNumber nvarchar(50),
	@startDate datetime,
	@rate float,
	@repayment int,
	@amountPaid float,
	@posted bit,
	@disbursed bit,
	@loanAmount float,
	@fees float,
	@loanType int,
	@tenure float,
	@loanNo int
declare curLoan cursor for
select
	accountNumber,
	loanNo,
	firstName,
	lastName,
	sex,
	dateofBirth,
	idNumber,
	startDate,
	rate,
	repayment,
	amountPaid,
	posted,
	disbursed,
	loanAmount,
	fees,
	loanType,
	tenure
from dbo.loanOpeningBalance
where accountNumber is not null

open curLoan
fetch next from curLoan into
	@accountNumber,
	@loanNo,
	@firstName,
	@lastName,
	@sex,
	@dateofBirth,
	@idNumber,
	@startDate,
	@rate,
	@repayment,
	@amountPaid,
	@posted,
	@disbursed,
	@loanAmount,
	@fees,
	@loanType,
	@tenure
while @@FETCH_STATUS=0
begin
	declare @clientID int
	declare @loanID int, @balance float
	 
	select @balance = @loanAmount
	select @clientID = MIN(clientID)
	from ln.client
	where accountNumber=ltrim(rtrim(@accountNumber))

	if (@clientID is  null)
	begin
		declare @idNoID int
		INSERT INTO [ln].[idNo]
			([idNoTypeID]
			,[idNo]
			,[expriryDate]
			,[sortCode])
		VALUES
			(2
			,@idNumber
			,NULL
			,1)
		select @idNoID = @@IDENTITY

		INSERT INTO [ln].[client]
				   ([accountNumber]
				   ,[surName]
				   ,[otherNames]
				   ,[DOB]
				   ,[maritalStatusID]
				   ,[sex]
				   ,[industryID]
				   ,[sectorID]
				   ,[branchID]
				   ,[categoryID]
				   ,[idNoID]
				   ,[creation_date]
				   ,[creator]
				   ,[modification_date]
				   ,[last_modifier]
				   ,[clientTypeID]
				   ,[idNoID2]
				   ,[companyName]
				   ,[isCompany]
				   ,[secondSurName]
				   ,[secondOtherNames]
				   ,[thirdSurName]
				   ,[thrifOtherNames]
				   ,[accountName])
			 VALUES
				   (@accountNumber
				   ,@lastName
				   ,@firstName
				   ,@dateofBirth
				   ,2
				   ,@sex
				   ,1006
				   ,7
				   ,6
				   ,1
				   ,@idNoID
				   ,getDate()
				   ,'SYSTEM'
				   ,getdate()
				   ,'SYSTEM'
				   ,1
				   ,NULL
				   ,''
				   ,0
				   ,''
				   ,''
				   ,''
				   ,''
				   ,'')
			select @clientID=@@IDENTITY
		end
		
		declare @countOfAccounts int
		select @countOfAccounts=count(*)+1
		from ln.loan
		where @clientID = clientID
		declare @loanAccountNo nvarchar(30) 
		select @loanAccountNo = @accountNumber + '/' + cast(@countOfAccounts as nvarchar(30))

		INSERT INTO [ln].[loan]
           ([clientID]
           ,[loanNo]
           ,[amountRequested]
           ,[amountApproved]
           ,[amountDisbursed]
           ,[applicationDate]
           ,[finalApprovalDate]
           ,[loanStatusID]
           ,[interestRate]
           ,[interestTypeID]
           ,[gracePeriod]
           ,[repaymentModeID]
           ,[affordabilityRatio]
           ,[applicationFee]
           ,[processingFee]
           ,[loanTypeID]
           ,[creation_date]
           ,[creator]
           ,[modification_date]
           ,[last_modifier]
           ,[disbursementDate]
           ,[loanTenure]
           ,[tenureTypeID]
           ,[commission]
           ,[balance]
           ,[applicationFeeBalance]
           ,[processingFeeBalance]
           ,[commissionBalance]
           ,[creditOfficerNotes]
           ,[lastPenaltyDate]
           ,[approvalComments]
           ,[addFeesToPrincipal]
           ,[invoiceNo]
           ,[edited]
           ,[staffID]
           ,[agentID]
           ,[approvedBy]
           ,[checkedBy]
           ,[enteredBy]
           ,[disbursedBy]
           ,[insuranceAmount]
           ,[creditManagerNotes]
           ,[lastEOD])
     VALUES
           (@clientID
           ,@loanAccountNo
           ,@loanAmount
           ,@loanAmount
           ,@loanAmount
           ,@startDate
           ,@startDate
           ,case when @disbursed=1 then 4 else 3 end
           ,@rate
           ,6
           ,0
           ,@repayment
           ,0.0
           ,0.0
           ,@fees
           ,@loanType
           ,getDate()
           ,'SYSTEM'
           ,getDate()
           ,'SYSTEM'
           ,@startDate
           ,@tenure
           ,1
           ,0.0
           ,@loanAmount
           ,0
           ,case when @disbursed=1 then 0 else @fees end
           ,0
           ,''
           ,NULL
           ,''
           ,0
           ,''
           ,0
           ,NULL
           ,NULL
           ,'SYSTEM'
           ,'SYSTEM'
           ,'SYSTEM'
           ,'SYSTEM'
           ,0
           ,''
           ,NULL)
    select @loanID = @@IDENTITY

	if @fees>0 and @disbursed=1
	begin
		insert into ln.loanRepayment
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
           ,@startDate
           ,@fees
           ,0
           ,0
           ,@fees
           ,0
           ,getDate()
           ,'SYSTEM'
           ,getDate()
           ,'SYSTEM'
           ,0
           ,NULL
           ,NULL
           ,NULL)
		INSERT INTO [ln].[loanFee]
           ([loanID]
           ,[feeDate]
           ,[feeAmount]
           ,[feeTypeID]
           ,[creation_date]
           ,[creator]
           ,[modification_date]
           ,[last_modifier])
		VALUES
           (@loanID
           ,@startDate
           ,@fees
           ,1
           ,getDate()
           ,'SYSTEM'
           ,getDate()
           ,'SYSTEM')
	end

	declare curSchedule cursor for
	select
		repaymenDate,
		principal,
		interest,
		penalty
	from dbo.loanOpeningSchedule
	where ltrim(rtrim(accountNumber)) = @accountNumber and loanNo = @loanNo
	declare @repaymenDate datetime,
		@principal float,
		@interest float,
		@penalty float
	open curSchedule

	fetch next from curSchedule into 
		@repaymenDate,
		@principal,
		@interest,
		@penalty
	while @@FETCH_STATUS=0
	begin
		INSERT INTO [ln].[repaymentSchedule]
           ([loanID]
           ,[repaymentDate]
           ,[interestPayment]
           ,[principalPayment]
           ,[balanceBF]
           ,[balanceCD]
           ,[interestBalance]
           ,[principalBalance]
           ,[creation_date]
           ,[creator]
           ,[modification_date]
           ,[last_modifier]
           ,[proposedInterestWriteOff]
           ,[interestWritenOff]
           ,[edited]
           ,[origInterestPayment]
           ,[additionalInterest]
           ,[origPrincipalCD]
           ,[origPrincipalBF]
           ,[penaltyAmount]
           ,[origPrincipalPayment]
           ,[additionalInterestBalance])
		VALUES
           (@loanID
           ,@repaymenDate
           ,@interest
           ,@principal
           ,@balance
           ,@balance-@principal
           ,@interest
           ,@principal
           ,getDate()
           ,'SYSTEM'
           ,getDate()
           ,'SYSTEM'
           ,0
           ,0
           ,0
           ,@interest
           ,0
           ,@balance
           ,@balance-@principal
           ,@penalty
           ,@principal
           ,0)
		if @penalty>0 
		begin
			INSERT INTO [ln].[loanPenalty]
			(
				[loanID]
			   ,[penaltyDate]
			   ,[penaltyFee]
			   ,[creation_date]
			   ,[creator]
			   ,[modification_date]
			   ,[last_modifier]
			   ,[penaltyBalance]
			   ,[proposedAmount])
			VALUES
			   (@loanID
			   ,@repaymenDate
			   ,@penalty
			   ,getDate()
			   ,'SYSTEM'
			   ,getDate()
			   ,'SYSTEM'
			   ,@penalty
			   ,0)
		end

		fetch next from curSchedule into 
			@repaymenDate,
			@principal,
			@interest,
			@penalty
	end

	close curSchedule
	deallocate curSchedule


	
	declare curRepayment cursor for
	select
		repaymenDate,
		principalPaid,
		interestPaid,
		penaltyPaid
	from dbo.loanOpeningRepayment
	where ltrim(rtrim(accountNumber)) = @accountNumber and loanNo = @loanNo
	declare  
		@principalPaid float,
		@interestPaid float,
		@penaltyPaid float
	open curRepayment

	fetch next from curRepayment into 
		@repaymenDate,
		@principalPaid,
		@interestPaid,
		@penaltyPaid
	while @@FETCH_STATUS=0
	begin
		declare curRS cursor for
		select repaymentScheduleID,
			principalBalance,
			interestBalance
		from ln.repaymentSchedule
		where loanID = @loanID and (principalbalance>0 or interestBalance>0)
		order by repaymentDate
		declare 
			@repaymentScheduleID int,
			@principalBalance float,
			@interestBalance float,
			@princBal float,
			@intBal float,
			@intAmt float,
			@princAmt float
		select @princBal = @principalPaid, @intBal = @interestPaid

		open curRS
		fetch next from curRS into
			@repaymentScheduleID,
			@principalBalance,
			@interestBalance
		while @@FETCH_STATUS=0 and (@princBal>0 or @intBal>0)
		begin
			if(@princBal>@principalBalance)
				select @princAmt = @principalBalance
			else
				select @princAmt = @princBal
			if(@intBal>@interestBalance)
				select @intAmt = @interestBalance
			else
				select @intAmt = @intBal
			select @princBal = @princBal - @princAmt, @intBal = @intBal - @intAmt
			update ln.repaymentSchedule
			set principalBalance = principalBalance - @princAmt,
					interestBalance = interestBalance - @intAmt
			where @repaymentScheduleID = repaymentScheduleID

			fetch next from curRS into
				@repaymentScheduleID,
				@principalBalance,
				@interestBalance
		end
		close curRS
		deallocate curRS

		declare curPen cursor for
		select
			loanPenaltyID,
			penaltyBalance
		from ln.loanPenalty 
		where loanID = @loanID and penaltyBalance > 0
		order by penaltyDate
		declare @loanPenaltyID int, @penaltyBalance float, @penBal float, @penAmt float
		select @penBal=@penaltyPaid
		open curPen
		fetch next from curPen into
			@loanPenaltyID,
			@penaltyBalance
		while @@FETCH_STATUS=0
		begin
			if @penBal> @penaltyBalance
				select @penAmt = @penaltyBalance
			else
				select @penAmt = @penBal
			select @penBal = @penBal - @penAmt
			update ln.loanPenalty
			set penaltyBalance = penaltyBalance - @penAmt
			where loanPenaltyID = @loanPenaltyID

			fetch next from curPen into
				@loanPenaltyID,
				@penaltyBalance
		end
		close curPen
		deallocate curPen

		if (@principalPaid>0 or @interestPaid>0)
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
			   ,1
			   ,@repaymenDate
			   ,@principalPaid+@interestPaid
			   ,@interestPaid
			   ,@principalPaid
			   ,0
			   ,0
			   ,getDate()
			   ,'SYSTEM'
			   ,getDate()
			   ,'SYSTEM'
			   ,0
			   ,NULL
			   ,NULL
			   ,NULL)
			update ln.loan set balance = balance - @principalPaid
			where @loanID = loanID
		end
		if @penaltyPaid>0
			insert into ln.loanRepayment
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
			   ,7
			   ,@repaymenDate
			   ,@penaltyPaid
			   ,0
			   ,0
			   ,0
			   ,@penaltyPaid
			   ,getDate()
			   ,'SYSTEM'
			   ,getDate()
			   ,'SYSTEM'
			   ,0
			   ,NULL
			   ,NULL
			   ,NULL)

		fetch next from curRepayment into 
			@repaymenDate,
			@principal,
			@interest,
			@penalty
	end

	close curRepayment
	deallocate curRepayment

	fetch next from curLoan into
		@accountNumber,
		@loanNo,
		@firstName,
		@lastName,
		@sex,
		@dateofBirth,
		@idNumber,
		@startDate,
		@rate,
		@repayment,
		@amountPaid,
		@posted,
		@disbursed,
		@loanAmount,
		@fees,
		@loanType,
		@tenure
end

close curLoan
deallocate curLoan

commit;
