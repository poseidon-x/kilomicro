use coreDB
go

drop function tr.bank_walk
go

CREATE FUNCTION tr.bank_walk
( 
	@transfer_id int, 
	@levelNeeded int 
)
RETURNS int
with encryption
AS
	BEGIN
		declare @rtr int
		
		select
			@rtr = bank_id
		from  
		(
			select bank_id, 
				row_number() over (order by bank_name asc) as rownum
			from
			(
				select distinct b.bank_id, c.bank_name
				from tr.transfer_dtl a inner join tr.district_acct b on a.district_acct_id = b.district_acct_id
					inner join dbo.banks c on b.bank_id = c.bank_id
				where a.transfer_id = @transfer_id
			) t
		) tbl
		where rownum = @levelNeeded
		
		RETURN @rtr
	END
 