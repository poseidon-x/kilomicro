use coreDB
go

CREATE FUNCTION dbo.gl_ou_walk
( 
	@ouID int,
	@levelNeeded int,
	@currentOUID int = null,
	@currentLevel int = 0,
	@current nvarchar(4000) = '' 
)
RETURNS nvarchar(250)
with encryption
AS
	BEGIN
		declare @rtr nvarchar(250)
		
		if @levelNeeded < 0
			select @rtr = @current
		else
		begin
			if @currentOUID is null 
			begin
				select @currentOUID = ou_id
				from gl_ou
				where ou_id = @ouID
			end 
			
			select @rtr = case when @current='' then ou_name else  ou_name + ' | ' + @current end
			from gl_ou
			where ou_id = @currentOUID
			if @currentLevel < @levelNeeded 
			begin
				select @currentOUID = parent_ou_id,
					@currentLevel = @currentLevel + 1
				from gl_ou
				where ou_id = @currentOUID
				
				select @rtr = dbo.gl_ou_walk(@ouID, @levelNeeded, @currentOUID, @currentLevel, @rtr)
			end 
		end
		
		RETURN @rtr
	END
 