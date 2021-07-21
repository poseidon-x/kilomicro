use coreDB
go


-- =============================================
CREATE FUNCTION tr.bank_name
(
	@bank_id int
)
RETURNS nvarchar(250)
with encryption
AS
BEGIN
	declare @rtr nvarchar(250)
	
	select @rtr = bank_name
	from dbo.banks
	where bank_id = @bank_id

	return @rtr
END
GO


