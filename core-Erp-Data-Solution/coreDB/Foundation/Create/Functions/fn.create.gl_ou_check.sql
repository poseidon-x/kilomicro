use coreDB
go

CREATE FUNCTION dbo.gl_ou_check
( 
	@ouID int,
	@userName nvarchar(50) 
)
RETURNS bit
with encryption
AS
	BEGIN
		declare @rtr bit, @found bit
		
		select @found = allow
		from user_gl_ou_gl_ou g
		where g.cost_center_id = @ouID
			and g.USER_NAME = @userName

		if @found is null
		begin 
			declare @pOUID int
			select @pOUID= MAX(parent_ou_id)
			from gl_ou
			where ou_id = @ouID

			if (@pOUID is not null)
				select @rtr = dbo.gl_ou_check(@pOUID, @userName)
			else
				select @rtr = 0
		end
		else
			select @rtr = @found
		
		RETURN @rtr
	END
 