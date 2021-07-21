use coreDB
go

drop function tr.transfer_amt
go

CREATE FUNCTION tr.transfer_amt
(
	@transfer_id int, 
	@bank_id int,
	@district_id int,
	@type int
)
RETURNS float
AS
BEGIN
	declare @amt float

	select @amt = SUM(case when @type=1 then pprice when @type=2 then commission_amt when @type=3 then total_amt end)
	from tr.transfer_dtl td inner join tr.district_acct da on td.district_acct_id = da.district_acct_id
	where td.transfer_id = @transfer_id and da.bank_id = @bank_id and td.district_id = @district_id

	return @amt

END