use coreDB
go

CREATE FUNCTION dbo.acc_head_walk
( 
	@accID int,
	@levelNeeded int,
	@currentHeadID int = null,
	@currentLevel int = 0 
)
RETURNS nvarchar(250)
with encryption
AS
	BEGIN
		declare @rtr nvarchar(250)
		
		if @levelNeeded < 0
			select @rtr = null
		else
		begin
			if @currentHeadID is null 
			begin
				select @currentHeadID = acct_head_id
				from accts
				where acct_id = @accID
			end 
			
			if @currentLevel = @levelNeeded
				select @rtr = head_name
				from acct_heads
				where acct_head_id = @currentHeadID
			else
			begin
				select @currentHeadID = parent_acct_head_id,
					@currentLevel = @currentLevel + 1
				from acct_heads
				where acct_head_id = @currentHeadID
				
				select @rtr = dbo.acc_head_walk(@accID, @levelNeeded, @currentHeadID, @currentLevel)
			end 
		end
		
		RETURN @rtr
	END
 