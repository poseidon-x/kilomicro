use coreDB
go

CREATE FUNCTION dbo.gl_ou_level
(  
	@currentOUID int,
	@currentLevel int = 0 
)
RETURNS int
with encryption
AS
	BEGIN
		declare @rtr int
		
		select @currentOUID = parent_ou_id,
			@currentLevel = @currentLevel + 1
		from gl_ou
		where ou_id = @currentOUID
		
		if @currentOUID is null 
		begin
			select @rtr = @currentLevel
		end 
		else
		begin
			select @rtr = dbo.gl_ou_level(@currentOUID, @currentLevel)
		end 
		
		RETURN @rtr
	END
