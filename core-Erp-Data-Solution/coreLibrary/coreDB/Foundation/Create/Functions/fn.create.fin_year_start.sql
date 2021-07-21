 use coreDB
go


-- =============================================
CREATE FUNCTION fin_year_start
(
	@date datetime
)
RETURNS datetime
with encryption
AS
BEGIN
	declare @d datetime, @fm int, @m tinyint, @y smallint
	
	select @fm = fmoy
	from comp_prof
	
	select @m = month(@date),@y= year(@date)
	if @fm<>0 and @m < @fm
	begin
		select @y = @y-1
	end
	select @d = cast(cast(@y as nvarchar(4))+ '-' + right('0000000' + cast(@fm as nvarchar(2)), 2)+ '-01'  as datetime)
		
	return @d
END
GO


