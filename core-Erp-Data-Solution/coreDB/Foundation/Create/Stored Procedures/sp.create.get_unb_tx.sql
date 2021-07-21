use coreDB
go


alter PROCEDURE get_unb_tx
(
	@start_date datetime,
	@end_date datetime
)
with encryption
AS
BEGIN
declare 
	@tbl TABLE 
(
	batch_no nvarchar(50), 
	crdt_amt decimal, 
	dbt_amt decimal, 
	creation_date datetime, 
	creator nvarchar(50),
	source nvarchar(10)
)
	insert into @tbl
	SELECT        b.batch_no, SUM(j.crdt_amt) AS crdt_amt, SUM(j.dbt_amt) AS dbt_amt, 
		max(b.creation_date) creation_date, max(b.creator) creator, max(b.source) source
FROM            jnl AS j INNER JOIN
                         jnl_batch AS b ON j.jnl_batch_id = b.jnl_batch_id
inner join
(
	select jnl_batch_id
	from jnl
	WHERE creation_date between @start_Date and @end_Date
	group by jnl_batch_id
	HAVING        (ROUND(SUM(crdt_amt), 2) <> ROUND(SUM(dbt_amt), 2))
) tbl on j.jnl_batch_id=tbl.jnl_batch_id
GROUP BY b.batch_no 
	
	select * from @tbl
END
GO