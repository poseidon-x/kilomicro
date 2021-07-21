use coreDB
go


alter PROCEDURE get_acc_bal
(
	@acct_id int,
	@date datetime,
	@cost_center_id int
)
with encryption
AS
BEGIN 
	 
	select dbo.acc_bal(acct_id, cat_code, '1990-01-01', @date,@cost_center_id)
	from vw_accounts
	where acct_id = @acct_id
	  
END
GO