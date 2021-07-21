use coreDB
go

CREATE FUNCTION tr.transfer_qty
(
	@transfer_id int, 
	@bank_id int,
	@district_id int 
)
RETURNS INT
AS
BEGIN
	declare @qty int

	select @qty = SUM(num_of_bags)
	from tr.transfer_dtl td inner join tr.district_acct da on td.district_acct_id = da.district_acct_id
	where td.transfer_id = @transfer_id and da.bank_id = @bank_id and td.district_id = @district_id

	return @qty

END