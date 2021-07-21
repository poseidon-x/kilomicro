use coreDB
go

alter View vw_tx_by_acct
with encryption 
 AS
	select
		0 as jnl_id,
		'' acc_num,
		'' acc_name,
		'' head_name1,
		'' head_name2,
		'' head_name3,
		'' head_name4,
		0 cat_code,
		'' cat_name,
		'' major_name,
		'' major_symbol,
		isnull(cast(0 as float ),0) as loc_end_bal,
		isnull(cast(0 as float ),0) as frgn_end_bal,
		isnull(cast(0 as float ),0) as loc_beg_bal,
		isnull(cast(0 as float ),0) as frgn_beg_bal,
		'' minor_name,
		'' minor_symbol,
		0 acct_id,
		'' head_name5,
		'' head_name6,
		'' head_name7,
		0 currency_id,
		getdate() tx_date,
		'' description,
		'' ref_no,
		isnull(cast(0 as float ),0) dbt_amt,
		isnull(cast(0 as float ),0) crdt_amt,
		isnull(cast(0 as float ),0) frgn_crdt_amt,
		isnull(cast(0 as float ),0) frgn_dbt_amt,
		'' as cost_center,
		'' as currency,
		isnull(cast(0 as float ),0)  as debit,
		isnull(cast(0 as float ),0)  as credit,
		isnull(cast(0 as float ),0)  as end_bal,
		isnull(cast(0 as float ),0)  as beg_bal,
		'' batch_no,
		'' creator,
		getdate() creation_date


GO
 