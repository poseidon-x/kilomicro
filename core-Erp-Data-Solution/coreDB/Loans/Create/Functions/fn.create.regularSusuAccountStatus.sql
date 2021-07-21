use coreDB
go

create function ln.regularSusuAccountStatus
(
	@regularSusuAccountID int,
	@date datetime
)
returns tinyint
with encryption
as
begin

	declare @status tinyint
	declare @noContribMade int, @contribMade float
	declare @noContribExpected int, @contribExpected float, @lastContribDate datetime
	declare @noDefaulted int
	declare 
		@entitledDate datetime,
		@dueDate datetime,
		@contributionAmount float,
		@maxDefaultDays int,
		@percentageInterest float,
		@netAmountEntitled float,
		@interestAmount float,
		@regularSusCommissionAmount float,
		@regularSusuDaysInPeriod int,
		@regularSusuPeriodsInCycle int,
		@regularSusuDaysDeductedPerPeriod int,
		@disbursed bit,
		@approvalDate datetime,
		@allPeriodsDone float

	select
		@entitledDate = entitledDate,
		@dueDate = dueDate,
		@contributionAmount = contributionAmount,
		@maxDefaultDays = 31,
		@netAmountEntitled = netAmountEntitled,
		@interestAmount = interestAmount,
		@regularSusCommissionAmount = regularSusCommissionAmount,
		@regularSusuDaysInPeriod = regularSusuDaysInPeriod,
		@regularSusuPeriodsInCycle = regularSusuPeriodsInCycle,
		@regularSusuDaysDeductedPerPeriod = regularSusuDaysDeductedPerPeriod,
		@disbursed = case when approvalDate is not null and posted = 1 then 1 else 0 end,
		@approvalDate = approvalDate
	from ln.regularSusuAccount sa 
		cross join ln.susuConfig
	where sa.regularSusuAccountID = @regularSusuAccountID

	select 
		@lastContribDate = max(contributionDate)
	from ln.regularSusuContribution sc
	where sc.regularSusuAccountID = @regularSusuAccountID
		and sc.contributionDate <= @date

	select 
		@noContribExpected=isnull(count(regularSusuContributionScheduleID), 0),
		@contribExpected = isnull(sum(amount), 0),
		@noDefaulted = isnull(count(case when sc.balance>0 then 1 else null end), 0),
		@noContribMade = isnull(count(case when balance <= 0 then regularSusuContributionScheduleID else null end), 0),		
		@contribMade = isnull(sum(case when balance <= 0 then amount else 0 end), 0)
	from ln.regularSusuContributionSchedule sc
	where sc.regularSusuAccountID = @regularSusuAccountID
		and sc.plannedContributionDate <= @date
						
	select 
		@allPeriodsDone = (isnull(sum(case when balance <= 0 then amount else 0 end), 0)
							/cast(@contributionAmount as float))/cast(@regularSusuDaysInPeriod as float)	from ln.regularSusuContributionSchedule sc
	where sc.regularSusuAccountID = @regularSusuAccountID

	if @noDefaulted < 1 and @disbursed=0 and @allPeriodsDone < @regularSusuPeriodsInCycle * @regularSusuDaysInPeriod
			select @status = 1 -- OkayUndisbursed White
	else if @allPeriodsDone >= @regularSusuPeriodsInCycle * @regularSusuDaysInPeriod and @disbursed=0
		select @status = 4 -- DueUnUndisbursed Yellow
	else if @disbursed = 0 and @contribMade < @contribExpected and @noDefaulted between 1 and @maxDefaultDays-1
		select @status = 2 -- DelayedUndisbursed Red
	else if @disbursed = 0 and @contribMade < @contribExpected and @noDefaulted >= @maxDefaultDays
		select @status = 3 -- DormantUndisbursed Red
	else if (@approvalDate is null)
		select @status = 5 --Unapproved White
	else if @disbursed = 1 and @contribMade >= @contribExpected
		select @status = 6 -- OkayDisbursed Green
	else if @disbursed = 1 and @contribMade < @contribExpected and @noDefaulted > 1
		select @status = 7 -- DisbursedDormant Orange
	else  
		select @status = 8 -- Exited Blue 

	return @status

end
go
