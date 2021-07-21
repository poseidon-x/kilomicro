use coreDB
go

CREATE View vw_jnl
with encryption  AS

SELECT TOP (100) PERCENT b.batch_no, b.multi_currency, b.source, b.posted, 
	max(j.tx_date) as tx_date, SUM(j.crdt_amt) as crdt_amt, SUM(j.dbt_amt) as dbt_amt,
	 SUM(j.frgn_crdt_amt) as frgn_crdt_amt, SUM(j.frgn_dbt_amt) as frgn_dbt_amt,
	 b.jnl_batch_id, b.creator,isnull(b.companyId,0)as companyID , isnull(com.comp_name,'')as comp_Name
FROM dbo.jnl_batch AS b INNER JOIN
    dbo.jnl AS j ON j.jnl_batch_id = b.jnl_batch_id Left Outer Join
	dbo.comp_prof com ON  com.companyId = b.companyId
GROUP BY b.batch_no, b.multi_currency, b.source, b.posted, 
	b.jnl_batch_id, b.creator, isnull(b.companyId,0), isnull(com.comp_name,'')
order by 	b.batch_no


GO
 