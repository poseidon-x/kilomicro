
CREATE Procedure change_acc_parent2
	( 
		@accountID int,
		@headerID int
	)
AS
begin
	update accts set acct_head_id = @headerID where acct_id = @accountID
	
end

GO

CREATE Procedure change_acc_parent
	(
		@sourceType nchar(1),
		@destType nchar(1),
		@sourceID int,
		@destID int
	)
with encryption
AS
begin
	if @sourceType = 'a' and @destType = 'h'
	begin
		exec change_acc_parent2 @sourceID, @destID
	end
	else if @sourceType = 'h'
	begin
		if @destType = 'h'
		begin
			print 'not implemented'
		end
		else if @destType = 'c'
		begin
			print 'not implemented'
		end
	end
	
end

GO
