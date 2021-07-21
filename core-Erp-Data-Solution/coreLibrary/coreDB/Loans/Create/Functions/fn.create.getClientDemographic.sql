use coreDB
go

create function ln.getClientAge
(
	@clientId int,
	@date datetime
)
returns int
as
begin
	declare @age int

	select @age = datediff(yy, dob, @date)
	from ln.client
	where clientId = @clientId

	select @age = case when @age<31 then 30
					when @age < 41 then 40
					when @age <51 then 50
					else 51 end

	return @age
end
go
use coreDB
go

create function ln.getLoanBand
(
	@loanId int
)
returns int
as
begin
	declare @band int

	select @band = amountDisbursed
	from ln.loan
	where loanId = @loanId

	select @band = case when @band<501 then 500
					when @band < 1001 then 1000
					when @band <1501 then 1500
					when @band <3001 then 3000
					else 3001 end

	return @band
end
go

create function ln.getLoanPrincBal
(
	@loanId int,
	@date datetime
)
returns float
as
begin
	declare @bal float, @paid float

	select @bal = amountDisbursed
	from ln.loan
	where loanId = @loanId

	select @paid = sum(principalPaid)
	from ln.loanRepayment
	where loanId=@loanId
		and repaymentDate <= @date

	select @bal = @bal - isnull(@paid, 0)

	return @bal
end
go
