use coreDB
go

alter procedure ln.getLoanByDemographic
(
	@type int,
	@monthEndDate datetime
)
as
begin
	declare @monthStartDate datetime, @month int, @year int
	select @year=year(@monthEndDate), @month=month(@monthEndDate)
	select @monthStartDate = DATETIMEFROMPARTS(@year, @month, 1, 0,0,0,0);

	with lq as
	(
		select
			c.clientId,
			loanNo,
			dob,
			ln.getClientAge(c.clientId, @monthEndDate) as age,
			case when sex = 'F' then 'Female' else 'Male' end as gender,
			ln.getLoanBand(loanId) as loanBand,
			ln.getLoanPrincBal(loanId, @monthEndDate) as princBal,
			disbursementDate,
			amountDisbursed
		from ln.client c inner join ln.loan l on c.clientId = l.clientId
		where disbursementDate <= @monthEndDate
	)
	select
		case when @type = 1 then
			age 
		when @type=2 then
			loanBand
		end as demoSort,
		case when @type = 1 then
			case when age=30 then '18-30'
				when age=40 then '31-40'
				when age=50 then '41-50'
				else '51 and above'	
			end
		when @type=2 then
			case when loanBand=500 then '0.00 - 500.00'
				when loanBand=1000 then '501.00 - 1000.00'
				when loanBand=1500 then '1001.00 - 1500.00'
				when loanBand=3000 then '1501.00 - 3000.00'
				else '3001 and above'
			end
		end as demographic,
		count(case when gender='Female' 
			and (@type=1 or ( @monthStartDate<=disbursementDate and @monthEndDate>=disbursementDate))   then clientId else null end) as countOfFemales,
		count(distinct case when gender='Female' 
			and (@type=1 or ( @monthStartDate<=disbursementDate and @monthEndDate>=disbursementDate))   then clientId else null end) as uniqueCountOfFemales,
		count(case when gender='Male' 
			and (@type=1 or ( @monthStartDate<=disbursementDate and @monthEndDate>=disbursementDate))  then clientId else null end) as countOfMales,
		count(distinct case when gender='Male' 
			and (@type=1 or ( @monthStartDate<=disbursementDate and @monthEndDate>=disbursementDate))  then clientId else null end) as uniqueCountOfMales,
		sum(case when gender='Female' 
			and (@type=1 or ( @monthStartDate<=disbursementDate and @monthEndDate>=disbursementDate))  then amountDisbursed else 0 end) as disbAmtFemales,
		sum(case when gender='Male' 
			and (@type=1 or ( @monthStartDate<=disbursementDate and @monthEndDate>=disbursementDate))  then amountDisbursed else 0 end) as disbAmtMales,
		sum(case when gender='Female' then princBal else 0 end) as femalePrincBal,
		sum(case when gender='Male' then princBal else 0 end) as malePrincBal
	from lq
	group by 
		case when @type = 1 then
			age 
		when @type=2 then
			loanBand
		end ,
		case when @type = 1 then
			case when age=30 then '18-30'
				when age=40 then '31-40'
				when age=50 then '41-50'
				else '51 and above'	
			end
		when @type=2 then
			case when loanBand=500 then '0.00 - 500.00'
				when loanBand=1000 then '501.00 - 1000.00'
				when loanBand=1500 then '1001.00 - 1500.00'
				when loanBand=3000 then '1501.00 - 3000.00'
				else '3001 and above'
			end
		end
end
go