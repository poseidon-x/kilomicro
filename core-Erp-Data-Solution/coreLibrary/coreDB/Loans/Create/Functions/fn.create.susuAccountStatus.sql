use coreDB
go

alter function ln.susuAccountStatus
(
	@susuAccountID int,
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
		@susuPositionID int,
		@contributionAmount float,
		@noOfWaitingPeriods int,
		@maxDefaultDays int,
		@percentageInterest float,
		@netAmountEntitled float,
		@interestAmount float,
		@commissionAmount float,
		@daysInPeriod int,
		@periodsInCycle int,
		@daysDeductedPerPeriod int,
		@disbursed bit,
		@approvalDate datetime,
		@allPeriodsDone float
	
	select
		@entitledDate = entitledDate,
		@dueDate = dueDate,
		@susuPositionID = sp.susuPositionID,
		@contributionAmount = contributionAmount,
		@noOfWaitingPeriods = noOfWaitingPeriods,
		@maxDefaultDays = maxDefaultDays,
		@percentageInterest = percentageInterest,
		@netAmountEntitled = netAmountEntitled,
		@interestAmount = interestAmount,
		@commissionAmount = commissionAmount,
		@daysInPeriod = daysInPeriod,
		@periodsInCycle = periodsInCycle,
		@daysDeductedPerPeriod = daysDeductedPerPeriod,
		@disbursed = case when approvalDate is not null and posted = 1 then 1 else 0 end,
		@approvalDate = approvalDate
	from ln.susuAccount sa inner join 
		ln.susuPosition sp on sa.susuPositionID = sp.susuPositionID
		cross join ln.susuConfig
	where sa.susuAccountID = @susuAccountID

	select 
		@lastContribDate = max(contributionDate)
	from ln.susuContribution sc
	where sc.susuAccountID = @susuAccountID
		and sc.contributionDate <= @date
	
	select 
		@noContribExpected=isnull(count(susuContributionScheduleID), 0),
		@contribExpected = isnull(sum(amount), 0),
		@noDefaulted = isnull(count(case when sc.balance>0 then 1 else null end), 0),
		@noContribMade = isnull(count(case when balance <= 0 then susuContributionScheduleID else null end), 0),		
		@contribMade = isnull(sum(case when balance <= 0 then amount else 0 end), 0)
	from ln.susuContributionSchedule sc
	where sc.susuAccountID = @susuAccountID
		and sc.plannedContributionDate <= @date
						
	select 
		@allPeriodsDone = (isnull(sum(case when balance <= 0 then amount else 0 end), 0)
							/cast(@contributionAmount as float))/cast(@daysInPeriod as float)	from ln.susuContributionSchedule sc
	where sc.susuAccountID = @susuAccountID

	if @noDefaulted < 1 and @disbursed=0 and @allPeriodsDone < @noOfWaitingPeriods
		select @status = 1 -- OkayUndisbursed White
	else if @allPeriodsDone >= @noOfWaitingPeriods and @disbursed=0
		select @status = 4 -- DueUnUndisbursed Yellow
	else if @disbursed = 0 and @contribMade < @contribExpected and @noDefaulted between 1 and @maxDefaultDays-1
		select @status = 2 -- DelayedUndisbursed Red
	else if @disbursed = 0 and @contribMade < @contribExpected and @noDefaulted >= @maxDefaultDays
		select @status = 3 -- DormantUndisbursed Indigo
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
