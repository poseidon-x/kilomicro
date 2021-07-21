use coreDB
go

CREATE View vw_unb_tx
with encryption  AS
SELECT        '' batch_no, 0.0000000000 AS crdt_amt, 
0.0000000000000 AS dbt_amt, getdate() creation_date, 
	'' creator, '' source

GO
 