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
	@position int,
	@amountContributed float,
	@posted bit,
	@disbursed bit
declare curSusu cursor for
select
	accountNumber,
	firstName,
	lastName,
	sex,
	dateofBirth,
	idNumber,
	startDate,
	rate,
	position,
	amountContributed,
	posted,
	disbursed
from dbo.susuOpeningBalance
where accountNumber is not null

open curSusu
fetch next from curSusu into
	@accountNumber,
	@firstName,
	@lastName,
	@sex,
	@dateofBirth,
	@idNumber,
	@startDate,
	@rate,
	@position,
	@amountContributed,
	@posted,
	@disbursed
while @@FETCH_STATUS=0
begin
	declare @clientID int
	declare @idNoID int
	declare @susuAccountID int
	declare @amountEntitled float
	declare @commission float
	declare @interestDed float

	select @clientID = MIN(clientID)
	from ln.client
	where accountNumber=ltrim(rtrim(@accountNumber))

	if (@clientID is  null)
	begin
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
		else
		begin
			select @clientID = @clientID
			from ln.client
			where @accountNumber=accountNumber
		end
		declare @percentageInterest float, @noOfWaitingPeriods int, @positionID int, @gradeID int
		select @percentageInterest=[percentageInterest], @noOfWaitingPeriods=[noOfWaitingPeriods],
			@positionID=susuPositionID
		from [ln].[susuPosition]
		where [susuPositionNo]=@position

		select @gradeID=susuGradeID
		from ln.susuGrade
		where [contributionAmount] = @rate

		declare @dt datetime, @j int, @flag bit, @i int
		declare @dueDate datetime, @expiryDate datetime

		select @dt = @startDate; 
        select @j = 1, @flag=1;
        while (@flag=1)
        begin 
            if (datepart(dw, @dt)=1)
				select @dt = dateadd(day, 1, @dt);
			else
			begin
				select @j = @j + 1;
				if (@j > @noOfWaitingPeriods * 31) select @flag=0;
				select @dt= dateadd(day, 1, @dt);
			end
        end
		select @dueDate = @dt

		if(@position=6)
			select @expiryDate = @dueDate
		else
		begin
			select @dt = @startDate; 
			select @i=0
			while @i<6
			begin
				select @i=@i+1
				select @j = 1, @flag=1;
				while (@flag=1)
				begin 
					if (datepart(dw, @dt)=1)
						select @dt = dateadd(day, 1, @dt);
					else
					begin
						select @j = @j + 1;
						if (@j > 6 * 31) select @flag=0;
						select @dt= dateadd(day, 1, @dt);
					end
				end
			end
			select @expiryDate = @dt
		end

		declare @groupID int
		select @groupID=min(susuGroupID)
		from ln.susuGroup
		where susuGroupID not in
		(
			select susuGroupID
			from ln.susuAccount
			where susuPositionID = @positionID
		)
		if @groupID is null
		begin
			declare @groupNo int
			select @groupNo=isnull(max(susuGroupNo), 0) +1 from ln.susuGroup

			INSERT INTO [ln].[susuGroup]
				   ([susuGroupNo]
				   ,[susuGroupName])
			 VALUES
				   (@groupNo
				   ,'Group ' + cast(@groupNo as nvarchar(30)))
			select @groupID = @@IDENTITY
		end
		declare @countOfAccounts int
		select @countOfAccounts=count(*)+1
		from ln.susuAccount
		where @clientID = clientID
		declare @susuAccountNo nvarchar(30), @netAmount float
		select @susuAccountNo = @accountNumber + '/' + cast(@countOfAccounts as nvarchar(30))

		select @amountEntitled = 6 * @rate * 31
		select @commission = 1 * @rate * 6
		select @interestDed = (@amountEntitled-@commission) * (@percentageInterest / 100.0)
		select @netAmount = @amountEntitled - @interestDed - @commission
		INSERT INTO [ln].[susuAccount]
           ([clientID]
           ,[applicationDate]
           ,[startDate]
           ,[susuPositionID]
           ,[susuGradeID]
			,[dueDate]
			,[disbursementDate]
			,[amountEntitled]
			,[amountTaken]
			,[balance]
			,[approvalDate]
			,[enteredBy]
			,[verifiedBy]
			,[approvedBy]
			,[staffID]
			,[agentID]
			,[loanID]
			,[contributionAmount]
			,[disbursedBy]
			,[entitledDate]
			,[susuAccountNo]
			,[susuGroupID]
			,[netAmountEntitled]
			,[interestAmount]
			,[modeOfPaymentID]
			,[checkNo]
			,[bankID]
			,[posted]
			,[authorized]
			,[commissionAmount]
			,[regularSusCommissionAmount]
			,[exited]
			,[exitDate]
			,[exitApprovedBy]
			,[commissionPaid]
			,[interestPaid]
			,[principalPaid]
			,[lastEOD]
			,[isDormant]
			,[convertedToLoan]
			,[appliedToLoan]
			,[interestAmountApplied])
		VALUES
			(@clientID
			,@startDate
			,@startDate
			,@positionID
			,@gradeID
			,@dueDate
			,@startDate
			,@amountEntitled
			,case when @disbursed=1 then @netAmount else 0 end
			,case when @disbursed=1 then @netAmount else 0 end
			,@startDate
			,'SYSTEM'
			,'SYSTEM'
			,'SYSTEM'
			,NULL
			,NULL
			,NULL
			,@rate
			,'SYSTEM'
			,@dueDate
			,@susuAccountNo
			,@groupID
			,@netAmount
			,@interestDed
			,1
			,NULL
			,NULL
			,1
			,@disbursed
			,@commission
			,@commission
			,0
			,NULL
			,'SYSTEM'
			,case when @disbursed=1 then @commission else 0 end
			,case when @disbursed=1 then @interestDed else 0 end
			,case when @disbursed=1 then @netAmount else 0 end
			,NULL
			,0
			,0
			,0
			,0)
    select @susuAccountID = @@IDENTITY

	declare @scheduledBalance float
	select @scheduledBalance=@amountContributed
	select @dt = @startDate; 
	select @i=0
	while @i<6
	begin
		select @i=@i+1
		select @j = 1, @flag=1;
		while (@flag=1)
		begin 
			if (datepart(dw, @dt)=1)
				select @dt = dateadd(day, 1, @dt);
			else
			begin
				declare @amtPaid float
				if @scheduledBalance>@rate
					select @amtPaid=@rate
				else 
					select @amtPaid=@scheduledBalance
				select @scheduledBalance = @scheduledBalance - @amtPaid
				INSERT INTO [ln].[susuContributionSchedule]
					([susuAccountID]
					,[plannedContributionDate]
					,[actualContributionDate]
					,[balance]
					,[amount])
				VALUES
					(@susuAccountID
					,@dt
					,case when @amtPaid>0 then @dt else NULL end
					,@rate-@amtPaid
					,@rate)
				if @amtPaid>0
					INSERT INTO [ln].[susuContribution]
						   ([susuAccountID]
						   ,[contributionDate]
						   ,[modeOfPaymentID]
						   ,[amount]
						   ,[receiverType]
						   ,[agentID]
						   ,[staffID]
						   ,[posted]
						   ,[cashierUsername]
						   ,[appliedToLoan]
						   ,[checkNo]
						   ,[bankID])
					 VALUES
						   (@susuAccountID
						   ,@dt
						   ,1
						   ,@amtPaid
						   ,1
						   ,NULL
						   ,NULL
						   ,1
						   ,'SYSTEM'
						   ,0
						   ,NULL
						   ,NULL)
				select @j = @j + 1;
				if (@j > 6 * 31) select @flag=0;
				select @dt= dateadd(day, 1, @dt);
			end
		end
	end

	fetch next from curSusu into
		@accountNumber,
		@firstName,
		@lastName,
		@sex,
		@dateofBirth,
		@idNumber,
		@startDate,
		@rate,
		@position,
		@amountContributed,
		@posted,
	@disbursed
end
close curSusu
deallocate curSusu

commit;
