use coreDB
go

CREATE FUNCTION dbo.acc_head_level
(  
	@currentHeadID int,
	@currentLevel int = 0 
)
RETURNS int
with encryption
AS
	BEGIN
		declare @rtr int
		
		select @currentHeadID = parent_acct_head_id,
			@currentLevel = @currentLevel + 1
		from acct_heads
		where acct_head_id = @currentHeadID
		
		if @currentHeadID is null 
		begin
			select @rtr = @currentLevel
		end 
		else
		begin
			select @rtr = dbo.acc_head_level(@currentHeadID, @currentLevel)
		end 
		
		RETURN @rtr
	END
